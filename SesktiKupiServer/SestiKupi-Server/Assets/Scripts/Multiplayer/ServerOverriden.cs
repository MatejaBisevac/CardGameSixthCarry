using Riptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Multiplayer
{
    public class ServerOverriden : Server
    {
        protected override void OnClientDisconnected(Connection connection, DisconnectReason reason)
        {
            base.OnClientDisconnected(connection, reason);
            NetworkManager.Instance.OnDisconnectClient(connection.Id);
        }
    }
}
