using System;
using UnityEngine;

public class AIPlayer : Player
{
    public AIPlayer()
    {

    }

    //----------------------------------------------
    protected override void OnInit()
    {
       
    }

    //--------------------------------------------------------------------
    protected override void OnShutdown()
    {
        
    }

    //--------------------------------------------------------------------
    protected override void OnTurnStart() 
    {
        PlayAtRandom();
    }

    //--------------------------------------------------------------------
    protected override void OnTurnStop() 
    {

    }

    //--------------------------------------------------------------------
    void PlayAtRandom()
    {
        if(TurnPlayableCards != null && ! TurnPlayableCards.Empty)
        {
            int indexToPlay = UnityEngine.Random.Range(0, TurnPlayableCards.Size);
            Play(TurnPlayableCards.Cards[indexToPlay], Stage.CurrentFold);
        }
         
    }
}

