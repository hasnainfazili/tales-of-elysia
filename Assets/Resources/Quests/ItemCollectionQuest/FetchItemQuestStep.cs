using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchItemQuestStep : QuestStep
{
     //TO SAVE BOOLEAN CREATE A METHOD FOR STORING 0 if false 1 if true;
   private GameObject fetchObject;
   private bool IsCollected = false;
   private void OnEnable() {
        EventsManager.instance.miscEvents.onItemCollected += ItemCollected;
   }

   private void OnDisable() {
        EventsManager.instance.miscEvents.onItemCollected -= ItemCollected;
   }
   private void Start()
   {
     UpdateState();
   }
   private void ItemCollected(GameObject itemCollected) {
    IsCollected = true;
    UpdateState();
    FinishQuestStep();
   }

   private void UpdateState() {
     string state = BoolStringConverted.BoolToString(IsCollected);
     string status = "";
     string description = "";
     ChangeState(state, status, description);
   }

    protected override void SetQuestStepState(string state)
    {
        this.IsCollected = BoolStringConverted.StringToBool(state, out bool result);
        UpdateState();
    }
}
