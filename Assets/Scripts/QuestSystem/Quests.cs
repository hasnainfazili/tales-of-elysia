using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quests
{
    public QuestInfoSO info;
    public QuestState state;
    private int currentQuestStepIndex;
    private QuestStepState[] questStepStates;
    public Quests(QuestInfoSO questInfo) {
        this.info = questInfo;
        this.state = QuestState.CAN_START;
        this.currentQuestStepIndex = 0;
        this.questStepStates = new QuestStepState[info.questStepPrefabs.Length];
        for(int i = 0; i < questStepStates.Length; i++) 
        {
            questStepStates[i] = new QuestStepState();
        }
    }
    public Quests(QuestInfoSO questInfo,  QuestState questState, int currentQuestStepIndex, QuestStepState[] questStepStates)
    {
        this.info = questInfo;
        this.state = questState;
        this.currentQuestStepIndex = currentQuestStepIndex;
        this.questStepStates = questStepStates;

        if(this.questStepStates.Length != this.info.questStepPrefabs.Length)
        {
            Debug.LogWarning("Quest Step Prefabs and quest step states are of different lengths."+ 
            " This indicates something changed with the quest info and the saved data is out of sync");
        }
    }
    public void MoveToNextStep() {
        currentQuestStepIndex++;
    }
    public void MoveToPreviousStep()
    {
        currentQuestStepIndex--;
    }
    public bool CurrentStepExits() {
        return (currentQuestStepIndex < info.questStepPrefabs.Length);
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform) {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if(questStepPrefab != null) {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform)
                .GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.id, currentQuestStepIndex,questStepStates[currentQuestStepIndex].state, questStepStates[currentQuestStepIndex].description);
        }
    }

    private GameObject GetCurrentQuestStepPrefab(){
        GameObject questStepPrefab = null;
        if(CurrentStepExits()) {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        } else {
            Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range");
        }

        return questStepPrefab;
    }

    public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
    {
        if(stepIndex < questStepStates.Length)
        {
            questStepStates[stepIndex].state = questStepState.state;
            questStepStates[stepIndex].status = questStepState.status;
        } else {
            Debug.LogWarning("Tried to access quest step data, but stepIndex was out of range: " + " Quest Id = " + info.id + " , Step Index = " + stepIndex);
        }
    }

    public QuestData GetQuestData() 
    {
        return new QuestData(state, currentQuestStepIndex, questStepStates);
    }

    public string GetFullStatus() 
    {
        string fullStatus = "";
        if(state == QuestState.CAN_START)
        {
            fullStatus = "Quest can be started!";
        } else {
            for(int i = 0; i < currentQuestStepIndex; i++)
            {
                fullStatus += "<s>" + questStepStates[i].status + "</s>\n";
            }
            if(CurrentStepExits()) {
                fullStatus += questStepStates[currentQuestStepIndex].status;
            }
            if(state == QuestState.CAN_FINISH) {
                fullStatus = "Can turn in quest";
            } else if(state == QuestState.FINISHED){
                fullStatus = "Quest is completed!";
            }
            
        }

        return fullStatus;
    }

    public string GetQuestDescription() 
    {
        string fullDescription = "";
        if(state == QuestState.CAN_START)
        {
            fullDescription = "Quest can be started! + \n" + questStepStates[currentQuestStepIndex].description;
        } else {
            if(CurrentStepExits()) {
                fullDescription += questStepStates[currentQuestStepIndex].description + " \n";
            }
        }

        return questStepStates[currentQuestStepIndex].description;
    }
}
