using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents
{
    public event Action onADHDTriggered;
    public void ADHDTriggered()
    {
        if(onADHDTriggered != null)
        {
            onADHDTriggered();
        }
    }
    
    public event Action onAnxietyTriggered;
    public void AnxietyTriggered()
    {
        if(onAnxietyTriggered != null)
        {
    
            onAnxietyTriggered();
        }
    }

    public event Action onPanicAttackTriggered;
    public void PanicAttackTriggered()
    {
        if(onPanicAttackTriggered != null)
        {
            onPanicAttackTriggered();
        }
    }

    public event Action onInattentivenessTriggered;
    public void InattentivenessTriggered()
    {
        if(onInattentivenessTriggered != null)
        {
            onInattentivenessTriggered();
     
        }
    }
}
