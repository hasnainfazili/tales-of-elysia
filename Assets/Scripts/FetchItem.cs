using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchItem : MonoBehaviour, IInteractable
{
    Collider triggerCollider;
    void Start() {
        triggerCollider = GetComponent<Collider>();
    }
    private void ItemCollected() {
        triggerCollider.enabled = false;
        gameObject.SetActive(false);
        EventsManager.instance.miscEvents.ItemCollected(gameObject);
    }

    public void Interact(Transform interactorTransform) {
        ItemCollected();
    }

    public string GetInteractableName() { return name;}
    public Transform GetInteractableTransform(){return transform;}
}
