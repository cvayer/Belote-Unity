using System.Collections;
using UnityEngine;

//----------------------------------------------
//----------------------------------------------
// Card
//----------------------------------------------
//----------------------------------------------
public partial class Card
{
    //----------------------------------------------
    // Variables

    public CardEnum Value { get; set; }
    public CardFamilyEnum Family { get; set; }

    public IDeckOwner Owner { get; set; }

    public int Point { get; set; }
    public int TrumpPoint { get; set; }

    //----------------------------------------------
    public Card()
    {
    }

    //----------------------------------------------
 /*   public CardComponent Spawn()
    {
        if (CardStaticData.Instance.Prefab != null)
        {
            GameObject cardObj = Object.Instantiate(CardStaticData.Instance.Prefab);
            CardComponent cardComp = cardObj.GetComponent<CardComponent>();
            if(cardComp != null)
            {
                cardComp.Init(this);
            }

            return cardComp;
        }
        return null;
    }*/

    public void Play(ActionQueue actionQueue)
    {
        Played evt = Pools.Claim<Played>();
        evt.Init(this);
        EventManager.SendEvent(evt);
    }

    public override string ToString()
    {
        return "(" + Value + " " + Family + ")";
    }
}
