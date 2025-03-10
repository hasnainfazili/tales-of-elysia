using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] public Rigidbody[] ragdollRigidbodies;
    [SerializeField] Rigidbody mainRigidbody;
    [SerializeField] CapsuleCollider mainCollider;

    // Start is called before the first frame update
    void Awake()
    {
        SetRagdollRigidbodiesOff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RagdollOn()
    {
        SetRagdollRigidbodiesOn();
    }
    void SetRagdollRigidbodiesOff()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
            rb.GetComponent<Collider>().enabled = false;
        }
        mainCollider.enabled = true;
        mainRigidbody.isKinematic = true;
    }

    void SetRagdollRigidbodiesOn()
    {
        if(CompareTag("Enemy"))
        {
            GetComponent<Enemy>().enabled = false;
            GetComponent<Animator>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Health>().enabled = false;
            GetComponentInChildren<EnemyWeapon>().IsAttacking = false;
        }
        else if(CompareTag("Player"))
        {
            GetComponentInChildren<Animator>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            StartCoroutine(ResetRagDoll());
        }
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.GetComponent<Collider>().enabled = true;
        }

        mainCollider.enabled = true;
        mainRigidbody.isKinematic = true;
    }

    IEnumerator ResetRagDoll()
    {
        yield return new WaitForSecondsRealtime(2f);
        // SetRagdollRigidbodiesOff();
        // GetComponentInChildren<Animator>().enabled = true;
        GetComponent<CharacterController>().enabled = true;
    }
}
