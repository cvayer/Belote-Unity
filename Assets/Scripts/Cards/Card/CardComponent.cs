using System;
using UnityEngine;

//----------------------------------------------
//----------------------------------------------
// Card
//----------------------------------------------
//----------------------------------------------
public class CardComponent : MonoBehaviour
{
    //----------------------------------------------
    // Variables
    private Card m_card;
    private bool m_isHovered = false;
    private bool m_isSelected = false;
    private Vector3 m_posInHand = new Vector3();

    //----------------------------------------------
    // Properties

    public bool Hovered
    {
        get { return m_isHovered; }
    }

    public bool Selected
    {
        get { return m_isSelected; }
    }

    public Card Card
    {
        get { return m_card; }
    }

    // Use this for initialization
    void Start()
    {
        
    }

    public void Init(Card card)
    {
        m_card = card;
        m_isHovered = false;
        m_isSelected = false;
       /* SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = m_card.Definition.Color;
        }*/
    }

    public void SetPosInHand(Vector3 posInHand)
    {
        transform.localPosition = posInHand;
        m_posInHand = posInHand;
    }
    

    // Update is called once per frame
    void Update()
    {
        GameObject underMouse = Picker.Instance.UnderMouse;

        if (underMouse != null && underMouse == gameObject)
        {
            SetHovered(true);
        }
        else
        {
            SetHovered(false);
        }

        if(Hovered && !Selected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetSelected(true);
            }
        }

        if (Selected && Input.GetMouseButtonUp(0))
        {
            SetSelected(false);
        }

        if(m_isSelected)
        {
            Vector3 newPos = new Vector3();
            newPos = Picker.Instance.MouseWorldPos;
            newPos.z -= 0.1f;
            transform.localPosition = newPos;
        }
        else
        {
            transform.localPosition = m_posInHand;
        }
    }

    static float scaleFactor = 1.2f;
    static float invScaleFactor = 1.0f / scaleFactor;
    protected void SetHovered(bool under)
    {
        if (under != m_isHovered)
        {
            if (m_isHovered)
            {
                Vector3 scale = gameObject.transform.localScale;
                scale.x *= invScaleFactor;
                scale.y *= invScaleFactor;
                gameObject.transform.localScale = scale;
            }
            m_isHovered = under;
            if (m_isHovered)
            {
                Vector3 scale = gameObject.transform.localScale;
                scale.x *= scaleFactor;
                scale.y *= scaleFactor;
                gameObject.transform.localScale = scale;
            }
        }
    }

    protected void SetSelected(bool selected)
    {
        if(selected != m_isSelected)
        {
            bool isInHandArea = IsInHandArea();

            m_isSelected = selected;

            Card.Selected evt = Pools.Claim<Card.Selected>();
            evt.Init(m_card, m_isSelected, isInHandArea);
            EventManager.SendEvent(evt);
        }
    }

    protected bool IsInHandArea()
    {
        if(Selected)
        {
            Rect rect = new Rect(0, Screen.height / 2, Screen.width, Screen.height / 2);
            return rect.Contains(Input.mousePosition);
        }
        return false;
    }
}