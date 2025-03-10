using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(Transform interactorTransform);
    public string GetInteractableName();
    public Transform GetInteractableTransform();
}
