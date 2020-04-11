using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------
//----------------------------------------------
// Deck
//----------------------------------------------
//----------------------------------------------

public class Deck
{
    //----------------------------------------------
    // Variables
    private List<Card> m_cards = null;
    private IDeckOwner m_owner = null;
    //----------------------------------------------
    // Properties
    public int Size
    {
        get
        {
            return m_cards.Count;
        }
    }

    public List<Card> Cards
    {
        get
        {
            return m_cards;
        }
    }

    public bool Empty
    {
        get
        {
            return Size == 0;
        }
    }

    public IDeckOwner Owner
    {
        get
        {
            return m_owner;
        }
    }


    //------------------------------------------------------
    public Deck(IDeckOwner owner)
    {
        m_owner = owner;
        m_cards = new List<Card>();
    }

    public Deck() : this(null)
    {

    }

    public List<Card>.Enumerator GetEnumerator()
    {
        return m_cards.GetEnumerator();
    }

    //------------------------------------------------------
    public void Init(ScoringData scoring)
    {
        foreach(CardFamilyEnum family in (CardFamilyEnum[])System.Enum.GetValues(typeof(CardFamilyEnum)))
        {
            foreach (CardEnum value in (CardEnum[])System.Enum.GetValues(typeof(CardEnum)))
            {
                Card card = new Card();
                card.Family = family;
                card.Value = value;
                card.Point = scoring.GetPoint(value, false);
                card.TrumpPoint = scoring.GetPoint(value, true);
                AddCard(card);
            }
        }
    }

    public void Clear()
    {
        // TODO : Pool and release cards
        m_cards.Clear();
    }

    //------------------------------------------------------
    public void Shuffle()
    {
        m_cards.Shuffle();
        m_cards.Shuffle();
        m_cards.Shuffle();
    }

    //------------------------------------------------------
    public void AddCard(Card card)
    {
        if(Owner != null) // A deck with no ownership cannot set the new owner
        {
            card.Owner = Owner;
        }
        m_cards.Add(card);
    }

    //------------------------------------------------------
    public void AddCards(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            AddCard(card);
        }
    }

    //------------------------------------------------------
    public void RemoveCard(Card card)
    {
        m_cards.Remove(card);
    }

    //------------------------------------------------------
    public bool Contains(Card card)
    {
        return m_cards.Contains(card);
    }

    //------------------------------------------------------
    public int MoveCardsTo(int requested, Deck other)
    {
        int oldSize = Size;
        if(requested >= oldSize)
        {
            other.AddCards(m_cards);
            m_cards.Clear();
            return oldSize;
        }
        else
        {
            int index = oldSize - requested;
            List<Card> drawed = m_cards.GetRange(index, requested);
            other.AddCards(drawed);
            m_cards.RemoveRange(index, requested);
            return requested;
        }
    }

    //------------------------------------------------------
    public bool MoveCardTo(Card card, Deck other)
    {
        if(Contains(card))
        {
            other.AddCard(card);
            m_cards.Remove(card);
            return true;
        }
        return false;
    }

    //------------------------------------------------------
    public void MoveAllCards(Deck other)
    {
        MoveCardsTo(Size, other);
    }

    public bool IsOverDraw(int requested)
    {
        return requested > Size;
    }

    private static int CompareByFamilyAndValue(Card a, Card b)
    {
        int compareFamily = a.Family.CompareTo(b.Family);
        if(compareFamily == 0)
        {
              return a.Value.CompareTo(b.Value);  
        }
        return compareFamily;
    }

    public void SortByFamilyAndValue()
    {
        Cards.Sort(CompareByFamilyAndValue);
    }

    public void Print(string prefix)
    {
        string toPrint = prefix + " : ";
        if(Cards.Count > 0)
        {
            foreach (Card card in Cards)
            {
                toPrint += card.ToString() + ", ";
            }
        }
        else 
        {
            toPrint += "No Cards";
        }
           
        Debug.Log(toPrint);
    }
}