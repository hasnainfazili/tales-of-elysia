using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Anxiety : MonoBehaviour, IDamageable
{
    [SerializeField] private float currentAnxiety = 1f;
    [SerializeField] private float maxAnxiety;
    [SerializeField] private float playerAnxiety;
    [SerializeField] private float anxietyThreshold;
    [SerializeField] private float panicThreshold;

    public bool Blocking;
    bool IsPanicking = false;
   private void OnEnable()
   {
        EventsManager.instance.gameEvents.onAnxietyTriggered += AnxietyTriggered;
        EventsManager.instance.gameEvents.onPanicAttackTriggered += PanicAttack;
        EventsManager.instance.miscEvents.onCampfireActivated += ReduceAnxiety;

        EventsManager.instance.miscEvents.BreathingQTEState += PanicState;
   }

   private void OnDisable()
   {
        EventsManager.instance.gameEvents.onAnxietyTriggered -= AnxietyTriggered;
        EventsManager.instance.miscEvents.onCampfireActivated -= ReduceAnxiety;
        EventsManager.instance.gameEvents.onPanicAttackTriggered -= PanicAttack;

        EventsManager.instance.miscEvents.BreathingQTEState += PanicState;

   } 
  
    private void Update()
    {
        if(Input.GetButton("Fire2")) 
        {
            Blocking = true;
            EventsManager.instance.playerEvents.BlockPress();
        }
        if(Input.GetButtonUp("Fire2")) Blocking =false;
        currentAnxiety = CalculateAnxiety();
        if(currentAnxiety > anxietyThreshold)
        {
            EventsManager.instance.gameEvents.AnxietyTriggered();
        }
        if(currentAnxiety > panicThreshold && !IsPanicking)
        {
            IsPanicking = true;
            EventsManager.instance.gameEvents.PanicAttackTriggered();
            EventsManager.instance.playerEvents.PanicAttack();
        }
    }
    private float CalculateAnxiety()
    {
        return playerAnxiety / maxAnxiety * 100f;
    }
   private void AnxietyTriggered()
   {
        if(currentAnxiety > anxietyThreshold)
        {
            HighAnxiety();
        } 
        else 
        {
            EventsManager.instance.playerEvents.LowAnxietyTriggered();
        }
   }
   private void HighAnxiety()
   {
        EventsManager.instance.playerEvents.HighAnxietyTriggered();
   }

   private void PanicAttack()
   {
        EventsManager.instance.playerEvents.PanicAttack();
   }
    private void PanicState(bool status)
    {
        if(status)
        {
            PanicFinished();
        }
        else
        {
            Death();
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Weapon"))
        {
            if(col.GetComponent<EnemyWeapon>() != null)
            {
            EnemyWeapon hit = col.GetComponent<EnemyWeapon>();
            if(hit.IsAttacking) 
            {
                hit.Hit(GetComponent<AudioSource>(), col.ClosestPoint(transform.position), Blocking);
                TakeDamage(hit.WeaponDamage);
                KnockBack(hit.knockbackStrength, hit.transform);
            }
            }
           
        }
    }
    public void ReduceAnxiety()
    {
        playerAnxiety /= 3f;
        EventsManager.instance.playerEvents.LowAnxietyTriggered();
    }
    public void PanicFinished()
    {
        playerAnxiety -= 300f;
        EventsManager.instance.playerEvents.LowAnxietyTriggered();
    }
    public void TakeDamage(float damageTaken)
    {
       if(!Blocking)
       {
        playerAnxiety += damageTaken / 3f;
       }
        GetComponent<MMF_Player>().PlayFeedbacks();
        GetComponentInChildren<Animator>().SetTrigger("Hit");

   
    }
    public void KnockBack(float knockbackStrength, Transform weaponImpact)
    {
        Vector3 knockBackDirection = (transform.position - weaponImpact.position).normalized;
        GetComponent<Rigidbody>().AddForce(knockBackDirection * knockbackStrength);
    }
    public void Death()
    {
        SceneController.instance.GameOver();
    }

    
}
