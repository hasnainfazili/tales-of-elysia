using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InteractableNPC : MonoBehaviour, IInteractable
{
    
    public void Interact(Transform interactorTransform)
    {
       transform.LookAt(interactorTransform);
    }


    public string GetInteractableName (){
        return name;
    }



    public Transform GetInteractableTransform(){
        return transform;
    }
   

}


