using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTutorialQuestStep : QuestStep
{
    [SerializeField] private KeyCode input;
    private bool state = false;
    private string status;
    [SerializeField] private string description;
    private void OnEnable()
    {
        EventsManager.instance.miscEvents.onQuestStepCompleted += QuestStepCompleted;
        EventsManager.instance.miscEvents.ObjectiveUpdate(description);

    }
    private void OnDisable()
    {
        EventsManager.instance.miscEvents.onQuestStepCompleted -= QuestStepCompleted;
    }
   
    private void Update()
    {
        if(Input.GetKeyDown(input))
        {
            QuestStepCompleted();
        }
    }
    private void QuestStepCompleted()
    {
        state = true;
        UpdateState();
        FinishQuestStep();
    }

    private void UpdateState()
    {
        string state = BoolStringConverted.BoolToString(this.state);
        string status = this.status;
        string description = this.description;

        ChangeState(state, status, description);
    }

    protected override void SetQuestStepState(string state)
    {
        UpdateState();
    }
}
