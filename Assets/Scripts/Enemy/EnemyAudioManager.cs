using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] private AudioClip[] damageSounds;

    [SerializeField] private AudioClip[] DeathSounds;

    [SerializeField] private AudioSource footStepAudioSource;
    [SerializeField] private AudioSource enemyAudioSource;
    [SerializeField] private AudioSource weaponAudioSource;
    [SerializeField] private MMF_Player enemyFeedbacks;

    void Hit()
    {
        enemyAudioSource.PlayOneShot(damageSounds[Random.Range(0, damageSounds.Length - 1)]);
    }
    void Death()
    {
        enemyAudioSource.PlayOneShot(DeathSounds[Random.Range(0, DeathSounds.Length - 1)]);
    }
    
    void Footsteps()
    {
        footStepAudioSource.Play();
    }
    void SwordSwing()
    {
        weaponAudioSource.Play();
        // enemyFeedbacks.PlayFeedbacks();
    }
    public void Attacking()
    {
        enemyAudioSource.PlayOneShot(attackSounds[Random.Range(0, attackSounds.Length - 1)]);
        weaponAudioSource.GetComponent<EnemyWeapon>().IsAttacking = true;
        
    }

    public void ResetAttack()
    {
        weaponAudioSource.GetComponent<EnemyWeapon>().IsAttacking = false;
    }
    void ImpactGround()
    {
        //Hitting Ground Feedback +
        enemyFeedbacks.PlayFeedbacks();
    }
}   
