using Sirenix.OdinInspector;
using UnityEngine;
using System;
//-------------------------------------------------------
//-------------------------------------------------------
// CardPrefabRef
//-------------------------------------------------------
//-------------------------------------------------------
[Serializable]
public class CardSpriteRef
{
    public CardEnum Value;
    public CardFamilyEnum FamilyEnum;

    [AssetsOnly]
    public Sprite Prefab;
}