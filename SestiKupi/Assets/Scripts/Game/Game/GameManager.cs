using Assets.Scripts.Game.UI;
using Riptide;
using SestiKupi.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // Start is called before the first frame update
    public bool CanStartGame { get; set; }
    public GameObject CardPrefab;
    public GameObject FirstColumn;
    public GameObject SecondColumn;
    public GameObject ThirdColumn;
    public GameObject FourthColumn;
    public GameObject Hand;
    [Header("Waiting interface")]
    public GameObject WaitingBox;
    public GameObject InfoText;

    [Header("Player Points")]
    public int LocalPlayerPoints;

    private GameObject RecievedCardForNewStack = null;

    Stack<GameObject> column1 = new Stack<GameObject>();
    Stack<GameObject> column2 = new Stack<GameObject>();
    Stack<GameObject> column3 = new Stack<GameObject>();
    Stack<GameObject> column4 = new Stack<GameObject>();

    [Header("Flags for Gameplay")]
    public bool LocalPlayerRequestedPlayTurn;
    public bool LocalPlayerWaiting;
    public bool LocalPlayerChoosingStack; 

    private List<GameObject> generatedCards;
    protected override void Awake()
    {
        base.Awake();
        generatedCards = new List<GameObject>();
    }

    void ResetGame()
    {
        foreach(Transform card in FirstColumn.transform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in SecondColumn.transform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in ThirdColumn.transform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in FourthColumn.transform)
        {
            Destroy(card.gameObject);
        }
        LocalPlayerRequestedPlayTurn = false;
        LocalPlayerWaiting = true;
        LocalPlayerChoosingStack = false;
        UIManagerGame.Instance.readyForGameOverlay.SetActive(true);

    }

    void Start()
    {
        CanStartGame = false;
        LocalPlayerRequestedPlayTurn = false;
        LocalPlayerChoosingStack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(LocalPlayerWaiting)
        {

            WaitingBox.SetActive(true);
            //show waiting box;
        }
        else
        {
            WaitingBox.SetActive(false);
            //hide waiting box;
        }
    }
    
    public void ClearMessages()
    {
        LocalPlayerWaiting = false;
        UIManagerGame.Instance.readyForGameOverlay.SetActive(false);
    }

    //This should be moved to network manager to handle the cards generaton and which cards have players;
    //Cards on the field should be common for all players, Cards in hand should be distinct
    public void SetupGame(List<int> CardNums, List<int> CardVals)
    {
        //PlayerManager.Instance.PrintPlayers();
        var cardCount = 10 + 4;
        //List<int> CardNums = RandomNumGenerator.Instance.GenerateUniqueNums(cardCount, 1,104);
        //List<int> CardVals = RandomNumGenerator.Instance.GenerateNums(cardCount, 1, 10);
        generatedCards = new List<GameObject>();

        for (int i = 0; i < cardCount; i++)
        {
            var newCard = InstantiateCard();
            newCard.GetComponent<Card>().Init(CardNums[0], CardVals[0]);
            CardNums.RemoveAt(0); 
            CardVals.RemoveAt(0);

            generatedCards.Add(newCard);
        }
        
        //Adding initialy to field
        for(int i = 0; i < 4; i++)
        {
            generatedCards[i].GetComponent<Card>().Column = i + 1;
            AddToColumn(i+1, generatedCards[i]);
        }

        for(int i = 4; i < cardCount; i++)
        {
            AddToHand(generatedCards[i]);
        }
    }
    public void AddToHand(GameObject card)
    {
        card.transform.GetComponent<Card>().InHand = true;
        //card.transform.Translate(new Vector3(0f, 1.2f, -0.2f));
        card.transform.SetParent(Hand.transform);
        card.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(card.transform.position.x, card.transform.position.y, 0f);
        card.transform.Rotate(0f, 0f, 5f);
    }

    public GameObject GetCardToReplaceStack()
    {
        return RecievedCardForNewStack;
    }

    public GameObject UndoAddToHandTransform(GameObject card)
    {
        var newCard = InstantiateCard();
        newCard.GetComponent<Card>().Init(card.transform.GetComponent<Card>().Num, card.transform.GetComponent<Card>().Val);
        Destroy(card);
        return newCard;
        //card.transform.Rotate(-90f, 0f, -10f);
        //card.transform.Translate(new Vector3(0f, -1.2f, 0.2f));
    }

    public void AddToColumn(int column, GameObject card)
    {
        switch(column)
        {
            case 1:
               // card.transform.Rotate(90f, 0f, 0f);
                card.transform.SetParent(FirstColumn.transform);
                card.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(card.transform.position.x, card.transform.position.y, 0f);
                column1.Push(card);
                break;
            case 2:
                //card.transform.Rotate(90f, 0f, 0f);
                card.transform.SetParent(SecondColumn.transform);
                card.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(card.transform.position.x, card.transform.position.y, 0f);
                column2.Push(card);
                break;
            case 3:
                //card.transform.Rotate(90f, 0f, 0f);
                card.transform.SetParent(ThirdColumn.transform);
                card.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(card.transform.position.x, card.transform.position.y, 0f);
                column3.Push(card);
                break;
            case 4:
                //card.transform.Rotate(90f, 0f, 0f);
                card.transform.SetParent(FourthColumn.transform);
                card.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(card.transform.position.x, card.transform.position.y, 0f);
                column4.Push(card);
                break;
        }
    }
    public void DestroyChildrenInColumn(int column)
    {
        switch(column)
        {
            case 1:
                {
                    foreach(Transform child in FirstColumn.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    break;
                }
            case 2:
                {
                    foreach (Transform child in SecondColumn.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    break;
                }
            case 3:
                {
                    foreach (Transform child in ThirdColumn.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    break;
                }
            case 4:
                {
                    foreach (Transform child in FourthColumn.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    break;
                }
        }
    }
    public void ReplaceStack(int column)
    {
        //RecievedCardForNewStack
        switch(column)
        {
            case 1:
                //LocalPlayerPoints += GetPointForColumn(1);
                column1.Clear();
                DestroyChildrenInColumn(1);
                column1.Push(RecievedCardForNewStack);
                AddToColumn(1, RecievedCardForNewStack);
                RecievedCardForNewStack = null;
                break;
            case 2:
                //LocalPlayerPoints += GetPointForColumn(2);
                column2.Clear();
                DestroyChildrenInColumn(2);
                column2.Push(RecievedCardForNewStack);
                AddToColumn(2, RecievedCardForNewStack);
                RecievedCardForNewStack = null;
                break;
            case 3:
                //LocalPlayerPoints += GetPointForColumn(3);
                column3.Clear();
                DestroyChildrenInColumn(3);
                column3.Push(RecievedCardForNewStack);
                AddToColumn(3, RecievedCardForNewStack);
                RecievedCardForNewStack = null;
                break;
            case 4:
                //LocalPlayerPoints += GetPointForColumn(4);
                column4.Clear();
                DestroyChildrenInColumn(4);
                column4.Push(RecievedCardForNewStack);
                AddToColumn(4, RecievedCardForNewStack);
                RecievedCardForNewStack = null;
                break;
        }

        //SendReplacedColumnToServer(column);
    }


    public void GetPointForColumn(int column)
    {
        int points = 0;
        switch (column)
        {
            case 1:
                foreach (var item in column1)
                {
                    points += item.GetComponent<Card>().Val;
                }
                break;
            case 2:
                foreach (var item in column2)
                {
                    points += item.GetComponent<Card>().Val;
                }
                break;
            case 3:
                foreach (var item in column3)
                {
                    points += item.GetComponent<Card>().Val;
                }
                break;
            case 4:
                foreach (var item in column4)
                {
                    points += item.GetComponent<Card>().Val;
                }
                break;
        }

        LocalPlayerPoints += points;
        Debug.Log(LocalPlayerPoints);
        //return points;
    }


    //TODO -- Delete this method
    public void AddToColumn(GameObject card)
    {
        var placeOn = 0;
        var maxVal = 0;
        var cardNum = card.transform.GetComponent<Card>().Num;
        var col1Num = column1.Peek().transform.GetComponent<Card>().Num;
        var col2Num = column2.Peek().transform.GetComponent<Card>().Num;
        var col3Num = column3.Peek().transform.GetComponent<Card>().Num;
        var col4Num = column4.Peek().transform.GetComponent<Card>().Num;

        if(cardNum > col1Num)
        {
            placeOn = 1;
            maxVal = col1Num;
        }
        if(cardNum > col2Num && col2Num > maxVal)
        {
            placeOn = 2;
            maxVal = col2Num;
        }
        if(cardNum > col3Num && col3Num > maxVal)
        {
            placeOn = 3;
            maxVal = col3Num;
        }
        if(cardNum > col4Num && col4Num > maxVal)
        {
            placeOn = 4;
            maxVal = col4Num;
        }

        switch(placeOn)
        {
            case 1:
                var newCard1 = UndoAddToHandTransform(card);
                AddToColumn(1, newCard1);
                break;
            case 2:
                var newCard2 = UndoAddToHandTransform(card);
                AddToColumn(2, newCard2);
                break;
            case 3:
                var newCard3 = UndoAddToHandTransform(card);
                AddToColumn(3, newCard3);
                break;
            case 4:
                var newCard4 = UndoAddToHandTransform(card);
                AddToColumn(4, newCard4);
                break;
            case 0:
                LogInvalidMove();
                break;
        }
    }

    public void LogInvalidMove()
    {
        //TODO - Handle invalid move
        Debug.LogError("INVALID MOVE");
    }

    public void SetMessageToPlayer(string newMsg)
    {
        InfoText.transform.GetComponent<Text>().text = newMsg;
    }

    public void ShowErrorMessage(string errorMessage)
    {
        SetMessageToPlayer(errorMessage);
        LocalPlayerWaiting = true;
    }

    private GameObject InstantiateCard()
    {
        var newCard = Instantiate(CardPrefab);
        newCard.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(newCard.transform.position.x, newCard.transform.position.y, 0f);

        return newCard;
    }

    //-----------MESSAGE RECEIVING-------------------

    [MessageHandler((ushort)ServerToClientMsg.InitialCards)]
    private static void ReceiveIntialCards(Message msg)
    {
        Instance.ClearMessages();
        List<int> CardNums = new List<int>();
        List<int> CardVals = new List<int>();

        for(int i = 0; i < 4; i++)
        {
            var num = msg.GetInt();
            var val = msg.GetInt();

            CardNums.Add(num);
            CardVals.Add(val);
        }
        for(int i = 0; i< 10; i++)
        {
            var num = msg.GetInt();
            var val = msg.GetInt();

            CardNums.Add(num);
            CardVals.Add(val);
        }

        Instance.SetupGame(CardNums, CardVals);
    }

    [MessageHandler((ushort)ServerToClientMsg.AddCard)]
    private static void ReceiveCard(Message msg)
    {
        int newCardNum = msg.GetInt();
        int newCardVal = msg.GetInt();
        ushort fromPlayerId = msg.GetUShort();
        int newCardCol = msg.GetInt();

        var newCard = Instantiate(Instance.CardPrefab);
        newCard.GetComponent<Card>().InitCardFromServer(newCardNum, newCardVal, fromPlayerId, newCardCol);

        Instance.AddToColumn(newCardCol, newCard);
    }

    [MessageHandler((ushort)ServerToClientMsg.Wait)]
    private static void ReceiveWait(Message msg)
    {
        Instance.SetMessageToPlayer(msg.GetString());
        //InfoText.transform.GetComponent<TextMeshPro>().text = msg.GetString();
    }

    [MessageHandler((ushort)ServerToClientMsg.ChooseStack)]
    private static void ReceiveChooseStack(Message msg)
    {
        Instance.SetMessageToPlayer(msg.GetString());
        int newStackCardNum = msg.GetInt();
        int newStackCardVal = msg.GetInt();

        var newCard = Instantiate(Instance.CardPrefab);
        newCard.GetComponent<Card>().Init(newStackCardNum, newStackCardVal);

        GameManager.Instance.LocalPlayerChoosingStack = true;

        Instance.RecievedCardForNewStack = newCard;
    }


    [MessageHandler((ushort)ServerToClientMsg.TakeStack)]
    private static void TakeStack(Message msg)
    {
        var Num = msg.GetInt();
        var Val = msg.GetInt();
        var PlayerId = msg.GetUShort();
        var Column = msg.GetInt();

        var newCard = instance.InstantiateCard();
        newCard.GetComponent<Card>().InitCardFromServer(Num, Val,PlayerId,Column);
        Instance.RecievedCardForNewStack = newCard;
        Instance.ReplaceStack(Column);
        Instance.GetPointForColumn(Column);
    }
    [MessageHandler((ushort)ServerToClientMsg.TakenStackByOther)]
    private static void TakenStackByOther(Message msg)
    {
        var Num = msg.GetInt();
        var Val = msg.GetInt();
        var PlayerId = msg.GetUShort();
        var Column = msg.GetInt();

        var newCard = Instance.InstantiateCard();
        newCard.GetComponent<Card>().InitCardFromServer(Num, Val, PlayerId, Column);
        Instance.RecievedCardForNewStack = newCard;
        Instance.ReplaceStack(Column);

    }

    [MessageHandler((ushort)ServerToClientMsg.EndTurn)]
    private static void EndTurn(Message msg)
    {
        var recvMsg = msg.GetString();
        Instance.LocalPlayerRequestedPlayTurn = false;
        Instance.LocalPlayerWaiting = false;
        Instance.LocalPlayerChoosingStack = false;
    }

    [MessageHandler((ushort)ServerToClientMsg.EndGame)]

    private static void EndGame(Message msg)
    {
        var recvMsg = msg.GetString();
        Instance.LocalPlayerWaiting = true;
        Instance.SetMessageToPlayer(recvMsg);
        Instance.ResetGame();
    }


    //------------MESSAGE SENDING-------------------------//

    public void SendReplacedColumnToServer(int column)
    {
        Message msg = Message.Create(MessageSendMode.Reliable, ClientToServerMsg.RequestChooseStack);
        msg.AddInt(column);
        NetworkManager.Instance.Client.Send(msg);
        LocalPlayerChoosingStack = false;
    }

}
