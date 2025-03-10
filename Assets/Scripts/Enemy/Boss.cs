using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour, IDamageable, IEnemy, IFocusable
{
  private Transform player;
  private Animator _animator;
  private NavMeshAgent agent;
  [SerializeField]private float attackRange;
  [SerializeField]private Health health;
  [SerializeField] private EnemyWeapon weapon;
  [SerializeField] private EnemyState currentState;

  [SerializeField] private float timeToAttack = 3.5f;
  private bool IsAttacking = false;
  private bool IsSlowed = false;
  private void OnEnable()
  {
    EventsManager.instance.playerEvents.onHighAnxietyTriggered += PlayerAnxious;
    EventsManager.instance.playerEvents.onLowAnxietyTriggered += PlayerRelaxed;
    EventsManager.instance.gameEvents.onADHDTriggered += PlayerFocus;

    _animator = GetComponent<Animator>();
    agent = GetComponent<NavMeshAgent>();
  }

  private void OnDisable()
  {
    EventsManager.instance.playerEvents.onHighAnxietyTriggered -= PlayerAnxious;
    EventsManager.instance.playerEvents.onLowAnxietyTriggered -= PlayerRelaxed;
    EventsManager.instance.gameEvents.onADHDTriggered -= PlayerFocus;
  }
  private void PlayerAnxious()
    {
        _animator.speed  = .8f;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3 (2f, 2f ,2f),2f);
    }
    private void PlayerRelaxed()
    {
      if(player != null)
      {
        _animator.speed  = 1f;
        if(transform.localScale != new Vector3(1,1,1))
        {
          transform.localScale = Vector3.Lerp(transform.localScale, new Vector3 (1f, 1f ,1f),2f);
        }   
      }
    
    }
    private void PlayerFocus()
    {
        if(IsSlowed)
        {
            _animator.speed = 1f;
        }
        else 
        {
            _animator.speed = .8f;
        }

    }

  private void Update()
  {
    // if(Player == null || IsAttacking) return;
    // float distanceFromPlayer = Vector3.Distance(transform.position, Player.position);
    // _agent.SetDestination(Player.position);
    // if(_agent.speed > 0)
    // {
    //   _animator.SetFloat("Speed", _agent.speed);
    // }
    // if(distanceFromPlayer <= attackRange && !IsAttacking)
    //   Attack();
        if(player != null) 
        {
            if(currentState == EnemyState.Combat) 
            {
                _animator.SetBool("Run", false);
                Combat();
            }
            if(currentState == EnemyState.Chase) 
            {
                _animator.SetBool("Walk", false);
                Chase();
            }
        }
  }

  private void OnTriggerEnter(Collider col)
  {
    if(col.CompareTag("Player"))
    {
      player = col.transform;
      currentState = EnemyState.Chase; 

    }
    if(col.CompareTag("Weapon") && col.GetComponent<Weapon>().IsAttacking && gameObject.CompareTag("Boss"))
    {
      col.GetComponent<Weapon>().Hit(GetComponent<AudioSource>(), col.ClosestPoint(transform.position));
      TakeDamage(col.GetComponent<Weapon>().WeaponDamage);
      GetComponent<MMF_Player>().PlayFeedbacks();
      _animator.SetTrigger("Hit");
    }
  }
  public void TakeDamage(float damage)
  {
    health.TakeDamage(damage);
    if(health.currentHealth <= damage)
    {
      Death();
    }
  }

  public void Death()
  {
    _animator.SetTrigger("Death");
  }
  
  public void UpdateEnemyEncounters() {}

  public float GetPastEncounters()
  {
    return 1;
  }
  public string GetFocusableName(){return name;}
  public Transform GetFocusableTransform(){return transform;}


  Vector3 GetPatrolPosition(Vector3 origin, float distance, int layerMask){
        Vector3 patrolDirection = UnityEngine.Random.insideUnitSphere * distance;
        patrolDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(patrolDirection, out navHit, distance, layerMask);

        return navHit.position;
  }
    void Chase(){
        agent.SetDestination(player.position);
        _animator.SetFloat("Speed", agent.velocity.magnitude);
        float distanceFromPlayer = Vector3.Distance(player.position, transform.position);
        if(distanceFromPlayer <= attackRange) {
            currentState = EnemyState.Combat;
        }
    }

    void Combat()
    {
        float distanceFromPlayer = Vector3.Distance(player.position, transform.position);
        _animator.SetFloat("Speed", agent.velocity.magnitude);
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
      _animator.SetTrigger("Attack");
      _animator.SetInteger("Attack Number", Random.Range(0,3));
      StartCoroutine(AttackDelay());
    }

  IEnumerator AttackDelay()
  {
    yield return new WaitForSeconds(timeToAttack);
    IsAttacking = false;
  }
}
