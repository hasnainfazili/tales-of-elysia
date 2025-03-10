using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestsManager : MonoBehaviour
{
    public static QuestsManager instance {get; private set;}
    [SerializeField] private bool loadQuestState = true;
    private Dictionary<string, Quests> questMap;
    private void Awake() {
        questMap = CreateQuestMap();
        instance = this;
    }
    private void OnEnable() {
        EventsManager.instance.questEvents.onStartQuest += StartQuest;
        EventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        EventsManager.instance.questEvents.onFinishQuest += FinishQuest;
        // EventsManager.instance.questEvents.onQuestForgotten += ForgetQuest;
        EventsManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;
    }
    private void OnDisable() {
        EventsManager.instance.questEvents.onStartQuest -= StartQuest;
        EventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        EventsManager.instance.questEvents.onFinishQuest -= FinishQuest;
        // EventsManager.instance.questEvents.onQuestForgotten -= ForgetQuest;


        EventsManager.instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;

    }
    private void Start() {
        foreach(Quests quest in questMap.Values) 
        {
            if(quest.state == QuestState.IN_PROGRESS) 
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            EventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }
    private void StartQuest(string id)
    {
        Quests quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }
    private void AdvanceQuest(string id)
    {
        Quests quest = GetQuestById(id);
        quest.MoveToNextStep();
        if(quest.CurrentStepExits())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        else
        {
            EventsManager.instance.miscEvents.ObjectiveUpdate(quest.info.description);
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }
    private void ForgetQuest()
    {
        Quests quest = GetQuestById(questMap.Values.Last().info.id);
        quest.MoveToPreviousStep();
        ChangeQuestState(quest.info.id, QuestState.FORGOTTEN);
    }
    private void FinishQuest(string id) {
        Quests quest = GetQuestById(id);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }

    private void ChangeQuestState(string id, QuestState state) 
    {
        Quests quest = GetQuestById(id);
        quest.state = state;
        EventsManager.instance.questEvents.QuestStateChange(quest);
    }
    private Dictionary<string, Quests> CreateQuestMap() {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quests> idToQuestMap = new Dictionary<string, Quests>();
        foreach(QuestInfoSO questInfo in allQuests) {
            if(idToQuestMap.ContainsKey(questInfo.id)) {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id );
            }
            idToQuestMap.Add(questInfo.id, LoadQuest(questInfo));
        }
        return idToQuestMap;
    }
    
    private Quests GetQuestById(string id) {
        Quests quest = questMap[id];
        if(quest == null) {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return quest;
    }

    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        Quests quest = GetQuestById(id);
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.state);
    }

    private void OnApplicationQuit() 
    {
        foreach(Quests quest in questMap.Values) 
        {
           SaveQuest(quest);
        }
    }

    private void SaveQuest(Quests quest)
    {
        try {
            QuestData questData = quest.GetQuestData();
            string serializedData = JsonUtility.ToJson(questData);
            PlayerPrefs.SetString(quest.info.id, serializedData);
        } catch (System.Exception e) 
        {
            Debug.LogError("Failed to save quest with id " + quest.info.id + " : " + e);
        }
    }

    private Quests LoadQuest(QuestInfoSO questInfo) 
    {
        Quests quest = null;
        try {
            if(PlayerPrefs.HasKey(questInfo.id) && loadQuestState) {
                string serializedData = PlayerPrefs.GetString(questInfo.id);
                QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                quest = new Quests(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
            } else {
                quest = new Quests(questInfo);
            }
        } catch(System.Exception e) {
            Debug.LogError("Failed to load quest with id " + quest.info.id + ": " + e);
        }
        return quest;
    }
}
