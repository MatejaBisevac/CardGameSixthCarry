using Assets.Scripts.Game.UI;
using Riptide;
using SestiKupi.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private GameObject m_PlayerPrefab;
    private static Dictionary<ushort, Player> s_Players = new Dictionary<ushort, Player>();
    private static Dictionary<ushort, GameObject> s_PlayersGameObj = new Dictionary<ushort, GameObject>();
    private static Dictionary<ushort, string> other_players = new Dictionary<ushort, string>();
    
    private static GameObject s_PlayerPrefab;
    public static Player GetPlayer(ushort id)
    {
        s_Players.TryGetValue(id, out Player player);
            return player;
    }

    public void PrintPlayers()
    {
        foreach(var player in s_Players)
        {
            Debug.Log("player: --- " + player.Value.Username);
        }
    }

    public static bool RemovePlayer(ushort id)
    {
        if(s_Players.TryGetValue(id, out Player player))
        {
            s_Players.Remove(id);
            return true;
        }
        return false;
    }

    public static Player LocalPlayer => GetPlayer(NetworkManager.Instance.Client.Id);
    public static bool IsLocalPlayer(ushort id) => id == LocalPlayer.Id;

    protected override void Awake()
    {
        base.Awake();
        s_PlayerPrefab = m_PlayerPrefab;
    }

    public void DestroyPlayersOnDisconnect()
    {
        Application.Quit();
        //foreach(var playerGameObj in  s_PlayersGameObj)
        //{
        //    Destroy(playerGameObj.Value);
        //}

        //s_PlayersGameObj.Clear();
        //s_Players.Clear();

        //ReturnToLoginPage();
    }
    public void LocalPlayerReadyToPlay()
    {
        LocalPlayer.RequestReadyToPlay();
    }

    public void SpawnInitalPlayer(string username)
    {
        GameObject playerGameObj = Instantiate(s_PlayerPrefab, Vector3.zero, Quaternion.identity);
        Player player = playerGameObj.GetComponent<Player>();
        player.name = username + " -- LOCAL PLAYER (WAITING FOR SERVER)";
        ushort id = NetworkManager.Instance.Client.Id;
        player.Init(id, username, true);
        playerGameObj.transform.SetParent(this.transform);

        s_Players.Add(id, player);
        s_PlayersGameObj.Add(id, playerGameObj);

        player.RequestInit();
    }

    public void SpawnPlayer(ushort id, string username)
    {
        GameObject playerGameObj = Instantiate(s_PlayerPrefab, Vector3.zero, Quaternion.identity);
        Player player = playerGameObj.GetComponent<Player>();
        player.name = username;
        player.Init(id, username, false);
        playerGameObj.transform.SetParent(this.transform);

        s_Players.Add(id, player);
        s_PlayersGameObj.Add(id, playerGameObj);
    }

    private static void InitializeLocalPlayer()
    {
        LocalPlayer.name = LocalPlayer.Username + "--" + LocalPlayer.Id + "--LOCAL"; 
    }

    #region Messages

    /*---------------------MESSAGE RECEIVING ------------------------*/
    [MessageHandler((ushort)ServerToClientMsg.ApproveLogin)]
    private static void ReceiveApproveLogin(Message msg)
    {
        bool approve = msg.GetBool();
        if(approve)
        {
            SceneManager.LoadScene("01-Level");
            InitializeLocalPlayer();
        }
    }

    [MessageHandler((ushort)ServerToClientMsg.CanPlay)]
    private static void ReceiveCanPlay(Message msg)
    {
       var playerCount = msg.GetInt();
        for (int i = 0; i < playerCount; i++)
        {
           var playerId = msg.GetUShort();
           var playerUsername =  msg.GetString();

           if(playerId != LocalPlayer.Id)
           {
                Instance.SpawnPlayer(playerId, playerUsername);
           }
        }

        UIManagerGame.Instance.ReceiveCanPlay();
        //GameManager.Instance.ReceiveCanPlay();
    }

    #endregion
}
