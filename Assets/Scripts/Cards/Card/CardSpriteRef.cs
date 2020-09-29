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
    public Card32Value Value;
    public Card32Family Family;

    [AssetsOnly]
    public Sprite Prefab;
}