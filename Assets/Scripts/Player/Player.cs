using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------
//----------------------------------------------
// Player
//----------------------------------------------
//----------------------------------------------
public class Player : IDeckOwner
{

    //----------------------------------------------
    // Variables
    protected GameScreen m_screen;
    private   BeloteDeck m_hand;

    private bool m_isAllowedToPlay = false;

    //----------------------------------------------
    // Properties

    public GameScreen Screen
    {
        get
        {
            return m_screen;
        }
        set
        {
            m_screen = value;
        }
    }

    public PlayerTeam Team { get; set; }

    public BeloteDeck Hand
    {
        get
        {
            return m_hand;
        }
    }

    public BeloteDeck TurnPlayableCards
    {
        get; set;
    }

    public string Name { get; set; }

    public PlayerPosition Position { get; set; }

    //----------------------------------------------
    public Player()
    {
        m_hand = new BeloteDeck(this);
    }

    //----------------------------------------------
    public void Init()
    {
        EventManager.Subscribe<GameScreen.NewTurnEvent>(this.OnNewTurn);

        OnInit();
    }

    //----------------------------------------------
    protected virtual void OnInit()
    {

    }

    //--------------------------------------------------------------------
    public void Shutdown()
    {
        OnShutdown();
        EventManager.UnSubscribe<GameScreen.NewTurnEvent>(this.OnNewTurn);
    }

    //--------------------------------------------------------------------
    protected virtual void OnShutdown()
    {

    }

   
    //----------------------------------------------
    public void Update()
    {
        OnUpdate();
    }

    //----------------------------------------------
    protected virtual void OnUpdate()
    {

    }

    //----------------------------------------------
    protected void Play(BeloteCard card, Fold fold)
    {
        if (CanPlay(card))
        {
            DoPlay(card, fold);
        }
    }

    //----------------------------------------------
    public bool CanPlay(BeloteCard card)
    {
        if (m_isAllowedToPlay && Hand.Contains(card))
        {
            if(TurnPlayableCards != null && TurnPlayableCards.Contains(card))
                return true;
        }
        return false;
    }

    //----------------------------------------------
    protected void DoPlay(BeloteCard card, Fold fold)
    {
        m_hand.MoveCardTo(card, fold.Deck);
        card.OnPlay();
    }

    List<BeloteCard>  m_trumpCards = new List<BeloteCard> ();
    List<BeloteCard>  m_trumpBetterCards = new List<BeloteCard> ();

    protected BeloteDeck ComputePlayableCards(Fold fold, Card32Family trumpFamily)
    {
        BeloteDeck playables = new BeloteDeck();

        m_trumpCards.Clear();
        m_trumpBetterCards.Clear();

        if(!Hand.Empty)
        {
            // No cards in the fold, all cards are valid
            if(fold.RequestedFamily == null)
            {
                playables.CopyFrom(Hand);
            }
            else
            {
                BeloteCard bestCard = fold.GetBest(trumpFamily);
                Player bestPlayer = bestCard.Owner as Player;

                Card32Family requestedFamily = (Card32Family)fold.RequestedFamily;

                // We look for cards of the requested families
                foreach(BeloteCard card in Hand.Cards)
                {
                    if(card.Family == requestedFamily)
                    {
                        playables.AddCard(card);
                    }

                    if(card.Family == trumpFamily)
                    {
                        m_trumpCards.Add(card);

                        if(bestCard.Family == trumpFamily)
                        {
                            if(BeloteCard.GetBestCard(card, bestCard, trumpFamily) == card)
                            {
                                m_trumpBetterCards.Add(card);
                            }
                        }
                    }
                }

                // Remove all trump cards that are too low
                if(!playables.Empty && trumpFamily == requestedFamily)
                {
                    if(m_trumpBetterCards.Count > 0)
                    {
                        playables.Clear();
                        playables.AddCards(m_trumpBetterCards);
                    }
                }


                // No card of the requested family
                if(playables.Empty)
                {
                    // Best card is partner we can play what we want
                    if(bestPlayer.Team == this.Team)
                    {
                        playables.CopyFrom(Hand);
                    }
                    else
                    {
                        if(bestCard.Family == trumpFamily)
                        {
                            if(m_trumpBetterCards.Count > 0)
                            {
                                playables.AddCards(m_trumpBetterCards);
                            }
                            else // TODO : Add "pisser" rules
                            {
                                playables.AddCards(m_trumpCards);
                            }
                        }
                        else
                        {
                            playables.AddCards(m_trumpCards);
                        }

                        if(playables.Empty)
                        {
                            playables.CopyFrom(Hand);
                        }    
                    }
                }
            }
        }
        return playables;
    }

    //----------------------------------------------
    private void OnNewTurn(GameScreen.NewTurnEvent evt)
    {
       if(evt.Previous == this)
       {
           m_isAllowedToPlay = false;
           OnTurnStop();
           TurnPlayableCards = null;
       }

       if(evt.Current == this)
       {
           m_isAllowedToPlay = true;

           TurnPlayableCards = ComputePlayableCards(Screen.CurrentFold, Screen.Trump);
           OnTurnStart();
       }
    }

    protected virtual void OnTurnStart() {}
    protected virtual void OnTurnStop() {}

    public void PrintHand()
    {
        Hand.Print(Name);
    }
}
