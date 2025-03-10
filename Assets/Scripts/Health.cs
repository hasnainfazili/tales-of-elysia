using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] private float maxHealth;
    [SerializeField] public float currentHealth {get; private set;}

    void Start(){
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageTaken) {
        currentHealth -= damageTaken;
    }


    public void Death() {
    }
    public void SetHealth(float multiply)
    {
        maxHealth *= multiply;
    }
}