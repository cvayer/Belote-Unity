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
    protected PlayerTeam m_team;


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

    public PlayerTeam Team
    {
        get
        {
            return m_team;
        }
        set
        {
            m_team = value;
        }
    }

    public Deck Hand
    {
        get
        {
            return m_hand;
        }
    }

    public string Name { get; set; }

    //----------------------------------------------
    public Player()
    {
        m_hand = new Deck(this);
    }

    //----------------------------------------------
    public void Init()
    {
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
    protected bool CanPlay(Card card)
    {
        if (Hand.Contains(card))
        {
            return true;
        }
        return false;
    }

    //----------------------------------------------
    protected void DoPlay(Card card, Deck fold)
    {
        m_hand.MoveCardTo(card, fold);
    }

    public void PrintHand()
    {
        Hand.Print(Name);
    }
}
