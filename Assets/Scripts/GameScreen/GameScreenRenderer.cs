using System;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------
//-------------------------------------------------------
// GameScreenRenderer
//-------------------------------------------------------
//-------------------------------------------------------
public class GameScreenRenderer : ScreenRenderer<GameScreen>
{
    //----------------------------------------------
    // Variables
    private List<CardComponent> m_handCards;
    private List<GameObject> m_minions;

    //----------------------------------------------
    // Properties

    //----------------------------------------------
    // Methods
    //-------------------------------------------------------
    public GameScreenRenderer()
    {
        m_handCards = new List<CardComponent>();
        m_minions = new List<GameObject>();
    }

    protected override void OnInit()
    {
        EventManager.Subscribe<Turn.StateEvent>(this.OnTurnStateChanged, EventChannel.Post);
        EventManager.Subscribe<Card.Played>(this.OnCardPlayed, EventChannel.Post);
    }

    protected override void OnShutdown()
    {
        EventManager.UnSubscribe<Turn.StateEvent>(this.OnTurnStateChanged, EventChannel.Post);
        EventManager.UnSubscribe<Card.Played>(this.OnCardPlayed, EventChannel.Post);
    }

    protected override void OnStart()
    {
 
    }

    protected override void OnStop()
    {
 
    }

    //----------------------------------------------
    protected override void OnUpdate()
    {

    }

    protected override void OnUpdateGUI()
    {
        if(!Screen.HasEnded)
        {
            // UI display
            HumanPlayer human = Screen.CurrentTurn.Player as HumanPlayer;
            if (human != null)
            {
                if (GUI.Button(new Rect(UnityEngine.Screen.width - 120, UnityEngine.Screen.height - 60, 100, 30), "End turn"))
                {
                    EventManager.SendEmptyPooledEvent<EndTurnButtonClicked>();
                }

                /*GUI.Label(new Rect(20, UnityEngine.Screen.height - 160, 100, 30), "Energy : " + human.Energy);
                GUI.Label(new Rect(20, UnityEngine.Screen.height - 120, 100, 30), "DrawPile : " + human.DrawPile.Size);
                GUI.Label(new Rect(UnityEngine.Screen.width - 120, UnityEngine.Screen.height - 120, 100, 30), "Discard : " + human.DiscardPile.Size);*/
            }
            else
            {
                if (GUI.Button(new Rect(UnityEngine.Screen.width - 120, UnityEngine.Screen.height - 60, 100, 30), "End AI turn"))
                {
                    EventManager.SendEmptyPooledEvent<EndTurnButtonClicked>();
                }
            }

            // MinionDisplay

            foreach (Player combattant in Screen.Players)
            {
                int y = 50;
                int x = UnityEngine.Screen.width - 60;
                if (combattant is HumanPlayer)
                {
                    x = 30;
                }
            }
        }
        else
        {
            if(Screen.Succeded)
            {
                GUI.TextField(new Rect(20, UnityEngine.Screen.height - 160, 100, 30), "You win");
            }
            else
            {
                GUI.TextField(new Rect(20, UnityEngine.Screen.height - 160, 100, 30), "You Fail");
            }
            
        }
    }

    private void OnTurnStateChanged(Turn.StateEvent evt)
    {
        Turn turn = evt.Turn;
        if (turn != null)
        {
            HumanPlayer player = turn.Player as HumanPlayer;
            if (player != null)
            {
                if (evt.Turn.IsStarted)
                {
                    SpawnHand(player.Hand);
                }
                else if (evt.Turn.IsStopped)
                {
                    UnSpawnHand();
                }
            }
        }
    }

    private Vector3 spawnRef = new Vector3();
    protected void SpawnHand(Deck hand)
    {
        int increment = 2;
        spawnRef.x = -hand.Size / 2 * increment;
        spawnRef.y = -3;
        foreach (Card card in hand)
        {
            CardComponent newCard = null; //card.Spawn();
            if (newCard)
            {
                m_handCards.Add(newCard);
                newCard.SetPosInHand(spawnRef);

                spawnRef.x += increment;
            }
        }
    }

    protected void UnSpawnHand()
    {
        foreach (CardComponent cardObj in m_handCards)
        {
            UnityEngine.Object.Destroy(cardObj.gameObject);
        }
        m_handCards.Clear();
    }

    protected void UnSpawnCard(CardComponent cardObj)
    {
        m_handCards.Remove(cardObj);
        UnityEngine.Object.Destroy(cardObj.gameObject);
    }

    protected CardComponent GetHandCard(Card card)
    {
        foreach (CardComponent cardObj in m_handCards)
        {
            if(cardObj.Card == card)
            {
                return cardObj;
            }
        }
        return null;
    }

    protected void OnCardPlayed(Card.Played evt)
    {
        CardComponent cardObj = GetHandCard(evt.Card);
        if(cardObj)
        {
            UnSpawnCard(cardObj);
        }
    }
}

//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
// EndTurnButtonClicked
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
public class EndTurnButtonClicked : PooledEvent
{
    public override void Reset()
    {

    }
}