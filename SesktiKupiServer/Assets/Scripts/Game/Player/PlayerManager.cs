using Assets.Scripts.Game;
using Riptide;
using SestiKupi.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private GameObject m_PlayerPrefab;
    public static Dictionary<ushort, Player> s_Players = new Dictionary<ushort, Player>();
    private static Dictionary<ushort, GameObject> s_PlayersGameObj = new Dictionary<ushort, GameObject>();
    private static Dictionary<ushort, bool> readyPlayers = new Dictionary<ushort, bool>();
    private static GameObject s_PlayerPrefab;
    public static Player GetPlayer(ushort id)
    {
        s_Players.TryGetValue(id, out Player player);
            return player;
    }

    public static bool RemovePlayer(ushort id)
    {
        bool removedBool = false;
        if(s_PlayersGameObj.TryGetValue(id, out GameObject playerGameObj))
        {
            Destroy(playerGameObj);
            s_PlayersGameObj.Remove(id);
            Debug.Log("s_PlayersGameObj[" + id + "] - removed;");
            removedBool = true;
        }
        if(s_Players.TryGetValue(id, out Player player))
        {
            s_Players.Remove(id);
            Debug.Log("s_Players["+id+"] - removed;");
            removedBool= true;
        }
        if (readyPlayers.TryGetValue(id, out bool readyPlayer))
        {
            readyPlayers.Remove(id);
            Debug.Log("readyPlayers[" + id + "] - removed");
            removedBool = true;
        }
        
        return removedBool;
    }


    protected override void Awake()
    {
        base.Awake();
        s_PlayerPrefab = m_PlayerPrefab;
    }
    public static void SpawnPlayer(ushort id, string username)
    {
        GameObject playerGameObj = Instantiate(s_PlayerPrefab, Vector3.zero, Quaternion.identity);
        s_PlayersGameObj.Add(id, playerGameObj);

        Player player = playerGameObj.GetComponent<Player>();
        player.name = username + "--"+ id;
        player.Init(id,username);
        s_Players.Add(id, player);
        bool shouldApprove = true; // Could be DB validation;
        player.ApproveLogin(shouldApprove);

        readyPlayers.Add(id, false);
    }

    public static void HandlePlayRequest(ushort id)
    {
        if(readyPlayers.ContainsKey(id))
        {
            readyPlayers[id] = true;
        }

        if(CheckIfAllReady())
        {
            foreach(var player in s_Players)
            {
                player.Value.SendCanPlay();
            }

            GameManager.Instance.SetupGame();
        }

    }

    private static bool CheckIfAllReady()
    {
        var readyCount = 0;
        foreach(var readyPly in readyPlayers)
        {
            if (readyPly.Value == true)
                readyCount++;
        }

        if(readyCount == s_Players.Count /*&& readyCount > 1*/)
        {
            return true;
        }

        return false;
    }

    #region Messages
    /*---------------------MESSAGE RECEIVING ------------------------*/
    [MessageHandler((ushort)ClientToServerMsg.RequestLogin)]
    private static void RecieveLoginRequest(ushort fromId, Message msg)
    {
        string username = msg.GetString();
        SpawnPlayer(fromId, username);
    }

    [MessageHandler((ushort)ClientToServerMsg.RequestPlay)]
    private static void RecievePlayRequest(ushort fromId, Message msg)
    {
        string username = msg.GetString();
        Debug.Log("Recieved play request from user : " +  username);
        HandlePlayRequest(fromId);
        //SpawnPlayer(fromId, username);
    }
    #endregion
}
