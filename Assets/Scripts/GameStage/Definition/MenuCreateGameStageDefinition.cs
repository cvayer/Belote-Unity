using UnityEngine;
using UnityEditor;
using System.Collections;
using Pebble;

class MenuCreateGameStageDefinition : MenuCreateBase
{
    [MenuItem("Assets/Create/Belote/Definition/GameStage")]
    public static void Create()
    {
        Create<GameStageDefinition>("GameStageDefinition");
    }
}
