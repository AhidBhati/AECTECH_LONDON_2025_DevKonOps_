using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Linq;
using Autodesk.Revit.Attributes;
using System;
using System.Collections.Generic;


namespace hack25
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RevitOperator : IExternalCommand
    {
        // Global fields
        public static UIDocument _uidoc;
        public static Document _doc;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Initialize the globals from commandData
            _uidoc = commandData.Application.ActiveUIDocument;
            _doc = _uidoc.Document;

            //Open Ui 

            // populate Chats. 

            //To delete dummy 

            string chatId = "CHAT001";
            string title = "Clash on Level 2 Wall";
            string description = "There is a pipe clashing with a beam near grid B2.";
            string createdBy = "john.doe@company.com";
            string status = "Closed";
            DateTime created = DateTime.Now;
            string linkToChat = "http://example.com/chat/123";
            ReferencePoint location = new ReferencePoint(8, 8, 10);

            // Create new Chat object
            Chat newChat = Chat.CreateNewChat(chatId, title, description, createdBy, status, created, linkToChat, location);
            var listChat = new List<Chat>();
            listChat.Add(newChat);

            Chat.PopulateChatPool(listChat);

            // Call 3D view logic -- To delete
            OpenOrCreateDefault3DView();
            // Call navigation 3D view logic -- To delete
            var Point = new ReferencePoint(9, 9, 10);
            NavigateToChatLocation(Point);
            //place sphere
            //PlaceSphere(newChat, "https://www.revitapidocs.com/2019/e21471cd-63a4-c452-8c29-fad41362a59b.htm");
            
            Chat.UpdateChat(newChat, linkToChat);

            var DTable = Chat.ConvertToDataTable(listChat);



            

            return Result.Succeeded;
        }
        public void OpenOrCreateDefault3DView()
        {
            View3D viewToOpen = new FilteredElementCollector(_doc)
                .OfClass(typeof(View3D))
                .Cast<View3D>()
                .Where(v => !v.IsTemplate && v.Name == "{3D}")
                .FirstOrDefault();

            using (Transaction tx = new Transaction(_doc, "Open or Create {3D} View"))
            {
                tx.Start();

                if (viewToOpen == null)
                {
                    ViewFamilyType viewFamilyType = new FilteredElementCollector(_doc)
                        .OfClass(typeof(ViewFamilyType))
                        .Cast<ViewFamilyType>()
                        .FirstOrDefault(vft => vft.ViewFamily == ViewFamily.ThreeDimensional);

                    if (viewFamilyType != null)
                    {
                        viewToOpen = View3D.CreateIsometric(_doc, viewFamilyType.Id);
                        viewToOpen.Name = "{3D}";
                    }
                }

                tx.Commit();
            }

            if (viewToOpen != null)
            {
                _uidoc.ActiveView = viewToOpen;
            }
            else
            {
                TaskDialog.Show("Error", "Couldn't find or create a 3D view.");
            }
        }

        public void NavigateToChatLocation(ReferencePoint ChatLocation)
        {
            using (Transaction trans = new Transaction(_doc, "Navigate to Chat Location"))
            {
                trans.Start();

                var X_Coord = ChatLocation.X;
                var Y_Coord = ChatLocation.Y;
                var Z_Coord = ChatLocation.Z;

                XYZ pointToFocus = new XYZ(X_Coord, Y_Coord, Z_Coord);

                View3D currentView = _uidoc.ActiveView as View3D;

                if (currentView != null)
                {
                    double zoomSize = 7.5;
                    XYZ min = new XYZ(pointToFocus.X - zoomSize, pointToFocus.Y - zoomSize, pointToFocus.Z - zoomSize);
                    XYZ max = new XYZ(pointToFocus.X + zoomSize, pointToFocus.Y + zoomSize, pointToFocus.Z + zoomSize);

                    BoundingBoxXYZ boundingBox = new BoundingBoxXYZ();
                    boundingBox.Min = min;
                    boundingBox.Max = max;

                    // Set the section box for the view to focus on that region
                    currentView.SetSectionBox(boundingBox);
                    currentView.IsSectionBoxActive = true;

                    // Adjust view settings to zoom to the bounding box
                    _uidoc.ActiveView = currentView;  // Re-set the view so that it updates.
                }
                else
                {
                    TaskDialog.Show("Error", "Active view is not a 3D view.");
                }

                trans.Commit();
            }
        }

        public static void PlaceSphere(Chat chat, string url)
        {
            using (Transaction trans = new Transaction(_doc, "Place Sphere and Show URL"))
            {
                trans.Start();

                FamilySymbol sphereSymbol = null;

                // Get the sphere family symbol
                FilteredElementCollector collector = new FilteredElementCollector(_doc);
                collector.OfClass(typeof(FamilySymbol));

                foreach (FamilySymbol fs in collector)
                {
                    if (fs.Name.Contains("Chat_Ball"))
                    {
                        sphereSymbol = fs;
                        break;
                    }
                }

                if (sphereSymbol == null)
                {
                    TaskDialog.Show("Error", "Sphere family not found.");
                    return;
                }

                if (!sphereSymbol.IsActive)
                {
                    sphereSymbol.Activate();
                }

                var ChatLocation = chat.ChatLocation;
                XYZ pointToPlace = new XYZ(ChatLocation.X, ChatLocation.Y, ChatLocation.Z);

                // Check if there's already a family instance at the location
                bool isSphereExisting = false;
                FilteredElementCollector elementCollector = new FilteredElementCollector(_doc);
                elementCollector.OfClass(typeof(FamilyInstance));

                foreach (FamilyInstance fi in elementCollector)
                {
                    if (fi.Symbol.Name.Contains("Chat_Ball"))
                    {
                        LocationPoint locationPoint = fi.Location as LocationPoint;
                        if (locationPoint != null && locationPoint.Point.IsAlmostEqualTo(pointToPlace))
                        {
                            isSphereExisting = true;
                            break;
                        }
                    }
                }
                // If there's no existing sphere at the location, place the new one
                if (!isSphereExisting)
                {
                    FamilyInstance sphereInstance = _doc.Create.NewFamilyInstance(pointToPlace, sphereSymbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

                    // Set the URL parameter
                    Parameter urlParameter = sphereInstance.LookupParameter("Chat_URL");
                    if (urlParameter != null)
                    {
                        urlParameter.Set(url);
                    }

                    // Get the RevitId of the newly placed sphere
                    string revitId = sphereInstance.Id.ToString();

                    // Set the RevitId in the chat object
                    chat.RevitId = revitId;
                    
                }

                trans.Commit();
            }
        }

        public static void DeleteSphereByRevitId(string revitId)
        {
            using (Transaction trans = new Transaction(_doc, "Delete Sphere"))
            {
                trans.Start();
                TaskDialog.Show("Deleting", revitId);
            
                ElementId elementId = new ElementId(Convert.ToInt64(revitId)); 
                // Retrieve the element using the ElementId
                Element elementToDelete = _doc.GetElement(elementId);

                // Delete the element if it exists
                if (elementToDelete != null)
                {
                    _doc.Delete(elementId);
                    
                }
                else
                {
                    TaskDialog.Show("Error", "Element not found.");
                }

                trans.Commit();
            }
        }
    }
}
