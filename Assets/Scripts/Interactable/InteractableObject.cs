using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
   [SerializeField] string Name;
   public string GetInteractableName (){
    return Name;
   }
   public Transform GetInteractableTransform(){
    return transform;
   }
   public void Interact(Transform interactorTransform){
      
   }
}
