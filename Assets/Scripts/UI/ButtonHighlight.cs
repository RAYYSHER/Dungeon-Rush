using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class ButtonHighlight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Image borderImage;
    [SerializeField] private Image whiteBorderImage; // Image ลูกที่ใช้เป็น border

    [Header("Normal State")]
    [SerializeField] private float normalFontSize = 20f;
    [SerializeField] private FontStyles normalFontStyle = FontStyles.Normal;
    [SerializeField] private Vector2 normalSize;  // เก็บขนาดเดิมไว้

    [Header("Highlighted State")]
    [SerializeField] private float highlightFontSize = 15f;
    [SerializeField] private FontStyles highlightFontStyle = FontStyles.Bold;
    [SerializeField] private float sizeAddY = 10f;
    [SerializeField] private float sizeAddX = 20f;

    

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        normalSize = rectTransform.sizeDelta;
    }

    public void OnHighlight()
    {
        // สี
        // buttonImage.color = Color.black;
        buttonText.color  = Color.white;

        // Font
        buttonText.fontSize  = highlightFontSize;
        buttonText.fontStyle = highlightFontStyle;

        // Border
        if (borderImage != null) borderImage.gameObject.SetActive(true);
        if (whiteBorderImage != null) whiteBorderImage.gameObject.SetActive(true);

        // Box size — เพิ่มแค่ X และ Y 
        rectTransform.sizeDelta = new Vector2(normalSize.x + sizeAddX, normalSize.y + sizeAddY);
    }

    public void OnNormal()
    {
        // ถ้า mouse กำลัง click อยู่ ไม่ทำอะไร ป้องกันปุ่มหดตอน click
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            return;

        // สี
        // buttonImage.color = Color.white;
        buttonText.color  = Color.black;

        // Font
        buttonText.fontSize  = normalFontSize;
        buttonText.fontStyle = normalFontStyle;

        // Border
        if (borderImage != null) borderImage.gameObject.SetActive(false);
        if (whiteBorderImage != null) whiteBorderImage.gameObject.SetActive(false);

        // Box size — คืนค่าเดิม
        rectTransform.sizeDelta = normalSize;
    }
}