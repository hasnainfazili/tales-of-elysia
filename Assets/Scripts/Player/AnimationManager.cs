using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]private AudioClip DrawSwordAudioClip;
    [SerializeField]private AudioClip SheathSwordAudioClip;

    
    [SerializeField] private Animator _animator;
    [SerializeField]private MMF_Player playerFeedback;
    [SerializeField]private MMF_Player anxiousFeedback;
    [SerializeField]private MMF_Player panicFeedback;
    public GameObject weapon;
    public GameObject shield;
    [SerializeField]private PlayerController player;
    private bool AnxietyTriggered = false;
    private bool IsSitting = false;
    private bool Blocking = false;
    private void OnEnable()
    {
        EventsManager.instance.gameEvents.onADHDTriggered += ADHDTriggered;
        EventsManager.instance.playerEvents.onHighAnxietyTriggered += HighAnxietyTriggered;
        EventsManager.instance.playerEvents.onLowAnxietyTriggered += LowAnxietyTriggered;
        EventsManager.instance.playerEvents.onAttackPressed += Attack;
        EventsManager.instance.playerEvents.onBlockPress += Block;
        EventsManager.instance.playerEvents.onPanicAttack += PanicAttack;
        EventsManager.instance.gameEvents.onPanicAttackTriggered += PanicAttack;
        EventsManager.instance.playerEvents.onWeaponDrawn += WeaponDrawn;
        EventsManager.instance.playerEvents.onWeaponSheathed += WeaponSheathed;

        EventsManager.instance.miscEvents.onCampfireActivated += CampfireSitting;
        EventsManager.instance.miscEvents.BreathingQTEState += TurnOff;

    }

    private void onDisable()
    {
        EventsManager.instance.gameEvents.onADHDTriggered -= ADHDTriggered;
        EventsManager.instance.playerEvents.onHighAnxietyTriggered -= HighAnxietyTriggered;
        EventsManager.instance.playerEvents.onLowAnxietyTriggered -= LowAnxietyTriggered;
        EventsManager.instance.playerEvents.onAttackPressed -= Attack;
        EventsManager.instance.playerEvents.onPanicAttack -= PanicAttack;
        EventsManager.instance.gameEvents.onPanicAttackTriggered -= PanicAttack;
        EventsManager.instance.playerEvents.onBlockPress -= Block;

        EventsManager.instance.miscEvents.onCampfireActivated -= CampfireSitting;

        EventsManager.instance.playerEvents.onWeaponDrawn -= WeaponDrawn;
        EventsManager.instance.playerEvents.onWeaponSheathed -= WeaponSheathed;

        EventsManager.instance.miscEvents.BreathingQTEState -= TurnOff;

    }
    private void ADHDTriggered()
    {
        _animator.SetTrigger("ADHD");
    }
    private void HighAnxietyTriggered()
    {
        AnxietyTriggered = true;
        _animator.SetTrigger("High Anxiety");
        anxiousFeedback.PlayFeedbacks();
    }
    private void LowAnxietyTriggered()
    {
        AnxietyTriggered = false;
        anxiousFeedback.StopFeedbacks();
        
    }
    private void Attack()
    {
        if(AnxietyTriggered)
        {
            _animator.SetTrigger("Lash");
        } 
        else 
        {
            _animator.SetTrigger("Attack");
        }
    }

    private void CampfireSitting()
    {
        if(IsSitting)
        {
            IsSitting = false;
            _animator.SetBool("Campfire", IsSitting);

        }
        else if(!IsSitting)
        {
            IsSitting = true;
            _animator.SetBool("Campfire", IsSitting);

        }

    }

    void WeaponDrawn()
    {
        _animator.SetTrigger("Draw Sword");
    }
    void WeaponSheathed()
    {
        _animator.SetTrigger("Sheath Sword");
    }
    private void DrawWeapon()
    {
        SoundFXManager.instance.PlaySingleSoundFXClip(DrawSwordAudioClip, weapon.transform.position, 1f);
        shield.SetActive(true);
        weapon.SetActive(true);
    }

    private void SheathWeapon()
    {
        SoundFXManager.instance.PlaySingleSoundFXClip(SheathSwordAudioClip, weapon.transform.position, 1f);
        weapon.SetActive(false);
        shield.SetActive(false);

    }
    private void PanicAttack()
    {
        panicFeedback.PlayFeedbacks();
        _animator.SetTrigger("Panic");
        //Stop Panic When Breathing QTE Complete
    }

    private void TurnOff(bool status)
    {
        if(status) 
        {
            panicFeedback.ResetFeedbacks();
            panicFeedback.StopFeedbacks();
            _animator.ResetTrigger("Panic");
            AnxietyTriggered = false;
            _animator.SetTrigger("Low Anxiety");
            anxiousFeedback.StopFeedbacks();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(AnxietyTriggered)
        {
            _animator.SetTrigger("High Anxiety");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(BreathingQTE.instance.isActive || DialogueManager.GetInstance().dialogueIsPlaying || GameManager.instance.Paused)
        {
            return;
        }
        if(!AnxietyTriggered )
        {
            _animator.SetTrigger("Low Anxiety");
        }
        _animator.SetFloat("Speed", player.speed);
        Blocking = GetComponentInParent<Anxiety>().Blocking;
        // if(Blocking)
        // {
        //     shield.SetActive(true);
        // }
        // else 
        // {
        //     shield.SetActive(false);
        // }
        _animator.SetBool("Blocking", Blocking);
 
    }

    void AttackShake()
    {
        playerFeedback.PlayFeedbacks();
    }

    void Slash()
    {
        weapon.SetActive(true);

        shield.SetActive(true);
    }

    public void Block()
    {
        if(!Blocking)
        {
            _animator.SetTrigger("Block");
            shield.SetActive(true);
            weapon.GetComponent<Weapon>().IsAttacking = false;
        }
    }
    public  void AttackSet()
    {
        weapon.GetComponent<Weapon>().IsAttacking = true;

    }
    
    public void ResetAttack()
    {
        weapon.GetComponent<Weapon>().IsAttacking = false;
    }
    
}
