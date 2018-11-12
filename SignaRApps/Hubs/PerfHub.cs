using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using SignaRApps.Counters;
using SignaRApps.Models;

namespace SignaRApps.Hubs
{
    public class PerfHub : Hub
    {
        static List<UserInfo> connectedusers = new List<UserInfo>();
        DbModel db = new DbModel();
        public void Connect(string name)
        {
            var now = DateTime.Now.ToString();
            UserInfo user = new UserInfo { ConnectionTime = now, ConnectionID = Context.ConnectionId, Name = name };
            connectedusers.Add(user);
            db.UserInfo.Add(user);
            db.SaveChanges();
            Clients.Others.ReceivedMessage("New User Connected as  ", name, now);
            Clients.Caller.ReceivedMessage("You are Connected as  ", name, now);
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var user = connectedusers.SingleOrDefault(s => s.ConnectionID == Context.ConnectionId);
            if (user != null)
            {
                connectedusers.Remove(user);
                Clients.All.ReceivedMessage("Server : ", user.Name + "Logged Out");
            }
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// knockout part starts here
        /// </summary>
        public PerfHub()
        {
            StartCounterCollection();
        }

        private void StartCounterCollection()
        {
            var task = Task.Factory.StartNew(async () =>
            {
                var perfService = new PerfCounterService();
                while (true)
                {
                    var results = perfService.GetResults();
                    Clients.All.newCounters(results);
                    await Task.Delay(2000);
                }

            }, TaskCreationOptions.LongRunning);
        }
        public void Send(string message)
        {
            //
            //Clients.All.newMessage(
            //    Context.User.Identity.Name + " says: " + message
            //    );
            //////
            //////
            var now = DateTime.Now.ToString();
            var uid = db.UserInfo.Where(u => u.ConnectionID == Context.ConnectionId).SingleOrDefault().UserInfoID;
            var uname = connectedusers.SingleOrDefault(s => s.ConnectionID == Context.ConnectionId).Name;
            MessageInfo msg = new MessageInfo { MessageText = message, Time = now, UserInfoID = uid };
            db.MessageInfo.Add(msg);
            db.SaveChanges();
            Clients.Others.ReceivedMessage(uname, message, now);
            Clients.Caller.ReceivedMessage("Me  ", message, now);
        }

        public void SendB(string mes)
        {
            Clients.All.newMessage(
                Context.User.Identity.Name + " says: " + mes
                );
        }
    }
}