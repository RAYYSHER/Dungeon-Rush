using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatTracker : MonoBehaviour
{
    public static GameStatTracker Instance { get; private set; }

    #region Tracked Stats

    public int   Eliminations    { get; private set; }
    public float ClearTime       { get; private set; }
    public float AverageSTS      { get; private set; }
    public int PenaltyCount      { get; private set; }

    #endregion

    #region Score Settings

    [Header("Time Score Settings")]
    [Tooltip("เวลา (วินาที) ที่ถือว่าดีมาก → ได้ 100 คะแนน")]
    [SerializeField] private float _excellentClearTime  = 60f;

    [Tooltip("เวลา (วินาที) สูงสุดที่คาดไว้ → ได้ 0 คะแนน  (ควรตรงกับ WorldTimer)")]
    [SerializeField] private float _maxExpectedSeconds  = 300f;

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

        if (_stsSnapshots.Count > 0)
        {
            TakeSTSSnapshot();
        }

        CalculateAverageSTS();

        Debug.Log($"[GameStatTracker] Run ended → Time: {ClearTime:F1}s | AvgSTS: {AverageSTS:F1} | Eliminations: {Eliminations}");
    }

    // Returns STS score 0–100 (higher = calmer = better)
    // ใช้ stress ratio ล้วนๆ ก่อน — penalty deduction ไว้ใส่ใน update หน้า
    public float GetSTSScore()
    {
        if (_stressSystem == null) return 0f;
        if (_stsSnapshots.Count == 0) return 0f;

        float baseScore = (1f - (AverageSTS / _stressSystem.maxSts)) * 100f;
        return Mathf.Clamp(baseScore, 0f, 100f);
    }

    // Returns time score 0–100
    // ถ้า clear ใน excellentTime → 100 คะแนน
    // หลังจากนั้น decay เป็น linear จนถึง 0 ที่ maxExpectedSeconds
    public float GetTimeScore()
    {
        if (ClearTime <= _excellentClearTime)
            return 100f;

        float window = _maxExpectedSeconds - _excellentClearTime;
        if (window <= 0f) return 0f;

        float ratio = Mathf.Clamp01((ClearTime - _excellentClearTime) / window);
        return (1f - ratio) * 100f;
    }

    public void AddPenalty()
    {
        PenaltyCount++;
        Debug.Log($"[GameStatTracker] Penalty count: {PenaltyCount}");
    }

    #endregion

    #region Private Methods

    void ResetStats()
    {
        Eliminations      = 0;
        ClearTime         = 0f;
        AverageSTS        = 0f;
        PenaltyCount      = 0;
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
        if (_stressSystem == null) 
        {
            Debug.Log("[Snapshot] _stressSystem is NULL");
            return;
        }

        float value = _stressSystem.IsInPenalty ? _stressSystem.maxSts : _stressSystem.sts;
        _stsSnapshots.Add(value);
        Debug.Log($"[Snapshot] sts: {value:F1} | IsInPenalty: {_stressSystem.IsInPenalty} | Total snapshots: {_stsSnapshots.Count}");
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

    #region reset scene
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _stressSystem = FindFirstObjectByType<Stress>();
        ResetStats();
        StartTimer();
    }
    #endregion
}