using System;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------
//----------------------------------------------
// GameScreen
//----------------------------------------------
//----------------------------------------------
public class GameScreen : Screen<GameScreenRenderer>, IDeckOwner
{
    public enum EndState
    {
        None,
        Success, 
        Fail
    }

    //----------------------------------------------
    // Variables
    private List<Player>                       m_players;
    private List<Turn>                         m_turns;
    private readonly GameScreenDefinition      m_definition;
    private Deck                               m_deck;
    private Deck                               m_fold;

    private ActionQueue                        m_actionQueue;

    private EndState m_endState;

    private static int s_invalidTurnIndex = -1;
    private int m_currentTurnIndex = s_invalidTurnIndex;

    //----------------------------------------------
    // Properties
    public Turn CurrentTurn
    {
        get
        {
            if(m_currentTurnIndex >=0 && m_currentTurnIndex < m_turns.Count)
            {
                return m_turns[m_currentTurnIndex];
            }
            return null;
        }
    }

    public List<Player> Players
    {
        get
        {
            return m_players;
        }
    }

    public ActionQueue ActionQueue
    {
        get
        {
            return m_actionQueue;
        }
    }

    public bool HasEnded
    {
        get{ return !(m_endState == EndState.None); }
    }

    public bool Succeded
    {
        get { return m_endState == EndState.Success; }
    }

    public bool Failed
    {
        get { return m_endState == EndState.Fail; }
    }


    //----------------------------------------------
    public GameScreen(GameScreenDefinition definition)
    {
        m_definition = definition;
        m_players = new List<Player>();
        m_turns = new List<Turn>();
        m_actionQueue = new ActionQueue();
        m_endState = EndState.None;
        m_deck = new Deck(this);
        m_fold = new Deck(); // A fold has no owner so that cards retain their previous players
    }

    //----------------------------------------------
    protected override void OnInit()
    {
        Renderer.SetScreen(this);

        m_deck.Init(m_definition.Scoring);
    }

    //----------------------------------------------
    protected override void OnStart() 
    {
        m_deck.Shuffle();
        AddPlayers();
        DealCards();
        StartFirstTurn();
    }

    protected override void OnStop()
    {

    }

    protected override void OnShutdown()
    {
        foreach (Turn turn in m_turns)
        {
            turn.Shutdown();
        }

        foreach (Player player in m_players)
        {
            player.Shutdown();
        }
    }

    //----------------------------------------------
    protected override void OnUpdate()
    {
        if(CurrentTurn != null && CurrentTurn.IsStopped)
        {
            StartNextTurn();
        }

        UpdatePlayers();

        if(CurrentTurn != null)
        {
            m_actionQueue.Process();
        }
    }

    //----------------------------------------------
    protected void SetEndState(EndState state)
    {
        if(m_endState != state)
        {
            m_endState = state;
        }
    }

  

    //----------------------------------------------
    protected void AddPlayer<PlayerType>(PlayerTeam team, string name)  where PlayerType : Player, new()
    {
        PlayerType newPlayer = new PlayerType();
        newPlayer.Screen = this;
        newPlayer.Team = team;
        newPlayer.Name = name;
        AddTurn(newPlayer);
        newPlayer.Init();
        m_players.Add(newPlayer);
        
    }

    //----------------------------------------------
    protected void AddTurn(Player player)
    {
        Turn newTurn = new Turn();
        newTurn.Init(player);
        m_turns.Add(newTurn);
    }

    //----------------------------------------------
    protected void AddPlayers()
    {
        AddPlayer<AIPlayer>(PlayerTeam.Team1, "Sud");
        AddPlayer<AIPlayer>(PlayerTeam.Team2, "Est");
        AddPlayer<AIPlayer>(PlayerTeam.Team1, "North");
        AddPlayer<AIPlayer>(PlayerTeam.Team2, "West");
    }

    protected void DealCards()
    {
        for(int  iDeal = 0; iDeal < m_definition.DealingRules.Dealings.Count; ++iDeal)
        {
            int dealing = m_definition.DealingRules.Dealings[iDeal];
            foreach (Player player in m_players)
            {
                m_deck.MoveCardsTo(dealing, player.Hand);
            }
        }
        foreach (Player player in m_players)
        {
            player.PrintHand();
        }
    }

    //----------------------------------------------
    void StartFirstTurn()
    {
        if(m_turns.Count > 0)
        {
            StartNextTurn();
        }
    }

    //----------------------------------------------
    void StartNextTurn()
    {
        m_currentTurnIndex = (m_currentTurnIndex + 1)% m_turns.Count;

        if(CurrentTurn != null)
        {
            CurrentTurn.Start();
        }
    }

    //----------------------------------------------
    protected  void UpdatePlayers()
    {
        foreach (Player player in m_players)
        {
            player.Update();
        }
    }
}


