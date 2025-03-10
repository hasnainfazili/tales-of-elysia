using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireTutorialQuestStep : QuestStep
{
    private string state;
    private string status;
    [SerializeField] private string description;
    private void OnEnable()
    {
        EventsManager.instance.miscEvents.onQuestStepCompleted += QuestStepCompleted;
        EventsManager.instance.miscEvents.onCampfireActivated += QuestStepCompleted;
        EventsManager.instance.miscEvents.ObjectiveUpdate(description);
    }
    private void OnDisable()
    {
        EventsManager.instance.miscEvents.onQuestStepCompleted -= QuestStepCompleted;
        EventsManager.instance.miscEvents.onCampfireActivated -= QuestStepCompleted;
        EventsManager.instance.miscEvents.ObjectiveUpdate("Talk to Guard to Complete tutorial");


    }
    
    private void QuestStepCompleted()
    {
        state = "Completed";
        UpdateState();
        FinishQuestStep();
    }

    private void UpdateState()
    {
        string state = this.state;
        string status = this.status;
        string description = this.description;

        ChangeState(state, status, description);
    }

    protected override void SetQuestStepState(string state)
    {
        UpdateState();
    }
}
