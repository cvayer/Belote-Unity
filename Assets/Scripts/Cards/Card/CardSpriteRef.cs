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
    public CardValue Value;
    public CardFamily Family;

    [AssetsOnly]
    public Sprite Prefab;
}