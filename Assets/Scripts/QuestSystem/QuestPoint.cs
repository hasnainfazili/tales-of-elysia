using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestPoint : MonoBehaviour, IInteractable, IFocusable
{ 
    [SerializeField] private AudioSource source;
    [SerializeField] private string questText;

    [SerializeField] private QuestInfoSO questInfoForPoint;
    private string questId;
    private QuestState currentQuestState;
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;
    private void Awake() {
        questId = questInfoForPoint.id;
    }
    private void OnEnable() {
        EventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }
    private void OnDisable() {
        EventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    public void Interact(Transform interactorTransform) 
    {
        if(currentQuestState.Equals(QuestState.CAN_START) && startPoint) 
            {
                EventsManager.instance.questEvents.StartQuest(questId);
            } 
        if(currentQuestState.Equals(QuestState.CAN_FINISH)  && finishPoint)
            {
                EventsManager.instance.questEvents.FinishQuest(questId);
                if(this.CompareTag("Door"))
                {
                    SceneController.instance.Lysandria();
                }
            }
        
    }
    private void QuestStateChange(Quests quest){
        if(quest.info.id.Equals(questId)) {
            currentQuestState = quest.state;
        }
    }
    public string GetInteractableName() {return questText;}
    public Transform GetInteractableTransform() {return transform;}
    public string GetFocusableName(){return name;}
    public Transform GetFocusableTransform(){return transform;}
    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            source.Play();
        }

        
    }
   
 }
