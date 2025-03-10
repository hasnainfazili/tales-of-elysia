using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D20Roll : MonoBehaviour
{
    DiceRoll diceRoll;
    [SerializeField] private int diceSides = 20;
    public int rolledValue;
    void Awake(){
        diceRoll = new DiceRoll();
        diceRoll.AddDice(diceSides);
    }

    public int Roll(){
        return diceRoll.RollValue();
    }
}
