using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [SerializeField] QuestDetailUI     questDetailUI;
    [SerializeField] QuestTrackerPanel questTrackerPanel;
    [SerializeField] QuestCompleteUI   questCompleteUI;

    readonly List<QuestInstance>                    activeQuests    = new();
    readonly Queue<QuestInstance>                   completionQueue = new();
    readonly Dictionary<QuestInstance, QuestReward> pendingRewards  = new();
    bool isShowingComplete;

    void Awake() { Instance = this; }

    public void ShowQuestOffer(QuestData data, QuestBoard board)
    {
        questDetailUI.Show(data,
            onAccept: () => AcceptQuest(data, board),
            onReject: () => { }
        );
    }

    public void NotifyEnemyKilled()
    {
        foreach (var quest in activeQuests) quest.RegisterKill();
    }

    public void NotifySurviveComplete()
    {
        foreach (var quest in activeQuests) quest.RegisterSurvive();
    }

    public void NotifyZoneComplete()
    {
        foreach (var quest in activeQuests) quest.RegisterZoneComplete();
    }

    void AcceptQuest(QuestData data, QuestBoard board)
    {
        board.Lock();
        var instance = new QuestInstance(data);
        instance.OnProgressChanged += OnProgressChanged;
        instance.OnCompleted       += OnQuestCompleted;
        activeQuests.Add(instance);
        questTrackerPanel.AddEntry(instance, board.GetQuestTimer());
    }

    void OnProgressChanged(QuestInstance quest)
    {
        questTrackerPanel.UpdateEntry(quest);
    }

    void OnQuestCompleted(QuestInstance quest)
    {
        // Generate reward ตอน complete ทันที
        pendingRewards[quest] = QuestRewardGenerator.Generate();
        completionQueue.Enqueue(quest);
        TryShowNextComplete();
    }

    void TryShowNextComplete()
    {
        if (isShowingComplete || completionQueue.Count == 0) return;

        isShowingComplete = true;
        var quest = completionQueue.Dequeue();

        pendingRewards.TryGetValue(quest, out QuestReward reward);
        pendingRewards.Remove(quest);

        // ★ ส่ง reward เข้าไปด้วย
        questCompleteUI.Show(quest, reward, () => FinishQuest(quest));
    }

    void FinishQuest(QuestInstance quest)
    {
        activeQuests.Remove(quest);
        questTrackerPanel.RemoveEntry(quest);
        isShowingComplete = false;
        TryShowNextComplete();
    }
}