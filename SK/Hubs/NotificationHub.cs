using Microsoft.AspNetCore.SignalR;

namespace SK.Hubs
{
    public class NotificationHub : Hub
    {
        /*Use Single-user groups.You can create a group for each user, 
        * and then send a message to that group when you want to reach only that user. 
        * The name of each group is the name of the user. 
        * If a user has more than one connection, each connection id is added to the user's group.
        * For example, base on the getting start document, I have create a SignalR application, 
        * it will send message to all users. Then, in the ChatHub class, 
        * add the Authorize attribute and override the OnConnectedAsync() method, 
        * in the OnConnectedAsync method, we could create a group based on the Identity User. 
        * Then, add a SendMessageToGroup method to send message to group.*/

       
        public override Task OnConnectedAsync()
        {
            try
            {
                if (Context.User.Identity.Name != null)
                    Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("NotificationHub: "+ex);
            }
           
            return base.OnConnectedAsync();
        }

        /*
       public async Task SendMessage(string message)
       {
           var CurrentLoggedUnit = Context.User.FindFirst(x => x.Type == "Unit").Value;
           var CurrentLoggedLastName = Context.User.FindFirst(x => x.Type == "LastName").Value;
           var CurrentLoggedUserId = Context.User.FindFirst(x => x.Type == "LoggedId").Value;
           var CurrentLoggedName = Context.User.Identity.Name;
           await Clients.Group(CurrentLoggedUnit).SendAsync("ReceiveMessage", CurrentLoggedUserId, CurrentLoggedName + " " + CurrentLoggedLastName, message);

       }
       */

        public async Task SendMessage(string message)
        {
            try
            {
                var CurrentLoggeId = Context.User.Identity.Name;
                await Clients.Group(CurrentLoggeId).SendAsync("ReceiveMessage", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("NotificationHub: " + ex);
            }
           
        }


    }
}
