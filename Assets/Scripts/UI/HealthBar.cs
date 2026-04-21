using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public Camera mainCamera;
    public bool isFloating = false;

    void Awake()
    {
        slider = GetComponent<Slider>();
        mainCamera = FindAnyObjectByType<Camera>();
    }
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFloating == true)
        {
            transform.rotation = mainCamera.transform.rotation;    
        }
    }
}
