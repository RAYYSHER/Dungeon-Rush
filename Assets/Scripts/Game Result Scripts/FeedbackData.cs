using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FeedbackData", menuName = "Feedback/Feedback Data" )] 
public class FeedbackData : ScriptableObject
{
    [Header("STS Feedback")]
    public List<string> stsPositivePool;
    public List<string> stsNegativePool;

    [Header("Time Feedback")]
    public List<string> timePositivePool;
    public List<string> timeNegativePool;

    [Header("Rank Descriptions")]
    public List<RankDescription> rankDescriptions;

    // หา description จาก rank letter
    public string GetRankDescription(string rankLetter)
    {
        foreach (RankDescription rd in rankDescriptions)
        {
            if (rd.rankLetter == rankLetter)
                return rd.description;
        }
        return "";
    }
}

[System.Serializable]
public class RankDescription
{
    public string rankLetter;
    [TextArea] public string description;
}
