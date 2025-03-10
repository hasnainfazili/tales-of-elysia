using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemQuestStep : QuestStep
{
    [SerializeField] private GameObject gameObjectToCollect;
    private bool state = false;
    [SerializeField] private string description;
    private void OnEnable() 
    {
        EventsManager.instance.miscEvents.onQuestStepCompleted += QuestStepCompleted;
        EventsManager.instance.miscEvents.onItemCollected += ItemCollected;
        EventsManager.instance.miscEvents.ObjectiveUpdate(description);
        EventsManager.instance.questEvents.QuestStepCreated(transform);

        gameObjectToCollect.GetComponent<CollectableItem>().enabled = true;
    }

    private void OnDisable()
    {
        EventsManager.instance.miscEvents.onQuestStepCompleted -= QuestStepCompleted;
        EventsManager.instance.miscEvents.onItemCollected -= ItemCollected;
    }
    
    private void QuestStepCompleted()
    {
        state = true;
        UpdateState();
        FinishQuestStep();

    }
    private void ItemCollected(GameObject itemCollected)
    {
            itemCollected.SetActive(false);
            UpdateState();
            FinishQuestStep();
    }
    private void UpdateState()
    {
        string state = BoolStringConverted.BoolToString(this.state);

        ChangeState(state, "", "");
    }

    protected override void SetQuestStepState(string state)
    {
        UpdateState();
    }
}
