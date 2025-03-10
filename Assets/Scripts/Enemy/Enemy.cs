using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Feedbacks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {
    Idle,
    Patrol,
    Chase,
    Combat,
    Damage,
    Dead
}
public class Enemy : MonoBehaviour, IEnemy, IDamageable, IFocusable
{
    private Animator anim;
    private NavMeshAgent agent;
    private Health health;
    public Transform player;

    [SerializeField] private string Name;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float attackRange;
    [SerializeField] private float patrolRadius;
    [SerializeField] private float patrolDelay;
    [SerializeField] private float timeToAttack;
    [SerializeField] private bool IsAttacking;
    float patrolTimer;
    
    [SerializeField] private float PastEncounters;
    
    [SerializeField] private EnemyState currentState;
    
    
    private bool IsSlowed = false;

    void OnEnable()
    {
        EventsManager.instance.playerEvents.onHighAnxietyTriggered += PlayerAnxious;
        EventsManager.instance.playerEvents.onLowAnxietyTriggered += PlayerRelaxed;
        EventsManager.instance.gameEvents.onADHDTriggered += PlayerFocus;

        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }
    private void OnDisable()
    {
        EventsManager.instance.playerEvents.onHighAnxietyTriggered -= PlayerAnxious;
        EventsManager.instance.playerEvents.onLowAnxietyTriggered -= PlayerRelaxed;
        EventsManager.instance.gameEvents.onADHDTriggered -= PlayerFocus;
    }
    private void Start() 
    {
        agent.speed = moveSpeed;
        if(player != null) currentState = EnemyState.Chase;
    }

    private void Update() 
    {
        if(currentState.Equals(EnemyState.Dead)) return;
        if(currentState.Equals(EnemyState.Patrol))
        {
            Patrol();
        }
        if(player != null) 
        {
            
            if(currentState == EnemyState.Combat) 
            {
                anim.SetBool("Run", false);
                Combat();
            }
            if(currentState == EnemyState.Chase) 
            {
                anim.SetBool("Walk", false);
                Chase();
            }
        }
    }
    private void FixedUpdate()
    {
        if(currentState.Equals(EnemyState.Dead))
        {
            Destroy(gameObject, 3f);
        }
        if(!agent.isOnNavMesh) Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider) 
    {
        if(collider != null)
        {
            Collider col = collider;
            if(col.CompareTag("Weapon")) 
            {
                if(col.GetComponent<Weapon>() != null)
                {
                    Weapon weapon = col.GetComponent<Weapon>();
                    if(weapon.IsAttacking)
                    {
                        weapon.Hit(GetComponent<AudioSource>(), col.ClosestPoint(transform.position));
                        TakeDamage(weapon.WeaponDamage);
                        DeathForce(weapon.transform);
                    }
                }
            }
            if(collider.CompareTag("Player"))
            {
                player = collider.transform;        
                currentState = EnemyState.Chase; 
                UpdateEnemyEncounters();
            }
        }
    }
    private void PlayerAnxious()
    {
        if(player != null)
        {
            anim.speed  = 1.2f;
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(2f,2f,2f),2f);
        }
        
    }
    private void PlayerRelaxed()
    {
        if(player != null)
        {
            anim.speed  = 1f;
            if(transform.localScale != new Vector3(1,1,1))
            {
                transform.localScale -= new Vector3(1,1,1);
            }   
        }
    }

    private void PlayerFocus()
    {
        if(player != null)
        {
            if(IsSlowed)
            {
                anim.speed = 1f;
            }   
            else 
            {
                anim.speed = .75f;
            }
        }
    }
    public string GetFocusableName()
    {
        return Name;
    }
   
    public Transform GetFocusableTransform()
    {
        return transform;
    }

    public void TakeDamage(float damageTaken)
    {
        health.TakeDamage(damageTaken);
        GetComponent<MMF_Player>().PlayFeedbacks();
        if(health.currentHealth <= damageTaken)
        {
            Death();
            currentState = EnemyState.Dead;

        }
        anim.SetTrigger("Take Damage");
    }
    public void Death(){
        anim.SetBool("Die", true);
    }   
    void Dead()
    {
        GetComponent<Ragdoll>().RagdollOn();
        EventsManager.instance.miscEvents.EnemyKilled();

    }
    void DeathForce(Transform weaponTransform)
    {
        Rigidbody[] rbs = GetComponent<Ragdoll>().ragdollRigidbodies;
        foreach(var rb in rbs)
        {
            rb.AddRelativeForce(weaponTransform.forward * 10000f);
        }

    }
      public float GetPastEncounters()
    {
        return PastEncounters;
    }
    public void UpdateEnemyEncounters()
    {
        PastEncounters++;
    }

    void Patrol(){
        anim.SetBool("Walk", true);
        if(!agent.pathPending && agent.remainingDistance < 0.5f){
            patrolTimer += Time.deltaTime;
            if(patrolTimer >= patrolDelay){
                agent.SetDestination(GetPatrolPosition(transform.position, patrolRadius, -1));
                patrolTimer = 0f;
            }
        }
    }

    Vector3 GetPatrolPosition(Vector3 origin, float distance, int layerMask){
        Vector3 patrolDirection = UnityEngine.Random.insideUnitSphere * distance;
        patrolDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(patrolDirection, out navHit, distance, layerMask);

        return navHit.position;
    }

    void Chase(){
        agent.SetDestination(player.position);
        anim.SetBool("Run", true);
        float distanceFromPlayer = Vector3.Distance(player.position, transform.position);
        if(distanceFromPlayer <= attackRange) {
            currentState = EnemyState.Combat;
        }
    }

   void Combat()
    {
        float distanceFromPlayer = Vector3.Distance(player.position, transform.position);
        if(distanceFromPlayer > attackRange)
        {
            currentState = EnemyState.Chase;
        }
        else 
        {
            if(!IsAttacking)
            {
              Attack();
            }
        }
    }

    private void Attack()
    {
      IsAttacking = true;
      Vector3 lookAtDirection = new Vector3(player.position.x, 0, player.position.z);
      transform.LookAt(Vector3.Lerp(transform.position, lookAtDirection, Time.deltaTime * agent.angularSpeed));
      anim.SetTrigger("Attack");
      anim.SetInteger("Attack Number", UnityEngine.Random.Range(0,3));
      StartCoroutine(AttackDelay());
    }

  IEnumerator AttackDelay()
  {
    yield return new WaitForSeconds(timeToAttack);
    IsAttacking = false;
  }
}
