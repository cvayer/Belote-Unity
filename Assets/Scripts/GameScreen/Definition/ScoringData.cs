using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class CardPoint
{
    public CardEnum Value;
    public int Point = 0;
    public int TrumpPoint = 0;
}

[Serializable]
public class ScoringData 
{
    public List<CardPoint> Points;
    public int LastFold = 0;
    public int Rebelote = 20;

    public int GetPoint(CardEnum value, bool trump)
    {
        foreach(CardPoint point in Points)
        {
            if(point.Value == value)
            {
                return (trump) ? point.TrumpPoint : point.Point;
            }
        }
        return 0;
    }
}
