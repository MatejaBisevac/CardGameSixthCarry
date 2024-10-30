using DG.Tweening;
using Riptide;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int Num { get; private set; }
    public int Val { get; private set; }

    public ushort PlayerCarrier { get; private set; }

    public bool InHand { get; set; }

    public int Column { get; set; }

    public GameObject thisGameObject;

    public Material playedCardMaterial;

    public void Awake()
    {
        PlayerCarrier = 0;
        InHand = false;
        Column = 0;
        initialPosition = transform.localScale;
    }
    public void Init(int num, int val)
    {
        PlayerCarrier = PlayerManager.LocalPlayer.Id;
        Num = num;
        Val = val;
        SetNumVal(Num, Val);
    }

    public void InitCardFromServer(int num, int val, ushort playerId, int col)
    {
        PlayerCarrier = playerId;
        Num = num;
        Val = val;
        Column = col;
        SetNumVal(Num, Val);
    }

    private void OnDestroy()
    {
        //PlayerManager.RemovePlayer(Id);
    }

    private void SetNumVal(int val, int num)
    {
        var Num = transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text;
        var Val = transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text;

        transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = val + "";
        transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = num + "";
        //
    }

    public void TryPlayCard()
    {
        Debug.Log("Trying to play selected card");

        if(!GameManager.Instance.LocalPlayerRequestedPlayTurn)
        {
            GameManager.Instance.LocalPlayerRequestedPlayTurn = true;
            GameManager.Instance.LocalPlayerWaiting = true;
            RequestPlayTurn();
            GameManager.Instance.SetMessageToPlayer("Waiting on other players...");
            this.transform.SetParent(null);
            Destroy(thisGameObject);
            //transform.GetComponent<MeshRenderer>().material = playedCardMaterial;
        }
        //Server should handle adding the cards to column
        //GameManager.Instance.AddToColumn(thisGameObject);
    }

    public void TryChooseStack()
    {
        Debug.Log("Trying to choose selected stack");
        if(GameManager.Instance.LocalPlayerChoosingStack)
        {
            var chosenCardToReplaceStack = GameManager.Instance.GetCardToReplaceStack();
            if(chosenCardToReplaceStack == null)
            {
                GameManager.Instance.ShowErrorMessage("SYNC ISSUE ABORT GAME NIGGA");
            }
            else
            {
                GameManager.Instance.ReplaceStack(this.Column);
                GameManager.Instance.SendReplacedColumnToServer(this.Column);
                GameManager.Instance.GetPointForColumn(this.Column);
                this.transform.SetParent(null);
                Destroy(thisGameObject);
                GameManager.Instance.SetMessageToPlayer("Waiting on other players...");
            }
        }

    }

    #region MouseHandlers
    private Vector3 initialPosition;
    private bool lifted = false;
    public void OnMouseDown()
    {
        if(InHand)
        {
            TryPlayCard();
        }
        else
        {
            TryChooseStack();
        }
    }
    public void OnMouseEnter()
    {
        var x = initialPosition.x;
        var y = initialPosition.y;
        var z = initialPosition.z;
        var newVector = new Vector3(x, y, z-0.5f);
        //transform.DOMove(initialPosition, 0.15f);
        if(!lifted)
        {
            transform.position += new Vector3(0f, 0.3f, 0f);
            //transform.DOScale(1.2f,0.15f);
            lifted = !lifted;
        }
    }

    public void OnMouseExit()
    {
        if(lifted)
        {
            transform.position -= new Vector3(0f, 0.3f, 0f);
            //transform.DOScale(1f, 0.15f);
            lifted = !lifted;
        }
        //transform.localPosition = initialPosition;
       // transform.DOMove(initialPosition, 0.15f);
    }
    #endregion



    #region Messages
    /*---------------------MESSAGE SENDING ------------------------*/
    //public void RequestInit()
    //{
    //    Message msg = Message.Create(MessageSendMode.Reliable, ClientToServerMsg.RequestLogin);
    //    msg.AddString(Username);
    //    NetworkManager.Instance.Client.Send(msg);
    //}

    public void RequestPlayTurn()
    {
        Message msg = Message.Create(MessageSendMode.Reliable, ClientToServerMsg.RequestPlayTurn);
        msg.AddInt(Num);
        msg.AddInt(Val);
        NetworkManager.Instance.Client.Send(msg);
    }

    #endregion
}
