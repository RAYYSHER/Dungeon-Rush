using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [SerializeField] QuestDetailUI questDetailUI;
    [SerializeField] QuestTrackerPanel questTrackerPanel;
    [SerializeField] QuestCompleteUI questCompleteUI;

    readonly List<QuestInstance> activeQuests = new();
    readonly Queue<QuestInstance> completionQueue = new();
    bool isShowingComplete;

    void Awake()
    {
        Instance = this;
    }

    // Called by QuestBoard when player interacts with it.
    public void ShowQuestOffer(QuestData data, QuestBoard board)
    {
        questDetailUI.Show(data,
            onAccept: () => AcceptQuest(data, board),
            onReject: () => { }
        );
    }

    void AcceptQuest(QuestData data, QuestBoard board)
    {
        board.Lock();
        var instance = new QuestInstance(data);
        instance.OnProgressChanged += OnProgressChanged;
        instance.OnCompleted += OnQuestCompleted;
        activeQuests.Add(instance);
        questTrackerPanel.AddEntry(instance);
    }

    // Called by Player.GetXP() — fires every time any enemy dies.
    public void NotifyEnemyKilled()
    {
        foreach (var quest in activeQuests)
            quest.RegisterKill();
    }

    void OnProgressChanged(QuestInstance quest)
    {
        questTrackerPanel.UpdateEntry(quest);
    }

    void OnQuestCompleted(QuestInstance quest)
    {
        completionQueue.Enqueue(quest);
        TryShowNextComplete();
    }

    void TryShowNextComplete()
    {
        if (isShowingComplete || completionQueue.Count == 0) 
            return;
        isShowingComplete = true;
        var quest = completionQueue.Dequeue();
        questCompleteUI.Show(quest, () => FinishQuest(quest));
    }

    void FinishQuest(QuestInstance quest)
    {
        activeQuests.Remove(quest);
        questTrackerPanel.RemoveEntry(quest);
        isShowingComplete = false;
        TryShowNextComplete();
    }
}
