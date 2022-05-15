using System.Collections.Generic;
using System;
using Pebble;

[Serializable]
public class CardPoint
{
    public Card32Value Value;
    public int Point = 0;
    public int TrumpPoint = 0;
}

[Serializable]
public class ScoringData 
{
    public List<CardPoint> Points;
    public int LastFold = 0;
    public int Rebelote = 20;

    public int GetPoint(Card32Value value, bool trump)
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
