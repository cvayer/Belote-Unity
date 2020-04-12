using System.Collections.Generic;
using UnityEngine;

public class Fold
{
    public Deck Deck { get; set;}

    public Player Winner { get; set; }

    public int Points { get; set; }

    public CardFamily? Requested
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
        Deck = new Deck();
    }

    public void MoveTo(Fold fold)
    {
        Deck.MoveAllCardsTo(fold.Deck);
        fold.Winner = Winner;
        fold.Points = Points;
        Winner = null;
        Points = 0;
    }

    public void Finalize(CardFamily trumpFamily)
    {
        Card bestCard = GetBest(trumpFamily);
        if(bestCard != null)
        {
            Winner = bestCard.Owner as Player;
            Points = GetPoints(trumpFamily);
        }
    }

    public Card GetBest(CardFamily trumpFamily)
    {
        CardFamily? requested = Requested;
        if(requested != null)
        {
            Card bestCard = Deck.Cards[0];
            if(Deck.Cards.Count > 1)
            {
                for(int  i = 1; i < Deck.Cards.Count ; ++i)
                {
                    Card card = Deck.Cards[i];

                    if(card.Family == bestCard.Family)
                    {
                        int cardPoint = card.GetPoint(trumpFamily);
                        int bestCardpoint = bestCard.GetPoint(trumpFamily);
                        if(cardPoint > bestCardpoint)
                        {
                            bestCard = card;
                        }    
                    }
                    else
                    {
                        if(card.Family == trumpFamily)
                        {
                            bestCard = card;
                        }     
                    }

                }    
            }
            return bestCard;
        }
        return null;
    }

    public int GetPoints(CardFamily trumpFamily)
    {
        int points = 0;
        foreach(Card card in Deck.Cards)
        {
            points += card.GetPoint(trumpFamily);
        }
        return points;
    }
}