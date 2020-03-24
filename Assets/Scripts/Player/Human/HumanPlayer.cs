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
        EventManager.Subscribe<Turn.StateEvent>(this.OnTurnStateChanged);
        EventManager.Subscribe<Card.Selected>(this.OnCardSelectedEvent);
    }

    //--------------------------------------------------------------------
    protected override void OnShutdown()
    {
        EventManager.UnSubscribe<Turn.StateEvent>(this.OnTurnStateChanged);
        EventManager.UnSubscribe<Card.Selected>(this.OnCardSelectedEvent);
    }

    private void OnTurnStateChanged(Turn.StateEvent evt)
    {

    }

    private void OnCardSelectedEvent(Card.Selected evt)
    {
      /*  if(evt.IsSelected == false && evt.OutsideOfHand)
        {
            Play(evt.Card);
        }*/
    }
}

