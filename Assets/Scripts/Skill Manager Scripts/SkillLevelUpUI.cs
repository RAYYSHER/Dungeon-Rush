using UnityEngine;
using UnityEngine.EventSystems;

public class SkillLevelUpUI : MonoBehaviour
{
    // ─────────────────────────────────────────
    //  Inspector
    // ─────────────────────────────────────────

    [Header("Panel")]
    [SerializeField] private GameObject     _panel;
    [SerializeField] private GameObject     _firstSelected;     // ปุ่มแรกที่ controller จะ focus
    [SerializeField] private UISelectionGuard _selectionGuard;

    [Header("Skill Cards")]
    [SerializeField] private SkillCardUI    _cardLeft;
    [SerializeField] private SkillCardUI    _cardRight;

    public bool IsShowing { get; private set; }


    // ─────────────────────────────────────────
    //  Build-in Functions
    // ─────────────────────────────────────────

    void Start()
    {
        _panel.SetActive(false);
    }

    // ─────────────────────────────────────────
    //  Public — called by LevelSystem
    // ─────────────────────────────────────────

    public void Show()
    {
        IsShowing = true;
        _panel.SetActive(true);
        Time.timeScale = 0f;

        if (SkillManager.Instance.HasEmptySlot)
        {
            ShowNewSkillOffer();
        }
        else
        {
            // ★ เช็ค all-max ก่อน
            SkillInstance[] slots = SkillManager.Instance.GetUpgradeOffers();
            if (slots[0].IsMaxLevel && slots[1].IsMaxLevel)
            {
                ShowAlternativeReward();
            }
            else
            {
                ShowUpgradeOffer();
            }
        }

        StartCoroutine(SelectAfterFrame(_firstSelected));
    }

    // ─────────────────────────────────────────
    //  Show Modes
    // ─────────────────────────────────────────

    private void ShowNewSkillOffer()
    {
        SkillData[] offers = SkillManager.Instance.GetNewSkillOffers();

        // offers อาจมีแค่ 1 อัน ถ้า pool เหลือน้อย
        bool hasLeft  = offers.Length > 0;
        bool hasRight = offers.Length > 1;

        _cardLeft.gameObject.SetActive(hasLeft);
        _cardRight.gameObject.SetActive(hasRight);

        if (hasLeft)
        {
            _cardLeft.SetupNewSkill(offers[0], onChoose: () =>
            {
                SkillManager.Instance.AddSkill(offers[0]);
                Hide();
            });
        }

        if (hasRight)
        {
            _cardRight.SetupNewSkill(offers[1], onChoose: () =>
            {
                SkillManager.Instance.AddSkill(offers[1]);
                Hide();
            });
        }
    }

    private void ShowUpgradeOffer()
    {
        SkillInstance[] slots = SkillManager.Instance.GetUpgradeOffers();

        _cardLeft.gameObject.SetActive(true);
        _cardRight.gameObject.SetActive(true);

        _cardLeft.SetupUpgrade(slots[0], onChoose: () =>
        {
            SkillManager.Instance.UpgradeSkill(0);
            Hide();
        });

        _cardRight.SetupUpgrade(slots[1], onChoose: () =>
        {
            SkillManager.Instance.UpgradeSkill(1);
            Hide();
        });
    }

    private void ShowAlternativeReward()
    {
        QuestReward reward = QuestRewardGenerator.Instance.GenerateAlternative();

        // แสดงแค่ card ซ้าย ซ่อนขวา
        _cardLeft.gameObject.SetActive(true);
        _cardRight.gameObject.SetActive(false);

        _cardLeft.SetupAlternativeReward(reward);

        // ★ Choose button ต้องกดได้เพื่อรับ reward และปิด panel
        _cardLeft.SetupChooseCallback(() =>
        {
            SkillManager.Instance.ApplyReward(reward);
            Hide();
        });
    }

    // ─────────────────────────────────────────
    //  Hide
    // ─────────────────────────────────────────

    private void Hide()
    {
        IsShowing = false;
        _panel.SetActive(false);
        Time.timeScale = 1f;
        EventSystem.current.SetSelectedGameObject(null);

        // ตรวจสอบว่าสะสม exp ค้างไว้มั้ยระหว่างที่ panel เปิดอยู่
        // LevelSystem จะ check ใหม่ใน GainXP ครั้งถัดไปเอง
    }

    // ─────────────────────────────────────────
    //  Helpers
    // ─────────────────────────────────────────

    private System.Collections.IEnumerator SelectAfterFrame(GameObject target)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(target);
    }
}