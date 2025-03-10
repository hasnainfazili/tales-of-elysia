using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro.Examples;
using MoreMountains.Feedbacks;

public class ADHD : MonoBehaviour
{
    [Header("Inattentiveness")]
    int rolledValue;
    DiceRoll diceRoll;
    private int diceSides = 20;
    public bool hyper {get; private set;}
    [SerializeField] private MMF_Player feedbackPlayer;
    [SerializeField] private float hyperDuration;

    void Awake()
    {
        hyper = false;
        diceRoll = new DiceRoll();
        diceRoll.AddDice(diceSides);
        InvokeRepeating("Roll",5f, 50f);
    }

    void Update()
    {
        if(rolledValue > 15 && !hyper)
        {
            Hyperfocus();
        } 
        else if(rolledValue < 5)
        {
            Inattentiveness();
        }
    }
    
    void Inattentiveness()
    {
        EventsManager.instance.gameEvents.InattentivenessTriggered();
        Forgetfulness();
    }
    
    void Hyperfocus()
    {
        hyper = true;
        EventsManager.instance.gameEvents.ADHDTriggered();

        if(!feedbackPlayer.IsPlaying)
        {
            feedbackPlayer.PlayFeedbacks();
        }
        StartCoroutine(Hyper());
    }
    IEnumerator Hyper()
    {
        yield return new WaitForSeconds(hyperDuration);
        hyper = false;
    }
    void Forgetfulness()
    {
        EventsManager.instance.questEvents.QuestForgotten();
    }
    public List<GameObject> GetFocusableObject()
    {
        List<GameObject> focusableList = new List<GameObject>();
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, 3f);
        foreach(Collider collider in colliderArray)
        {
            if(collider.TryGetComponent(out IFocusable focusable))
            {
                focusableList.Add(focusable.GetFocusableTransform().gameObject);
            }
        }
        return focusableList;
    }
    private void Roll(){
        diceRoll.Roll();
        rolledValue = diceRoll.RollValue(); 
        
    }
}
