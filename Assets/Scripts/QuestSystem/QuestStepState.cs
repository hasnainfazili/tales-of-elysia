using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestStepState
{
    public string state;
    public string status;
    public string description;
    public QuestStepState(string state, string status, string description) 
    {
        this.state = state;
        this.status = status;
        this.description = description;
    }

    public QuestStepState()
    {
        this.state = "";
        this.status = "";
        this.description = "";
    }
}
