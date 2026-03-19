using UnityEngine;
using UnityEngine.UI;

public class StressBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>(); 
    }
    public void UpdateStressBar(float currentSts, float maxSts)
    {
        slider.value = currentSts / maxSts;
        
    }
}
