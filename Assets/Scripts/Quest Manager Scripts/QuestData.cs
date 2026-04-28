using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest Data")]
public class QuestData : ScriptableObject
{
    public string questName;
    [TextArea] public string description;
    public QuestGoal[] goals;
    public string rewardDescription; // placeholder until skill system is implemented
}
