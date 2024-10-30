using Assets.Scripts.Utils;
using Riptide;
using SestiKupi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
//using static UnityEditor.Progress;

namespace Assets.Scripts.Game
{
    public class GameManager : Singleton<GameManager>
    {
        private int currentTurn = 1;
        private bool StartedPlaying = false;
        List<Card> playedCards = new List<Card>();
        private int chosenStack = 0;

        Stack<Card> col1 = new Stack<Card>();
        Stack<Card> col2 = new Stack<Card>();
        Stack<Card> col3 = new Stack<Card>();
        Stack<Card> col4 = new Stack<Card>();
        public void SetupGame()
        {
            var cardCount = PlayerManager.s_Players.Count * 10 + 4;
            List<int> CardNums = RandomNumGenerator.Instance.GenerateUniqueNums(cardCount, 1, 104);
            List<int> CardVals = RandomNumGenerator.Instance.GenerateNums(cardCount, 1, 10);
            List<Card> cardsField = new List<Card>();
            List<Card> cardsHand = new List<Card>();

            //Adding initialy to field
            for (int i = 0; i < 4; i++)
            {
                cardsField.Add(new Card(CardNums[i], CardVals[i]));
            }

            //Adding field cards to columns
            col1.Push(cardsField[0]);
            col2.Push(cardsField[1]);
            col3.Push(cardsField[2]);
            col4.Push(cardsField[3]);

            int playerCounter = 0;
            foreach(var player in PlayerManager.s_Players)
            {
                for(int j = 0; j < 10; j++)
                {
                    cardsHand.Add(new Card(CardNums[playerCounter * 10 + j + 4], CardVals[playerCounter * 10 + j + 4]));
                }

                SendHandStatus(cardsField, cardsHand, player.Value.Id);
                cardsHand.Clear();
                playerCounter++;
            }
           
        }
        private void UpdatePlayedCards()
        {
            playedCards = playedCards.Where<Card>(item =>
            {
                return !item.MarkForDelete;
            }).ToList();
        }
        private void SortCardsByNum()
        {
            playedCards.Sort();
        }

        //0 - pushed to table; 1,2,3,4 - breached stack take all from column; -1 - smallest number take selected row; -2 error
        private int PushCardToTable(Card card)
        {
            var placeOn = 0;
            var maxVal = 0;
            var cardNum = card.Num;
            var col1Num = col1.Peek().Num;
            var col2Num = col2.Peek().Num;
            var col3Num = col3.Peek().Num;
            var col4Num = col4.Peek().Num;

            if (cardNum > col1Num)
            {
                placeOn = 1;
                maxVal = col1Num;
            }
            if (cardNum > col2Num && col2Num > maxVal)
            {
                placeOn = 2;
                maxVal = col2Num;
            }
            if (cardNum > col3Num && col3Num > maxVal)
            {
                placeOn = 3;
                maxVal = col3Num;
            }
            if (cardNum > col4Num && col4Num > maxVal)
            {
                placeOn = 4;
                maxVal = col4Num;
            }

            switch (placeOn)
            {
                case 1:
                    col1.Push(card);
                    card.Column = 1;
                    if (col1.Count == 6)
                        return 1;
                    else
                        return 0;
                case 2:
                    col2.Push(card);
                    card.Column = 2;
                    if (col2.Count == 6)
                        return 2;
                    else
                        return 0;
                case 3:
                    col3.Push(card);
                    card.Column = 3;
                    if (col3.Count == 6)
                        return 3;
                    else
                        return 0;
                case 4:
                    col4.Push(card);
                    card.Column = 4;
                    if (col4.Count == 6)
                        return 4;
                    else
                        return 0;
                case 0:
                    return -1;
                    //LogInvalidMove();
                    //break;
            }
            return -2;
        }

        public void CheckCanPlayTurn()
        {
            int removeCount = 0;
            if(chosenStack>0)
            {
                switch(chosenStack)
                {
                    case 1:
                        {
                            SendTakeStack(playedCards[0]);
                            AddPointsForPlayer(playedCards[0].PlayerId, 1);
                            col1.Clear();
                            col1.Push(playedCards[0]);
                            playedCards.RemoveAt(0);
                            break;
                        }
                    case 2:
                        {
                            SendTakeStack(playedCards[0]);
                            AddPointsForPlayer(playedCards[0].PlayerId, 2);
                            col2.Clear();
                            col2.Push(playedCards[0]);
                            playedCards.RemoveAt(0);
                            break;
                        }
                    case 3:
                        {
                            SendTakeStack(playedCards[0]);
                            AddPointsForPlayer(playedCards[0].PlayerId, 3);
                            col3.Clear();
                            col3.Push(playedCards[0]);
                            playedCards.RemoveAt(0);
                            break;
                        }
                    case 4:
                        {
                            SendTakeStack(playedCards[0]);
                            AddPointsForPlayer(playedCards[0].PlayerId, 4);
                            col4.Clear();
                            col4.Push(playedCards[0]);
                            playedCards.RemoveAt(0);
                            break;
                        }
                    default:
                        {
                            SendWaitAll("");
                            break;
                        }
                }
                chosenStack = 0;
            }
            if (playedCards.Count == PlayerManager.s_Players.Count && StartedPlaying == false)
                StartedPlaying = true;

            if (playedCards.Count == PlayerManager.s_Players.Count || StartedPlaying == true)
            {
                SortCardsByNum();

                foreach (var item in playedCards)
                {
                    switch (PushCardToTable(item))
                    {
                        case 0:
                            {
                                SendPlayCard(item);
                                item.MarkForDelete = true;
                                //playedCards.RemoveAt(0);
                                break;
                            }
                        case 1:
                            {
                                SendTakeStack(item);
                                AddPointsForPlayer(item.PlayerId, 1);
                                col1.Clear();
                                col1.Push(item);
                                item.MarkForDelete = true;
                                //playedCards.RemoveAt(0);
                                break;
                            }
                        case 2:
                            {
                                SendTakeStack(item);
                                AddPointsForPlayer(item.PlayerId, 2);
                                col2.Clear();
                                col2.Push(item);
                                item.MarkForDelete = true;
                                //playedCards.RemoveAt(0);
                                break;
                            }
                        case 3:
                            {
                                SendTakeStack(item);
                                AddPointsForPlayer(item.PlayerId, 3);
                                col3.Clear();
                                col3.Push(item);
                                item.MarkForDelete = true;
                                //playedCards.RemoveAt(0);
                                break;
                            }
                        case 4:
                            {
                                SendTakeStack(item);
                                AddPointsForPlayer(item.PlayerId, 4);
                                col4.Clear();
                                col4.Push(item);
                                item.MarkForDelete = true;
                                //playedCards.RemoveAt(0);
                                break;
                            }
                        case -1:
                            {
                                SendChooseStackToTakeCards(item, item.PlayerId);
                                //playedCards.RemoveAt(0);
                                //item.MarkForDelete = true;
                                return;
                            }
                        case -2:
                            {
                                SendWaitAll("");
                                break;
                            }
                    }
                }
                UpdatePlayedCards();

                //if (!CheckFullStack())
                //{
                //    SendPlayTurnForAll();
                //}
                //else
                //{
                //    SendChooseStackForPlayer();
                //}
                if(StartedPlaying == true && playedCards.Count == 0)
                {
                    StartedPlaying = false;
                    currentTurn++;
                    SendEndTurn();
                }
            }
            else if (StartedPlaying == true && playedCards.Count == 0)
            {
                StartedPlaying = false;
                currentTurn++;
                SendEndTurn();
            }

            if(currentTurn>10)
            {
                SendEndGame();
            }
        }

        private void AddPointsForPlayer(ushort playerId, int column)
        {

            var player = PlayerManager.GetPlayer(playerId);
            player.Points += GetPointsForColumn(column);
        }

        private int GetPointsForColumn(int column)
        {
            int points = 0;
            switch(column)
            {
                case 1:
                    {
                        foreach(var card in col1)
                        {
                            points += card.Val;
                        }
                        break;
                    }
                case 2:
                    {
                        foreach (var card in col2)
                        {
                            points += card.Val;
                        }
                        break;
                    }
                case 3:
                    {
                        foreach (var card in col3)
                        {
                            points += card.Val;
                        }
                        break;
                    }
                case 4:
                    {
                        foreach (var card in col4)
                        {
                            points += card.Val;
                        }
                        break;
                    }
            }
            return points;
        }

        public void SendChooseStackToTakeCards(Card card, ushort playerId)
        {
            SendChooseStackForPlayer(card, playerId);

            //todo__implement
        }

        //----------------------MESSAGE SENDING-----------------------------//
        public static void SendEndTurn()
        {
            Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.EndTurn);
            msg.AddString("END TURN NIGGGGAAA ");
            NetworkManager.Instance.Server.SendToAll(msg);
        }
        public static void SendHandStatus(List<Card> cardsField, List<Card> cardsHand, ushort playerId)
        {
            Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.InitialCards);

            for(int i = 0;  i < 4; i++)
            {
                msg.AddInt(cardsField[i].Num);
                msg.AddInt(cardsField[i].Val);
            }
            
            for(int i = 0; i < 10; i++)
            {
                msg.AddInt(cardsHand[i].Num);
                msg.AddInt(cardsHand[i].Val);
            }

            NetworkManager.Instance.Server.Send(msg, playerId);
        }

        public static void SendPlayCard(Card card)
        {
            Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.AddCard);

            msg.AddInt(card.Num);
            msg.AddInt(card.Val);
            msg.AddUShort(card.PlayerId);
            msg.AddInt(card.Column);

            NetworkManager.Instance.Server.SendToAll(msg);
        }

        public static void SendTakeStack(Card card)
        {
            Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.TakeStack);

            msg.AddInt(card.Num);
            msg.AddInt(card.Val);
            msg.AddUShort(card.PlayerId);
            msg.AddInt(card.Column);

            NetworkManager.Instance.Server.Send(msg, card.PlayerId);

            msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.TakenStackByOther);
            msg.AddInt(card.Num);
            msg.AddInt(card.Val);
            msg.AddUShort(card.PlayerId);
            msg.AddInt(card.Column);
            
            foreach(var player in PlayerManager.s_Players)
            {
                if(player.Value.Id!= card.PlayerId)
                {
                    NetworkManager.Instance.Server.Send(msg, player.Value.Id);
                }
            }
        }

        public static void SendChooseStackForPlayer(Card card, ushort playerId)
        {
            Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.ChooseStack);
            msg.AddString("NIGGGAAA CHOOSEEE(Stack)");
            msg.AddInt(card.Num);
            msg.AddInt(card.Val);
            NetworkManager.Instance.Server.Send(msg, playerId);
            //var chosenStack = await ReceiveChosenStack;
        }

        public static void SendEndGame()
        {
            string pointsList = " points : ";
            foreach(var player in PlayerManager.s_Players)
            {
                pointsList += player.Value.Username + " : " + player.Value.Points + " ;";
            }

            Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.EndGame);
            msg.AddString(pointsList);
            NetworkManager.Instance.Server.SendToAll(msg);
        }

        public static void SendWait(ushort playerId, string message)
        {
            Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.Wait);
            msg.AddString("NIGGGAAA WAITTT(server message: "+message+")");
            NetworkManager.Instance.Server.Send(msg, playerId);
        }

        public static void SendWaitAll(string message)
        {
            Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.Wait);
            message = string.IsNullOrEmpty(message) ? "NIGGAAAA AAAAAA I DIEEEED" : message;
            msg.AddString(message);
            NetworkManager.Instance.Server.SendToAll(msg);
        }


//---------------------MESSAGE RECEIVING---------------------------------//
        [MessageHandler((ushort)ClientToServerMsg.RequestPlayTurn)]
        public static void ReceivePlayCardRequest(ushort fromId, Message msg)
        {
            var num = msg.GetInt();
            var val = msg.GetInt();

            var cardPlayed = new Card(num, val, fromId);
            Instance.playedCards.Add(cardPlayed);
            Instance.CheckCanPlayTurn();
            Instance.UpdatePlayedCards();
        }

        [MessageHandler((ushort)ClientToServerMsg.RequestChooseStack)]
        public static void ReceiveChosenStack(ushort fromId, Message msg)
        {
            Instance.chosenStack = msg.GetInt();
            Instance.CheckCanPlayTurn();
            Instance.UpdatePlayedCards();
        }

    }
}
