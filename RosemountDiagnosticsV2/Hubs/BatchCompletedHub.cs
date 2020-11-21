using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BatchDataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.SignalR;
using RosemountDiagnosticsV2.View_Models;

namespace RosemountDiagnosticsV2.Hubs
{
    public class BatchCompletedHub : Hub
    {
        
        public Task LastBatch(string message)
        {
            return Clients.All.SendAsync("LastBatch", message);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", message);
        }

        public Task BatchesMadeByCategory(string count)
        {
            return Clients.All.SendAsync("BatchesMadeByCategory", count);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        
    }
}
