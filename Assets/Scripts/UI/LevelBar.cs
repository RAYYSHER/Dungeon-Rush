using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text levelText; // ADD THIS - drag assign in Inspector
    

    void Awake()
    {
        slider = GetComponent<Slider>(); 
    }
    public void UpdateLevelBar(float currentExp, float nextLevelExp, int level)
    {
        slider.value = currentExp / nextLevelExp;
        if (levelText != null)
        {
            // levelText.text = $"Lv.{level}";
            levelText.text = level.ToString("D2");
        }
    }
  
}
