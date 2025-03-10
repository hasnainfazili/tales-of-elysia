using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class PlayerInteractionUI : MonoBehaviour
{
    [Header("Interact")]
    [SerializeField] InteractionScript interactScript;
    [SerializeField] GameObject containerGameObject;
    [SerializeField] GameObject questStateContainerGameObject;
    [SerializeField] MMF_Player uiFeedback;
    [SerializeField] TextMeshProUGUI questStateTextMeshProUGUI;
    [SerializeField] TextMeshProUGUI interactTextMeshProUGUI;

    private void OnEnable()
    {
        EventsManager.instance.questEvents.onStartQuest += QuestStart;
        EventsManager.instance.questEvents.onFinishQuest += QuestFinish;
        EventsManager.instance.questEvents.onAdvanceQuest += QuestStateUpdate;
    }

    private void OnDisable()
    {
        EventsManager.instance.questEvents.onStartQuest -= QuestStart;
        EventsManager.instance.questEvents.onFinishQuest -= QuestFinish;
        EventsManager.instance.questEvents.onAdvanceQuest -= QuestStateUpdate;

    }
    private void Update(){
        if(interactScript.GetInteractableObject() != null) {
            Show(interactScript.GetInteractableObject());
        } else {
            Hide();
        }
    }
    private void Show(IInteractable interactable){
        containerGameObject.SetActive(true);
        interactTextMeshProUGUI.text = interactable.GetInteractableName();
    }   
    private void Hide(){
        containerGameObject.SetActive(false);
    }
    private void QuestStateUpdate(string id)
    {
        questStateContainerGameObject.SetActive(true);
        uiFeedback.PlayFeedbacks();

        questStateTextMeshProUGUI.text = "Quest: " + id + "Step Completed" ;
        StartCoroutine(TurnOff());
    }
    private void QuestStart(string id)
    {
        questStateContainerGameObject.SetActive(true);
        uiFeedback.PlayFeedbacks();

        questStateTextMeshProUGUI.text = "Quest: " + id + "Step Completed" ;
        questStateTextMeshProUGUI.text = "Quest Started: " + id;

        StartCoroutine(TurnOff());

    }
    private void QuestFinish(string id)
    {
        questStateContainerGameObject.SetActive(true);
        uiFeedback.PlayFeedbacks();
        questStateTextMeshProUGUI.text = "Quest Completed: " + id;
        StartCoroutine(TurnOff());

    }

    IEnumerator TurnOff()
    {
        yield return new WaitForSecondsRealtime(2f);
        questStateContainerGameObject.SetActive(false);

    }
}
