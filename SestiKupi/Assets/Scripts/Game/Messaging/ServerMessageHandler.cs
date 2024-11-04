using Riptide;
using SestiKupi.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMessageHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TextMeshPro MessagesFromServer;
    public static string LastMessageFromServer;
    public static bool IsZoomedIn = true;
    void Start()
    {
        MessagesFromServer.text = "Server messages...<br>";
    }

    void OnMouseDown()
    {
        ToggleCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(!string.IsNullOrEmpty(LastMessageFromServer))
        {
            MessagesFromServer.text += LastMessageFromServer+"<br>";
            LastMessageFromServer = null;
            MessagesFromServer.pageToDisplay = MessagesFromServer.textInfo.pageCount;
        }
    }

    public void ToggleCameraPosition()
    {
        if(IsZoomedIn)
        {
            CameraAnimator.Instance.ReturnToOriginalPosition();
            IsZoomedIn = false;
        }
        else
        {
            CameraAnimator.Instance.GoToSecondPosition();
            IsZoomedIn = true;
        }
    }


    public void ResetValue()
    {
        MessagesFromServer.text = " Server messages...<br>";
    }



    //----------------MESSAGE RECIEVING---------------------//
    [MessageHandler((ushort)ServerToClientMsg.Message)]
    public static void GetMessageFromServer(Message msg)
    {
        var serverMessage = msg.GetString();
        LastMessageFromServer = serverMessage;
    }
}
