using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------
//----------------------------------------------
// HumanPlayer
//----------------------------------------------
//----------------------------------------------
public class HumanPlayer : Player
{


    //----------------------------------------------
    public HumanPlayer()
    {
    }

    //----------------------------------------------
    protected override void OnInit()
    {
        EventManager.Subscribe<Card.Selected>(this.OnCardSelectedEvent);
    }

    //--------------------------------------------------------------------
    protected override void OnShutdown()
    {
        EventManager.UnSubscribe<Card.Selected>(this.OnCardSelectedEvent);
    }

    private void OnCardSelectedEvent(Card.Selected evt)
    {
        if(evt.IsSelected == false && evt.OutsideOfHand)
        {
            Play(evt.Card, Screen.CurrentFold);
        }
    }
}

