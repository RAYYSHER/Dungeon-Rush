using UnityEngine;

public class RankCalculator
{
    #region Rank Result

    public string RankLetter      { get; private set; }
    public string RankLabel       { get; private set; }
    public float  FinalScore      { get; private set; }

    #endregion

    #region Public Methods

    public void Calculate(float timeScore, float stsScore, bool isWin)
    {
        float levelMultiplier = 1.0f; // placeholder รอ skill system

        FinalScore = ((timeScore * 0.6f) + (stsScore * 0.4f)) * levelMultiplier;

        if (!isWin)
        {
            FinalScore *= 0.5f;
        }

        FinalScore = Mathf.Clamp(FinalScore, 0f, 100f);

        AssignRank(FinalScore);

        Debug.Log($"[RankCalculator] TimeScore: {timeScore:F1} | STSScore: {stsScore:F1} | Final: {FinalScore:F1} | Rank: {RankLetter}");
    }

    #endregion

    #region Private Methods

    private void AssignRank(float score)
    {
        if (score >= 85f)
        {
            RankLetter = "S";
            RankLabel  = "Excellent";
        }
        else if (score >= 71f)
        {
            RankLetter = "A";
            RankLabel  = "Great";
        }
        else if (score >= 56f)
        {
            RankLetter = "B";
            RankLabel  = "Good";
        }
        else if (score >= 41f)
        {
            RankLetter = "C";
            RankLabel  = "Normal";
        }
        else
        {
            RankLetter = "D";
            RankLabel  = "Bad";
        }
    }

    #endregion
}