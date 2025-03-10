using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents
{
    public event Action onAttackPressed;
    public void AttackPressed()
    {
        if(onAttackPressed != null)
        {
            onAttackPressed();
        }
    }
    public event Action onBlockPress;
    public void BlockPress()
    {
        if(onBlockPress != null)
        {
            onBlockPress();
        }
    }
    public event Action onHighAnxietyTriggered;
    public void HighAnxietyTriggered()
    {
        if(onHighAnxietyTriggered != null)
        {
            onHighAnxietyTriggered();
        }
    }
    public event Action onLowAnxietyTriggered;
    public void LowAnxietyTriggered()
    {
        if(onLowAnxietyTriggered != null)
        {
            onLowAnxietyTriggered();
        }
    }
    
    public event Action onPanicAttack;
    public void PanicAttack()
    {
        if(onPanicAttack != null)
        {
            onPanicAttack();
        }
    }

    public event Action<Transform> onNearestCampfire;
    public void NearestCampfire(Transform campfire)
    {
        if(onNearestCampfire != null)
        {
            onNearestCampfire(campfire);
        }
    }
    public event Action onWeaponDrawn;
    public void WeaponDrawn()
    {
        if(onWeaponDrawn != null) 
        {
            onWeaponDrawn();
        }
    }
    public event Action onWeaponSheathed;
    public void WeaponSheathed()
    {
        if(onWeaponSheathed != null) 
        {
            onWeaponSheathed();
        }
    }

}
