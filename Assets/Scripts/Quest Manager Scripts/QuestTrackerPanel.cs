using System.Collections.Generic;
using UnityEngine;

public class QuestTrackerPanel : MonoBehaviour
{
    [SerializeField] Transform content;              // parent with Vertical Layout Group
    [SerializeField] QuestTrackerEntry entryPrefab;

    readonly Dictionary<QuestInstance, QuestTrackerEntry> entries = new();

    void Awake() => gameObject.SetActive(false);

    public void AddEntry(QuestInstance quest)
    {
        var entry = Instantiate(entryPrefab, content);
        entry.Init(quest);
        entries[quest] = entry;
        gameObject.SetActive(true);
    }

    public void UpdateEntry(QuestInstance quest)
    {
        if (entries.TryGetValue(quest, out var entry))
            entry.UpdateProgress(quest);
    }

    public void RemoveEntry(QuestInstance quest)
    {
        if (!entries.TryGetValue(quest, out var entry)) 
            return;
        entries.Remove(quest);
        Destroy(entry.gameObject);
        if (entries.Count == 0) 
            gameObject.SetActive(false);
    }
}
