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

    private readonly GameScreenDefinition      m_definition;
    private Deck                               m_deck;
    private Deck                               m_currentFold;

    private List<Deck>                         m_pastFolds;

    private ActionQueue                        m_actionQueue;

    private EndState m_endState;

    private static int s_invalidPlayerIndex = -1;
    private int m_currentPlayerIndex = s_invalidPlayerIndex;


    private static int s_invalidRoundCount = -1;

    private int m_foldsInOneRound = s_invalidRoundCount;
    private int m_currentRound = s_invalidRoundCount;


    private static float s_afterPlayDuration = 1.0f;
    private float m_afterPlayTimer = -1.0f;

    //----------------------------------------------
    // Properties
    public Player CurrentPlayer
    {
        get
        {
            if(m_currentPlayerIndex >=0 && m_currentPlayerIndex < m_players.Count)
            {
                return m_players[m_currentPlayerIndex];
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

    public Deck CurrentFold
    {
        get { return m_currentFold; }
    }

    public List<Deck> PastFolds
    {
        get { return m_pastFolds; }
    }

    public Deck LastFold
    {
        get 
        { 
            if(PastFolds.Count > 0)
            {
                return PastFolds.Last();   
            }
            return null;
        }
    }

    


    //----------------------------------------------
    public GameScreen(GameScreenDefinition definition)
    {
        m_definition = definition;
        m_players = new List<Player>();
        m_actionQueue = new ActionQueue();
        m_endState = EndState.None;
        m_deck = new Deck(this);
        m_currentFold = new Deck(); // A fold has no owner so that cards retain their previous players
        m_pastFolds = new List<Deck>();
    }

    //----------------------------------------------
    protected override void OnInit()
    {
        Renderer.SetScreen(this);

        m_deck.Init(m_definition.Scoring);

        EventManager.Subscribe<Card.Played>(this.OnCardPlayed);
    }

    
    protected override void OnShutdown()
    {
        EventManager.UnSubscribe<Card.Played>(this.OnCardPlayed);

        foreach (Player player in m_players)
        {
            player.Shutdown();
        }

        m_deck.Clear();
    }


    //----------------------------------------------
    protected override void OnStart() 
    {
        m_deck.Shuffle();
        AddPlayers();

        m_foldsInOneRound = m_deck.Size / Players.Count;

        DealCards();
        StartFirstTurn();
    }

    protected override void OnStop()
    {

    }

    //----------------------------------------------
    protected override void OnUpdate()
    {
        UpdatePlayers();

        if(m_afterPlayTimer >= 0.0f)
        {
            m_afterPlayTimer -= Time.deltaTime;
            if(m_afterPlayTimer <= 0.0f)
            {
                m_afterPlayTimer = -1.0f;
                OnAfterPlayTimerDone();
            }
        }


        if(CurrentPlayer != null)
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
    protected void AddPlayer<PlayerType>(PlayerTeam team, PlayerPosition position, string name)  where PlayerType : Player, new()
    {
        PlayerType newPlayer = new PlayerType();
        newPlayer.Screen = this;
        newPlayer.Team = team;
        newPlayer.Name = name;
        newPlayer.Position = position;
        newPlayer.Init();
        m_players.Add(newPlayer);
        
    }

    //----------------------------------------------
    protected void AddPlayers()
    {
        AddPlayer<HumanPlayer>(PlayerTeam.Team1, PlayerPosition.South, "South");
        AddPlayer<AIPlayer>(PlayerTeam.Team2, PlayerPosition.West, "West");
        AddPlayer<AIPlayer>(PlayerTeam.Team1, PlayerPosition.North, "North");
        AddPlayer<AIPlayer>(PlayerTeam.Team2, PlayerPosition.East, "East");
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
            player.Hand.SortByFamilyAndValue();
         //   player.PrintHand();
        }

        DeckDealtEvent evt = Pools.Claim<DeckDealtEvent>();
        EventManager.SendEvent(evt);
    }

    //----------------------------------------------
    void StartFirstTurn()
    {
        if(m_players.Count > 0)
        {
            m_currentRound = 0;
            StartNextTurn();
        }
    }

    //----------------------------------------------
    void StartNextTurn()
    {
        m_currentPlayerIndex = (m_currentPlayerIndex + 1)% m_players.Count;

        NewTurnEvent evt = Pools.Claim<NewTurnEvent>();
        EventManager.SendEvent(evt);
    }

    //----------------------------------------------
    protected  void UpdatePlayers()
    {
        foreach (Player player in m_players)
        {
            player.Update();
        }
    }

    protected void OnCardPlayed(Card.Played evt)
    {
        m_afterPlayTimer = s_afterPlayDuration;
    }

    protected void OnAfterPlayTimerDone()
    {
        if(CurrentFold.Size == Players.Count)
        {
            Deck newFold = new Deck();
            PastFolds.Add(newFold);
            CurrentFold.MoveAllCards(newFold);
        }

        if(PastFolds.Count == m_foldsInOneRound)
        {
            // Next Round;
            m_currentRound ++;


            StartNextTurn();
        }
        else
        {
            StartNextTurn();    
        }
    }

    //------------------------------------
    // Events
    public class DeckDealtEvent : PooledEvent
    {
        public override void Reset()
        {

        }
    }

    public class NewTurnEvent : PooledEvent
    {
        public override void Reset()
        {

        }
    }
}




