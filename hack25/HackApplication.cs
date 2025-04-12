using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;

namespace hack25
{
    // This class implements IExternalApplication and is used to add a new Ribbon tab and button.
    public class HackApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Define the name of your custom ribbon tab.
            string tabName = "hack2025";

            // Create the ribbon tab. This call might throw if the tab already exists, so catch the exception.
            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch (Exception)
            {
                // If the tab already exists, you might log or ignore the exception.
            }

            // Create a ribbon panel on the newly created tab.
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "My Panel");

            // Define the button data with:
            // - The internal name (unique id) of the button: "satrt"
            // - The text that appears on the button: "satrt"
            // - The path to the currently executing assembly
            // - The full class name of the command to be executed
            PushButtonData buttonData = new PushButtonData(
                "start",
                "start",
                System.Reflection.Assembly.GetExecutingAssembly().Location,
                "hack25.HackCommand");

            // Optionally, set additional properties
            buttonData.ToolTip = "Click to display Hello World";

            // Add the push button to the ribbon panel.
            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            // Optionally, you can set a large or small icon here.
            // pushButton.LargeImage = ...;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // Perform any necessary cleanup here.
            return Result.Succeeded;
        }
    }

    // This class implements IExternalCommand and is invoked when the ribbon button is clicked.
    [Transaction(TransactionMode.Manual)]
    public class HackCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Display a simple Hello World message using Revit's TaskDialog.
            // Dummy values
            string chatId = "CHAT001";
            string title = "Clash on Level 2 Wall";
            string description = "There is a pipe clashing with a beam near grid B2.";
            string createdBy = "john.doe@company.com";
            string status = "Open";
            DateTime created = DateTime.Now;
            string linkToChat = "http://example.com/chat/123";
            ReferencePoint location = new ReferencePoint(10.5, 23.3, 5.0);

            // Create new Chat object
            Chat newChat = Chat.CreateNewChat(chatId, title, description, createdBy, status, created, linkToChat, location);


            TaskDialog.Show("Chat Info",
                             $"Title: {newChat.Title}\n" +
                             $"Description: {newChat.Description}\n" +
                             $"Status: {newChat.Status}\n" +
                             $"Created By: {newChat.CreatedBy}\n" +
                             $"Created At: {newChat.Created_Date_Time}\n" +
                             $"Link: {newChat.Link_To_Chat}");
            return Result.Succeeded;
        }
    }
}

