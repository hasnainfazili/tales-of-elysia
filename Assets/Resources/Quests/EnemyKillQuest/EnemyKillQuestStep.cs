using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyKillQuestStep : QuestStep
{
    public int enemyKillCount = 5;
    private int currentKillCount;
    private string status;
    private string state;
    [SerializeField] private string description;
    void OnEnable()
    {
        EventsManager.instance.miscEvents.onQuestStepCompleted += QuestStepCompleted;
        EventsManager.instance.miscEvents.onEnemyKilled += UpdateKillCount;
        EventsManager.instance.miscEvents.ObjectiveUpdate(description + " " + currentKillCount.ToString() + "/" + enemyKillCount.ToString());
    }
    void OnDisable()
    {
        EventsManager.instance.miscEvents.onQuestStepCompleted -= QuestStepCompleted;
    }
    // Start is called before the first frame update
    private void Update()
    {
        if(currentKillCount >= enemyKillCount)
        QuestStepCompleted();
    }
    void UpdateKillCount()
    {
        currentKillCount++;
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
