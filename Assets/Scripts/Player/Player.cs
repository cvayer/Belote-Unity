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
    private   Deck m_hand;

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

    public Deck Hand
    {
        get
        {
            return m_hand;
        }
    }

    public string Name { get; set; }

    public PlayerPosition Position { get; set; }

    //----------------------------------------------
    public Player()
    {
        m_hand = new Deck(this);
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
    protected void Play(Card card, Deck fold)
    {
        if (CanPlay(card))
        {
            DoPlay(card, fold);
        }
    }

    //----------------------------------------------
    public bool CanPlay(Card card)
    {
        if (m_isAllowedToPlay && Hand.Contains(card))
        {
            return true;
        }
        return false;
    }

    //----------------------------------------------
    protected void DoPlay(Card card, Deck fold)
    {
        m_hand.MoveCardTo(card, fold);
        card.OnPlay();
    }

    //----------------------------------------------
    private void OnNewTurn(GameScreen.NewTurnEvent evt)
    {
       if(Screen.CurrentPlayer == this && m_isAllowedToPlay == false)
       {
           m_isAllowedToPlay = true;
           OnTurnStart();
       }
       else if( m_isAllowedToPlay == true)
       {
           m_isAllowedToPlay = false;
           OnTurnStop();
       }
    }

    protected virtual void OnTurnStart() {}
    protected virtual void OnTurnStop() {}

    public void PrintHand()
    {
        Hand.Print(Name);
    }
}
