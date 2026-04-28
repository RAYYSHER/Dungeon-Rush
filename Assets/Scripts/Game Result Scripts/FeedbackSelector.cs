using System.Collections.Generic;
using UnityEngine;

public class FeedbackSelector
{
    #region Feedback Result

    public List<string> PositiveFeedbacks { get; private set; } = new List<string>();
    public List<string> NegativeFeedbacks { get; private set; } = new List<string>();

    #endregion

    #region Attributes

    private FeedbackData _data;

    #endregion

    #region Constructor

    public FeedbackSelector(FeedbackData data)
    {
        _data = data;
    }

    #endregion

    #region Public Methods

    public void Select(string rankLetter, float stsScore, float timeScore)
    {
        PositiveFeedbacks.Clear();
        NegativeFeedbacks.Clear();

        int positiveCount = 0;
        int negativeCount = 0;

        switch (rankLetter)
        {
            case "S": positiveCount = 3; negativeCount = 0; break;
            case "A": positiveCount = 2; negativeCount = 1; break;
            case "B": positiveCount = 2; negativeCount = 1; break;
            case "C": positiveCount = 1; negativeCount = 2; break;
            case "D": positiveCount = 0; negativeCount = 3; break;
        }

        FillFeedbacks(stsScore, timeScore, positiveCount, negativeCount);
    }

    #endregion

    #region Private Methods

    private void FillFeedbacks(float stsScore, float timeScore, int positiveCount, int negativeCount)
    {
        bool stsIsGood  = stsScore  >= 56f;
        bool timeIsGood = timeScore >= 56f;

        for (int i = 0; i < positiveCount; i++)
        {
            if (stsIsGood && _data.stsPositivePool.Count > 0)
                PositiveFeedbacks.Add(PickRandom(_data.stsPositivePool));
            else if (timeIsGood && _data.timePositivePool.Count > 0)
                PositiveFeedbacks.Add(PickRandom(_data.timePositivePool));
        }

        for (int i = 0; i < negativeCount; i++)
        {
            if (!stsIsGood && _data.stsNegativePool.Count > 0)
                NegativeFeedbacks.Add(PickRandom(_data.stsNegativePool));
            else if (!timeIsGood && _data.timeNegativePool.Count > 0)
                NegativeFeedbacks.Add(PickRandom(_data.timeNegativePool));
        }
    }

    private string PickRandom(List<string> pool)
    {
        int index = Random.Range(0, pool.Count);
        return pool[index];
    }

    #endregion
}