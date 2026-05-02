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

    // [Header("Skill Cards")]
    // [SerializeField] private SkillCardUI    _cardLeft;
    // [SerializeField] private SkillCardUI    _cardRight;

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
        _panel.SetActive(true);
        Time.timeScale = 0f;

        if (SkillManager.Instance.HasEmptySlot)
        {
            ShowNewSkillOffer();
        }
        else
        {
            ShowUpgradeOffer();
        }

        StartCoroutine(SelectAfterFrame(_firstSelected));
    }

    // ─────────────────────────────────────────
    //  Show Modes
    // ─────────────────────────────────────────

    private void ShowNewSkillOffer()
    {
        // SkillData[] offers = SkillManager.Instance.GetNewSkillOffers();

        // // offers อาจมีแค่ 1 อัน ถ้า pool เหลือน้อย
        // bool hasLeft  = offers.Length > 0;
        // bool hasRight = offers.Length > 1;

        // _cardLeft.gameObject.SetActive(hasLeft);
        // _cardRight.gameObject.SetActive(hasRight);

        // if (hasLeft)
        // {
        //     _cardLeft.SetupNewSkill(offers[0], onChoose: () =>
        //     {
        //         SkillManager.Instance.AddSkill(offers[0]);
        //         Hide();
        //     });
        // }

        // if (hasRight)
        // {
        //     _cardRight.SetupNewSkill(offers[1], onChoose: () =>
        //     {
        //         SkillManager.Instance.AddSkill(offers[1]);
        //         Hide();
        //     });
        // }
    }

    private void ShowUpgradeOffer()
    {
        // SkillInstance[] slots = SkillManager.Instance.GetUpgradeOffers();

        // _cardLeft.gameObject.SetActive(true);
        // _cardRight.gameObject.SetActive(true);

        // _cardLeft.SetupUpgrade(slots[0], onChoose: () =>
        // {
        //     SkillManager.Instance.UpgradeSkill(0);
        //     Hide();
        // });

        // _cardRight.SetupUpgrade(slots[1], onChoose: () =>
        // {
        //     SkillManager.Instance.UpgradeSkill(1);
        //     Hide();
        // });
    }

    // ─────────────────────────────────────────
    //  Hide
    // ─────────────────────────────────────────

    private void Hide()
    {
        _panel.SetActive(false);
        Time.timeScale = 1f;
        EventSystem.current.SetSelectedGameObject(null);
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