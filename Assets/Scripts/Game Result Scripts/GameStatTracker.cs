using System.Collections.Generic;
using UnityEngine;

public class GameStatTracker : MonoBehaviour
{
    public static GameStatTracker Instance { get; private set; }

    #region Tracked Stats

    public int   Eliminations    { get; private set; }
    public float ClearTime       { get; private set; }
    public float AverageSTS      { get; private set; }

    #endregion

    #region Private

    private bool  _timerRunning        = false;
    private float _stsSnapshotInterval = 2f;
    private float _stsSnapshotTimer    = 0f;
    private List<float> _stsSnapshots  = new List<float>();
    private Stress _stressSystem;

    #endregion

    #region Function

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _stressSystem = FindFirstObjectByType<Stress>();

        ResetStats();
        StartTimer();
    }

    void Update()
    {
        if (!_timerRunning) return;

        // Count clear time
        ClearTime += Time.deltaTime;

        // Snapshot STS every interval
        if (_stressSystem != null)
        {
            _stsSnapshotTimer -= Time.deltaTime;
            if (_stsSnapshotTimer <= 0f)
            {
                _stsSnapshotTimer = _stsSnapshotInterval;
                TakeSTSSnapshot();
            }
        }
    }

    #endregion

    #region Public methods

    // Hook: call from Zombie.Die() and FloorBoss.Die()
    public void AddElimination()
    {
        Eliminations++;
    }

    // Hook: call from FloorBoss.Die() — stops the timer and finalizes avg STS
    public void StopTimer()
    {
        if (!_timerRunning) return;

        _timerRunning = false;

        // Take one last snapshot at the moment of victory
        TakeSTSSnapshot();
        CalculateAverageSTS();

        Debug.Log($"[GameStatTracker] Run ended → Time: {ClearTime:F1}s | AvgSTS: {AverageSTS:F1} | Eliminations: {Eliminations}");
    }

    // Returns STS score 0–100 (higher = calmer = better)
    public float GetSTSScore()
    {
        if (_stressSystem == null) return 100f;
        return (1f - (AverageSTS / _stressSystem.maxSts)) * 100f;
    }

    // Returns time score 0–100 (shorter clear time = higher score)
    // maxExpectedSeconds should match WorldTimer.timerDurationInMinutes × 60
    public float GetTimeScore(float maxExpectedSeconds = 300f)
    {
        float ratio = Mathf.Clamp01(ClearTime / maxExpectedSeconds);
        return (1f - ratio) * 100f;
    }

    #endregion

    #region Private Methods

    void ResetStats()
    {
        Eliminations      = 0;
        ClearTime         = 0f;
        AverageSTS        = 0f;
        _timerRunning     = false;
        _stsSnapshotTimer = _stsSnapshotInterval;
        _stsSnapshots.Clear();
    }

    void StartTimer()
    {
        _timerRunning     = true;
        _stsSnapshotTimer = _stsSnapshotInterval;
    }

    void TakeSTSSnapshot()
    {
        if (_stressSystem == null) return;
        _stsSnapshots.Add(_stressSystem.sts);
    }

    void CalculateAverageSTS()
    {
        if (_stsSnapshots.Count == 0)
        {
            AverageSTS = 0f;
            return;
        }

        float total = 0f;
        foreach (float s in _stsSnapshots)
            total += s;

        AverageSTS = total / _stsSnapshots.Count;
    }

    #endregion
}