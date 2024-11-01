using Assets.Scripts.Multiplayer;
using Riptide;
using Riptide.Utils;
using SestiKupi.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public enum ServerToClientMsg : ushort
{
    ApproveLogin,
    CanPlay,
    InitialCards,
    CanPlayTurn,
    Wait,
    ChooseStack,
    AddCard,
    TakeStack,
    TakenStackByOther,
    EndTurn,
    EndGame
}

public enum ClientToServerMsg : ushort
{
    RequestLogin,
    RequestPlay,
    RequestPlayTurn,
    RequestChooseStack
}
public class NetworkManager : Singleton<NetworkManager>
{
    protected override void Awake()
    {
        base.Awake();
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, true);
    }

    public ServerOverriden Server;
    [SerializeField] private ushort m_Port = 7777;
    [SerializeField] private ushort m_MaxPlayers = 10;
    private void Start()
    {
        Server = new ServerOverriden(); ;
        Server.Start(m_Port, m_MaxPlayers);
       // Server.MessageReceived += OnReceivedMessage;
    }

    //public void OnReceivedMessage(object sender, MessageReceivedEventArgs e)
    //{
    //    Debug.Log("Got message from client");
    //   // return 0;
    //}

    public void OnDisconnectClient(uint connectionID)
    {
        PlayerManager.RemovePlayer((ushort)connectionID);
    }

    private void FixedUpdate()
    {
        Server.Update();
    }
}
