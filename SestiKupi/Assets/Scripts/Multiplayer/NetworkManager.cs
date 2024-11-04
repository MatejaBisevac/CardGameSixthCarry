using Riptide;
using Riptide.Utils;
using SestiKupi.Core;
using System;
using System.Collections;
using System.Collections.Generic;
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
    EndGame,
    Message
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

    public Client Client;
    [SerializeField] private string m_Ip = "178.149.34.183";
    [SerializeField] private ushort m_Port = 7777;

    public void SetIp(string ip)
    {
        m_Ip = ip;
    }

    private static string s_LocalUsername;
    private void Start()
    {
        Client = new Client();
        Client.Connected += OnClientConnected;
        Client.Disconnected += OnClientDisconnected; 
    }

    private void OnClientConnected(object sender, EventArgs e)
    {
        PlayerManager.Instance.SpawnInitalPlayer(s_LocalUsername);
    }

    private void OnClientDisconnected(object sender, EventArgs e) 
    {
        PlayerManager.Instance.DestroyPlayersOnDisconnect();
    }

    public void Connect(string username)
    {
        s_LocalUsername = string.IsNullOrEmpty(username) ? "Guest" : username;
        //port - usually 7777
        Client.Connect(m_Ip/*+":"+m_Port*/);
    }

    private void FixedUpdate()
    {
        Client.Update();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Client.Connected -= OnClientConnected;
        Client.Disconnected -= OnClientDisconnected;
    }
}
