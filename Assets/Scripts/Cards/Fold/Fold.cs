using System.Collections.Generic;
using UnityEngine;

public class Fold
{
    public BeloteDeck Deck { get; set;}

    public Player Winner { get; set; }

    public int Points { get; set; }

    public Card32Family? RequestedFamily
    {
        get
        {
             if(Deck.Cards.Count > 0)
             {
                 return Deck.Cards.Front().Family;
             }   
             return null;
        }
    }

    public Fold()
    {
        Deck = new BeloteDeck();
    }

    public void MoveTo(Fold fold)
    {
        Deck.MoveAllCardsTo(fold.Deck);
        fold.Winner = Winner;
        fold.Points = Points;
        Winner = null;
        Points = 0;
    }

    public void Finalize(Card32Family trumpFamily)
    {
        BeloteCard bestCard = GetBest(trumpFamily);
        if(bestCard != null)
        {
            Winner = bestCard.Owner as Player;
            Points = GetPoints(trumpFamily);
        }
    }

    public BeloteCard GetBest(Card32Family trumpFamily)
    {
        Card32Family? requested = RequestedFamily;
        if(requested != null)
        {
            BeloteCard bestCard = Deck.Cards[0];
            if(Deck.Cards.Count > 1)
            {
                for(int  i = 1; i < Deck.Cards.Count ; ++i)
                {
                    BeloteCard card = Deck.Cards[i];

                    bestCard = BeloteCard.GetBestCard(card, bestCard, trumpFamily);
                }    
            }
            return bestCard;
        }
        return null;
    }

    public int GetPoints(Card32Family trumpFamily)
    {
        int points = 0;
        foreach(BeloteCard card in Deck.Cards)
        {
            points += card.GetPoint(trumpFamily);
        }
        return points;
    }
}