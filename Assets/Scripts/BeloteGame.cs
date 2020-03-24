using System;
using UnityEngine;

class BeloteGame : MonoBehaviour
{
    public GameScreenDefinition GameDefinition;

    IScreen screen = null;

    // Use this for initialization
    void Start()
    {
        screen = GameDefinition.CreateGame();
        if(screen != null)
        {
            screen.Init();
            screen.Start();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (screen != null)
        {
            screen.Update();
        }
    }

    void OnGUI()
    {
        if (screen != null)
        {
            screen.UpdateGUI();
        }
    }

    public virtual void OnDestroy()
    {
        if (screen != null)
        {
            screen.Stop();
            screen.Shutdown();
        }
    }
}
