using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{   
    public static EventsManager instance {get; private set;}
    public MiscEvents miscEvents;
    public QuestEvents questEvents;
    public PlayerEvents playerEvents;
    public GameEvents gameEvents;

   
    private void Awake() {
        if(instance != null) {
            Debug.LogError("Found more than one Event Manager in the scene");
        }
        instance = this;
        playerEvents = new PlayerEvents();
        miscEvents = new MiscEvents();
        questEvents = new QuestEvents();
        gameEvents = new GameEvents();
    }
}
