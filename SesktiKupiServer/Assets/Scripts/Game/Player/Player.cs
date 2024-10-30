using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ushort Id { get; private set; }
    public string Username { get; private set; }

    public int Points { get; set; }
    public void Init(ushort id, string username)
    {
        Id = id;
        Username = username;
        Points = 0;
    }

    private void OnDestroy()
    {
        //PlayerManager.RemovePlayer(Id);
        //NetworkManager.Instance.Server.DisconnectClient(Id);
    }

    #region Messages
    /*---------------------MESSAGE SENDING ------------------------*/
    public void ApproveLogin(bool shouldApprove)
    {
        Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.ApproveLogin);
        msg.AddBool(shouldApprove);
        NetworkManager.Instance.Server.Send(msg, Id);
        //NetworkManager.Instance.Server.SendToAll(msg); Salje svima poruke
    }

    public void SendCanPlay()
    {
        Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.CanPlay);
        msg.AddInt(PlayerManager.s_Players.Count);
        foreach(var player in PlayerManager.s_Players)
        {
            msg.AddUShort(player.Value.Id);
            msg.AddString(player.Value.Username);
        }

        NetworkManager.Instance.Server.Send(msg, Id);
    }
    #endregion
}
