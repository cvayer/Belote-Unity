using System;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------
//-------------------------------------------------------
// GameScreenRenderer
//-------------------------------------------------------
//-------------------------------------------------------
public class GameScreenRenderer : ScreenRenderer
{
    //----------------------------------------------
    // Variables
    private List<CardComponent> m_cards;

    //----------------------------------------------
    // Properties

    public new GameScreen Screen
    {
        get 
        { 
            return base.Screen as GameScreen;
        }
    }

    //----------------------------------------------
    // Methods
    //-------------------------------------------------------
    public GameScreenRenderer()
    {
        m_cards = new List<CardComponent>();
    }

    protected override void OnInit()
    {
        EventManager.Subscribe<BeloteCard.Played>(this.OnCardPlayed, EventChannel.Post);
        EventManager.Subscribe<GameScreen.NewRoundEvent>(this.OnNewRound);
        EventManager.Subscribe<GameScreen.NewTurnEvent>(this.OnNewTurn);
    }

    protected override void OnShutdown()
    {
        EventManager.UnSubscribe<BeloteCard.Played>(this.OnCardPlayed, EventChannel.Post);
        EventManager.UnSubscribe<GameScreen.NewRoundEvent>(this.OnNewRound);
        EventManager.UnSubscribe<GameScreen.NewTurnEvent>(this.OnNewTurn);
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
            if(Screen.Score != null)
            {
                GUI.Label(new Rect(UnityEngine.Screen.width - 320, 200, 100, 30), "Score : " + Screen.Score.GetScore(PlayerTeam.Team1) + " / " + Screen.Score.GetScore(PlayerTeam.Team2));
                GUI.Label(new Rect(UnityEngine.Screen.width - 320, 230, 100, 30), "Trump : " + Screen.Trump);
                GUI.Label(new Rect(UnityEngine.Screen.width - 320, 260, 100, 30), "Dealer : " + Screen.Dealer.Name);
                GUI.Label(new Rect(UnityEngine.Screen.width - 320, 290, 100, 30), "Bidder : " + Screen.Bidder.Name);
                GUI.Label(new Rect(UnityEngine.Screen.width - 320, 320, 100, 30), "Current : " + Screen.CurrentPlayer.Name);

            }
            

            /*// UI display
            HumanPlayer human = Screen.CurrentPlayer as HumanPlayer;
            if (human != null)
            {
                if (GUI.Button(new Rect(UnityEngine.Screen.width - 120, UnityEngine.Screen.height - 60, 100, 30), "End turn"))
                {
                    EventManager.SendEmptyPooledEvent<EndTurnButtonClicked>();
                }

                GUI.Label(new Rect(20, UnityEngine.Screen.height - 160, 100, 30), "Energy : " + human.Energy);
                GUI.Label(new Rect(20, UnityEngine.Screen.height - 120, 100, 30), "DrawPile : " + human.DrawPile.Size);
                GUI.Label(new Rect(UnityEngine.Screen.width - 120, UnityEngine.Screen.height - 120, 100, 30), "Discard : " + human.DiscardPile.Size);
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
            }*/
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

    private void OnNewRound(GameScreen.NewRoundEvent evt)
    {
        // TODO : Spawn once, then invisible
        if(evt.Start)
        {
            SpawnCards();
        }
        else
        {
            UnSpawnCards();
        }
            
        Refresh();
    }

    private void OnNewTurn(GameScreen.NewTurnEvent evt)
    {
       Refresh();
    }

    protected void OnCardPlayed(BeloteCard.Played evt)
    {
        Refresh();
    }

    protected void SpawnCards()
    {
        foreach (Player player in Screen.Players)
        {
            SpawnCards(player);
        }
    }
    protected void SpawnCards(Player player)
    {
        foreach (BeloteCard card in player.Hand)
        {
            CardComponent newCard = card.Spawn();
            if (newCard)
            {
                m_cards.Add(newCard);
            }
        }
    }

    protected void UnSpawnCards()
    {
        foreach (CardComponent cardObj in m_cards)
        {
            UnityEngine.Object.Destroy(cardObj.gameObject);
        }
        m_cards.Clear();
    }

    protected void UnSpawnCard(CardComponent cardObj)
    {
        m_cards.Remove(cardObj);
        UnityEngine.Object.Destroy(cardObj.gameObject);
    }

    void Refresh()
    {
        foreach (Player player in Screen.Players)
        {
            RefreshHand(player);
        }

        RefreshCurrentFold();

        RemovePastFolds();
    }

     private Vector3 spawnRef = new Vector3();
    private Vector3 rotation = new Vector3();
    protected void RefreshHand(Player player)
    {
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = halfHeight*Camera.main.aspect;

        float spacing = -0.4f;

        if(player.Position == PlayerPosition.South)
        {
            spawnRef.x = -0.5f * halfWidth;
            spawnRef.y = -0.75f * halfHeight;   
        }
        else  if(player.Position == PlayerPosition.West)
        {
            spawnRef.x = -0.85f * halfWidth;
            spawnRef.y = 0.8f * halfHeight;     
        }
        else if(player.Position == PlayerPosition.North)
        {
            spawnRef.x = -0.5f * halfWidth;
            spawnRef.y = 0.75f * halfHeight;   
        }
        else // East
        {
            spawnRef.x = 0.85f * halfWidth;
            spawnRef.y = 0.8f * halfHeight;    
        }
        
        foreach (Card card in player.Hand)
        {
            CardComponent cardComp = GetCardComponent(card);
            if (cardComp)
            {
                cardComp.SetInitialPosition(spawnRef);

                Renderer renderer = cardComp.gameObject.GetComponent<Renderer>();

                if(player.Position == PlayerPosition.South)
                {
                    spawnRef.x += renderer.bounds.size.x + spacing;
                }
                else  if(player.Position == PlayerPosition.West)
                {
                    spawnRef.y -= (renderer.bounds.size.x + spacing);
                    rotation.z = 90.0f;
                    cardComp.gameObject.transform.eulerAngles = rotation;
                }
                else if(player.Position == PlayerPosition.North)
                {
                   spawnRef.x += renderer.bounds.size.x + spacing;
                }
                else // East
                {
                    spawnRef.y -= (renderer.bounds.size.x + spacing);
                    rotation.z = -90.0f;
                    cardComp.gameObject.transform.eulerAngles = rotation;
                }
            }
        }
    }

    void RefreshCurrentFold()
    {
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = halfHeight*Camera.main.aspect;

        foreach (Card card in Screen.CurrentFold.Deck)
        {
            Player player = card.Owner as Player;

            CardComponent cardComp = GetCardComponent(card);
            if (cardComp)
            {
                if(player.Position == PlayerPosition.South)
                {
                    spawnRef.x = 0.0f;
                    spawnRef.y = -0.25f * halfHeight;  
                }
                else  if(player.Position == PlayerPosition.West)
                {
                    spawnRef.x = -0.20f * halfWidth;  
                    spawnRef.y = 0.0f;  
                }
                else if(player.Position == PlayerPosition.North)
                {
                    spawnRef.x = 0.0f;
                    spawnRef.y = 0.25f * halfHeight;
                }
                else // East
                {
                    spawnRef.x = 0.20f * halfWidth;  
                    spawnRef.y = 0.0f;  
                }
                
                cardComp.SetInitialPosition(spawnRef);
            }
        }
    }

    void RemovePastFolds()
    {
        Fold lastFold = Screen.LastFold;
        if(lastFold != null)
        {
            foreach (Card card in lastFold.Deck)
            {
                CardComponent cardComp = GetCardComponent(card);
                if(cardComp != null)
                {
                    UnSpawnCard(cardComp);
                }
            }
        }
    }

    protected CardComponent GetCardComponent(Card card)
    {
        foreach (CardComponent cardObj in m_cards)
        {
            if(cardObj.Card == card)
            {
                return cardObj;
            }
        }
        return null;
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