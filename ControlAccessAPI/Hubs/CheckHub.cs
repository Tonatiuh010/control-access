using Microsoft.AspNetCore.SignalR;
using System;
using Engine.BO;

namespace ControlAccess.Hubs
{
    public class CheckHub : Hub
    {
        public async Task BroadcastCheck(Check check)
            => await Clients.All.SendAsync("CheckMonitor", check);
    }
}
