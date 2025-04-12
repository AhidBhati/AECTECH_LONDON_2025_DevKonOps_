using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace hack25
{
    public class Chat
    {
        //Global
        public static  List<Chat> ChatPool = new List<Chat>();

        //properties 
        public string ChatID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string[] Comment { get; set; }
        public string Status { get; set; }
        public DateTime Created_Date_Time { get; set; }
        public DateTime Last_Comment { get; set; }
        public string[]  IFCGUID { get; set; }
        public string Link_To_Chat { get; set; }
        public ReferencePoint ChatLocation { get; set; }
        public string RevitId { get; set; }


        public Chat(string chatId, string title, string description,string createdBy, string status, DateTime created, string linkToChat, ReferencePoint Point)
        {
            this.ChatID = chatId;
            this.Title = title;
            this.Description = description;
            this.Status = status;
            this.Created_Date_Time = created;         
            this.Link_To_Chat = linkToChat;
            this.CreatedBy = createdBy;
            this.ChatLocation = Point;
        }

        public static Chat CreateNewChat(string chatId, string title, string description, string createdBy, string status, DateTime created, string linkToChat, ReferencePoint Point)
        {
            // Create a chat on the backend web here: 

            // create a chat
            DateTime now = DateTime.Now;
            return new Chat( chatId,title,description,createdBy,status,created,linkToChat,Point);   
        }


        public static Chat UpdateChat(Chat UpdatedChat ,string ChatURL)
        {
            foreach (var item in ChatPool)
            {
                var currentChatId = item.ChatID;
                
                if (currentChatId == UpdatedChat.ChatID)
                {
                    //update all fields. 
                    item.Comment = UpdatedChat.Comment;
                    item.Status = UpdatedChat.Status;
                    item.Created_Date_Time = UpdatedChat.Created_Date_Time;
                    item.Last_Comment = UpdatedChat.Last_Comment;
                    item.IFCGUID = UpdatedChat.IFCGUID;
                    item.Link_To_Chat = UpdatedChat.Link_To_Chat;
                    item.ChatLocation = UpdatedChat.ChatLocation;
                    item.RevitId = UpdatedChat.RevitId;
                    if (UpdatedChat.Status == "Closed")
                        RevitOperator.DeleteSphereByRevitId(UpdatedChat.RevitId);
                    else
                    {                        
                        RevitOperator.PlaceSphere(UpdatedChat, ChatURL);
                    }
                        break;
                }
            }

            return UpdatedChat;
        }
        public static void PopulateChatPool(List<Chat> PulledChatList)
        {
            ChatPool.Clear();
            var ExistingChatIds = new List<string>();
            ExistingChatIds.Clear();

            foreach (var item in ChatPool)
            {
                ExistingChatIds.Add(item.ChatID);
            }

            foreach (var chatItem in PulledChatList)
            {
                if (!(ExistingChatIds.Contains(chatItem.ChatID)))
                {
                    ChatPool.Add(chatItem);
                }           
            }
        }
        public static DataTable ConvertToDataTable(List<Chat> ChatList)
        {
            DataTable table = new DataTable("Chat");

            // Define columns
            table.Columns.Add("ChatID", typeof(string));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Description", typeof(string));

            // table.Columns.Add("Comment", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Created_Date_Time", typeof(DateTime));
            table.Columns.Add("Last_Comment", typeof(DateTime));
            //table.Columns.Add("IFCGUID", typeof(string)); 
            table.Columns.Add("Link_To_Chat", typeof(string));
            table.Columns.Add("ChatLocation", typeof(string));

            foreach (var chat in ChatList)
            {
                table.Rows.Add(chat.ChatID,chat.Title,chat.Description,chat.Status, chat.Created_Date_Time, chat.Last_Comment, chat.Link_To_Chat,
                    chat.ChatLocation);
            }
            return table;

        }
       
    }
}


public class ReferencePoint
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public ReferencePoint(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}

