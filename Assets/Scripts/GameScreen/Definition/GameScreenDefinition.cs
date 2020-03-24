using UnityEngine;


//-------------------------------------------------------
//-------------------------------------------------------
// GameScreenDefinition
//-------------------------------------------------------
//-------------------------------------------------------
public class GameScreenDefinition : ScreenDefinition
{ 
    public DealingRulesData DealingRules;

    public ScoringData Scoring;

    public GameScreen CreateGame()
    {
        return new GameScreen(this);
    }
}
