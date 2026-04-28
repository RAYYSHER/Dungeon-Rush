using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private Slider slider;
    private TMP_Text bossNameText;
    [SerializeField] private GameObject barUI;

    [Header("Proximity Setting")]
    public float visibleRange = 15f;
    private FloorBoss trackedBoss;
    private Player player;

    void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        player = FindAnyObjectByType<Player>();

        // Debug.Log($"[BossHealthBar] slider: {slider}");  
        // Debug.Log($"[BossHealthBar] barUI: {barUI}");    
        // Debug.Log($"[BossHealthBar] player: {player}");
    }

    void Start()
    {
        if (barUI != null)
        {
            barUI.SetActive(false);
        }
    }

    void Update()
    {
        if (player == null || trackedBoss == null)
        {
            return;
        }

        float distance = Vector3.Distance(trackedBoss.transform.position , player.transform.position);

        bool shouldShow = distance <= visibleRange;
        if (barUI != null && barUI.activeSelf != shouldShow)
        {
            barUI.SetActive(shouldShow);
        }
    }

    public void SetBoss(FloorBoss boss)
    {
        trackedBoss = boss;
        // Debug.Log($"[BossHealthBar] trackedBoss: {trackedBoss}"); 
    }

    public void UpdateBossHealthBar(float currentHP , float maxHP)
    {
        // Debug.Log($"[BossHealthBar] UpdateBossHealthBar called: {currentHP}/{maxHP}");
        slider.value = currentHP / maxHP;
    }

    public void SetBossName()
    {
        if (bossNameText != null)
        {
            bossNameText.text = name;
        }
    }
}
