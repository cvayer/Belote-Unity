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

    protected override void OnTurnStart() 
    {
        PlayAtRandom();
    }

    protected override void OnTurnStop() 
    {

    }

    void PlayAtRandom()
    {
         int indexToPlay = UnityEngine.Random.Range(0, Hand.Size);
         Play(Hand.Cards[indexToPlay], Screen.CurrentFold);
    }
}

