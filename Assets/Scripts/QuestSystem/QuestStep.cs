using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;
    private int stepIndex;
    [SerializeField] private string stepDescription;
    public void InitializeQuestStep(string questId, int stepIndex, string questStepState, string stepDescription)
    {
        this.questId = questId;
        this.stepIndex = stepIndex;
        this.stepDescription = stepDescription;
        if(questStepState != null && questStepState != "")
        {
            SetQuestStepState(questStepState);
        }
        
    }
    protected void FinishQuestStep() {
        if(!isFinished) {
            isFinished = true;
            EventsManager.instance.questEvents.AdvanceQuest(questId);
            Destroy(gameObject);
        } 
    }
    protected void ChangeState(string newState, string newStatus, string newDescription) {
        EventsManager.instance.questEvents.QuestStepStateChange(questId, stepIndex, new QuestStepState(newState, newStatus, newDescription));
    }

    protected abstract void SetQuestStepState(string state);
}
