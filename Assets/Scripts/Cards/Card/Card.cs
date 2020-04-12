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

    public CardValue Value { get; set; }
    public CardFamily Family { get; set; }

    public IDeckOwner Owner { get; set; }

    public int Point { get; set; }
    public int TrumpPoint { get; set; }

    //----------------------------------------------
    public Card()
    {
    }

    //----------------------------------------------
    public CardComponent Spawn()
    {
        if (CardStaticData.Instance.Prefab != null)
        {
            GameObject cardObj = Object.Instantiate(CardStaticData.Instance.Prefab) as GameObject;
            CardComponent cardComp = cardObj.GetComponent<CardComponent>();
            if(cardComp != null)
            {
                cardComp.Init(this);
            }
            return cardComp;
        }
        return null;
    }

    public void OnPlay()
    {
        Played evt = Pools.Claim<Played>();
        evt.Init(this);
        EventManager.SendEvent(evt);
    }

    public int GetPoint(CardFamily? trumpFamily)
    {
        if(trumpFamily != null && Family == trumpFamily)
        {
            return TrumpPoint;
        }
        return Point;
    }

    public override string ToString()
    {
        return "(" + Value + " " + Family + ")";
    }
}
