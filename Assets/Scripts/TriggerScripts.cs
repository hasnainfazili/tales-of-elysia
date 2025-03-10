using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerScripts : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            director.Play();
        }  
    }
}
