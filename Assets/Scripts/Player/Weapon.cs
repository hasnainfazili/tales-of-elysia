using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float WeaponDamage;
    public bool IsAttacking;
    public float attackDelay = 2f;
    public bool IsBuffed = false;
    public float knockbackStrength;
    private void OnEnable()
    {
        EventsManager.instance.gameEvents.onADHDTriggered += WeaponBuff;
    }

    private void OnDisable()
    {
        EventsManager.instance.gameEvents.onADHDTriggered -= WeaponBuff;

    }

    [SerializeField] public AudioClip[] hitSoundFXClips;
    [SerializeField] public GameObject[] hitImpactVFX ;
    [SerializeField] public GameObject bloodParticle;
    public void Hit(AudioSource audioSource, Vector3 hitPoint)
    {
        // audioSource.PlayOneShot(hitSoundFXClips[UnityEngine.Random.Range(0, hitSoundFXClips.Length-1)]);
        SoundFXManager.instance.PlaySoundFXClip(hitSoundFXClips, hitPoint, .4f);
        Instantiate(hitImpactVFX[UnityEngine.Random.Range(0, hitImpactVFX.Length-1)],hitPoint, quaternion.identity);
        Instantiate(bloodParticle,hitPoint, quaternion.identity);
    }   
    private void WeaponBuff()
    {
        if(!IsBuffed)
        {
            IsBuffed = true;
            attackDelay = .5f;
            WeaponDamage = 30f;
        }
    }
    
}