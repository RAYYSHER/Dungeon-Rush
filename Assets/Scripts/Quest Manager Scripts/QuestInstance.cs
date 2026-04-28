using System;

public class QuestInstance
{
    public QuestData data;
    public int[] goalProgress;
    public bool IsComplete { get; private set; }

    public event Action<QuestInstance> OnProgressChanged;
    public event Action<QuestInstance> OnCompleted;

    public QuestInstance(QuestData data)
    {
        this.data = data;
        goalProgress = new int[data.goals.Length];
    }

    public void RegisterKill()
    {
        if (IsComplete) 
            return;
        bool changed = false;
        for (int i = 0; i < data.goals.Length; i++)
        {
            var goal = data.goals[i];
            if (goal.type == QuestGoalType.KillCount && goalProgress[i] < goal.targetCount)
            {
                goalProgress[i]++;
                changed = true;
            }
        }
        if (!changed) 
            return;
        OnProgressChanged?.Invoke(this);
        CheckCompletion();
    }

    void CheckCompletion()
    {
        for (int i = 0; i < data.goals.Length; i++)
        {
            if (goalProgress[i] < data.goals[i].targetCount) 
                return;
        }
        IsComplete = true;
        OnCompleted?.Invoke(this);
    }
}
