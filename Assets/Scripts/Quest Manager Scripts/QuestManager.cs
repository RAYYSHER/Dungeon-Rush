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
        // ★ Generate reward ตอน offer — แสดงให้ผู้เล่นเห็นก่อนตัดสินใจ
        QuestReward reward = QuestRewardGenerator.Instance.Generate();

        questDetailUI.Show(data, reward,
            onAccept: () => AcceptQuest(data, board, reward),
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

    // ★ รับ reward เข้ามาด้วย เก็บไว้ใช้ตอน complete
    void AcceptQuest(QuestData data, QuestBoard board, QuestReward reward)
    {
        board.Lock();
        var instance = new QuestInstance(data);
        instance.OnProgressChanged += OnProgressChanged;
        instance.OnCompleted       += OnQuestCompleted;
        activeQuests.Add(instance);
        questTrackerPanel.AddEntry(instance, board.GetQuestTimer());

        // ★ เก็บ reward ที่ generate ไว้แล้วตอน offer
        pendingRewards[instance] = reward;
    }

    void OnProgressChanged(QuestInstance quest)
    {
        questTrackerPanel.UpdateEntry(quest);
    }

    void OnQuestCompleted(QuestInstance quest)
    {
        // ★ ไม่ generate ใหม่ — ใช้ reward ที่เก็บไว้ตอน offer
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