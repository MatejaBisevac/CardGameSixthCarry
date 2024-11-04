using Riptide;
using SestiKupi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game
{
    public class MessageService : Singleton<MessageService>
    {
        public static void Notify(string message)
        {
            Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.Message);
            msg.AddString(message);
            NetworkManager.Instance.Server.SendToAll(msg);
        }


    }
}
