using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die
{
    int sides;
    public int rollValue;
    public Die(int sides){
     this.sides = sides;
    }

    public void Roll()
    {
        rollValue = Random.Range(1,sides + 1);
    }
}

public class DiceRoll
{
    public List<Die> die;
    public DiceRoll(){
        die = new List<Die>();
    }

    public void AddDice(int sides){
        die.Add(new Die(sides));
    }

    public void Roll(){
        for(int i = 0; i < die.Count; i++){
            die[i].Roll();
        }
    }

    public int RollValue(){
        
        return die[0].rollValue;
    }
}
