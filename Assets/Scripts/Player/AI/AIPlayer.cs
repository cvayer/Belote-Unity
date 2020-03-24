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
        EventManager.Subscribe<Turn.StateEvent>(this.OnTurnStateChanged);
    }

    //--------------------------------------------------------------------
    protected override void OnShutdown()
    {
        EventManager.UnSubscribe<Turn.StateEvent>(this.OnTurnStateChanged);
    }

    private void OnTurnStateChanged(Turn.StateEvent evt)
    {

    }
}

