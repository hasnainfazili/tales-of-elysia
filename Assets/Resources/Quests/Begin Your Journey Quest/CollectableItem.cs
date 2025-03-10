using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour,IInteractable
{
    void OnEnable()
    {
    }
    public void Interact(Transform interactorTransform)
    {
        EventsManager.instance.miscEvents.ItemCollected(gameObject);
        EventsManager.instance.miscEvents.QuestStepCompleted();
    }
    public string GetInteractableName(){return name;}
    public Transform GetInteractableTransform(){return transform;}
}
