using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscEvents 
{
   public event Action<GameObject> onItemCollected;
   public void ItemCollected(GameObject gameObject) {
    if(onItemCollected != null) 
        onItemCollected(gameObject);
   }

   public event Action onQuestLogToggled;
   public void QuestLogToggled(){
      if(onQuestLogToggled != null) {
         onQuestLogToggled();
      }
   }

   public event Action onQuestStepCompleted;
   public void QuestStepCompleted() 
   {
      if(onQuestStepCompleted != null) 
      {
         onQuestStepCompleted();
      }
   }
   public event Action onCampfireActivated;
   public void CampfireActivated()
   {
      if(onCampfireActivated != null)
      {
         onCampfireActivated();
      }
   }

   public event Action<string> onObjectiveUpdate;

   public void ObjectiveUpdate(string objectiveText)
   {
      if(onObjectiveUpdate != null)
      {
         onObjectiveUpdate(objectiveText);
      }
   }
   public event Action onEnemyKilled;

   public void EnemyKilled()
   {
      if(onEnemyKilled != null)
      {
         onEnemyKilled();
      }
   }

   public event Action<bool> BreathingQTEState;
   public void BreathingQTE(bool status)
   {
      if(BreathingQTEState != null)
      {
         BreathingQTEState(status);
      }
   }
}
