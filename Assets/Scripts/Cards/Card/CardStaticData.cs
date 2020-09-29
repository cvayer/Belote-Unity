using Sirenix.OdinInspector;
using UnityEngine;

//----------------------------------------------
//----------------------------------------------
// CardStaticData
//----------------------------------------------
//----------------------------------------------
public class CardStaticData : Singleton<CardStaticData>
{
    public CardSpriteRef[] CardSprites;

    [AssetsOnly]
    public Sprite BackSprite;

    [AssetsOnly]
    public GameObject Prefab;


    public Sprite GetSprite(Card32Value Value, Card32Family Family)
    {
        foreach(CardSpriteRef cardRef in CardSprites)
        {
            if(cardRef.Value == Value && cardRef.Family == Family)
            {
                return cardRef.Prefab;
            }
        }
        return null;
    }
}