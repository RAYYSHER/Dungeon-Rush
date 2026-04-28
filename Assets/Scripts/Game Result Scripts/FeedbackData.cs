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
}
