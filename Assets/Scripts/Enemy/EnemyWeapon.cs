using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum WeaponType
{
    Boss,
    Normal
}
public class EnemyWeapon : MonoBehaviour
{
    public float WeaponDamage;
    public bool IsAttacking;
    public float attackDelay;
    public float knockbackStrength;
    public WeaponType type;

    [SerializeField] public AudioClip[] hitSoundFXClips;
    [SerializeField] public GameObject[] hitImpactVFX;
    [SerializeField] public GameObject[] blockImpactVFX;
    [SerializeField] public AudioClip[] blockSoundFXClips;
    [SerializeField] public GameObject bloodParticle;

   public void Hit(AudioSource audioSource, Vector3 hitPoint, bool blocking)
   {
        if(blocking)
        {
          
          SoundFXManager.instance.PlaySoundFXClip(blockSoundFXClips, hitPoint, .3f);
          Instantiate(blockImpactVFX[UnityEngine.Random.Range(0, blockImpactVFX.Length-1)],hitPoint, quaternion.identity);

        }
        else 
        {
          // if(type == WeaponType.Boss)
          // {
          //   audioSource.GetComponent<Ragdoll>().RagdollOn();
          // }
          SoundFXManager.instance.PlaySoundFXClip(hitSoundFXClips, hitPoint, .3f);
          Instantiate(hitImpactVFX[UnityEngine.Random.Range(0, hitImpactVFX.Length-1)],hitPoint, quaternion.identity);
          Instantiate(bloodParticle,hitPoint, quaternion.identity);
        }        
        
   }
}

