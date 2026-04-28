using System;
using UnityEngine;

[Serializable]
public class QuestGoal
{
    public QuestGoalType type;
    public string description; // e.g. "Kill 10 Enemies"
    public int targetCount; // e.g. "10"
}
