using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoals
{
    public void GetGoal();
    //GET goal description
    public string GetGoalDescription();
    //READ Current goal progress
    public void CurrentGoalProgress();
    //UPDATE if on checking goal has been completed;
    public bool GoalCompleted();
}
