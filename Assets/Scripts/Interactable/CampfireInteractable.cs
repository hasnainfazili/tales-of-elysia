using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireInteractable : MonoBehaviour, IInteractable, IFocusable
{
    [SerializeField] private string interactText;
    [SerializeField] private AudioClip interactAudio;

    public void Interact(Transform interactorTransform)
    {
        EventsManager.instance.miscEvents.CampfireActivated();
        GetComponent<AudioSource>().PlayOneShot(interactAudio);
    }


    public string GetInteractableName(){return interactText;}
    public Transform GetInteractableTransform(){return transform;}
    public string GetFocusableName(){return name;}
    public Transform GetFocusableTransform(){return transform;}
}
