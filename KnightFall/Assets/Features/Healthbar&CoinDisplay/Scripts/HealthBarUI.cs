using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetMaxHealth(int max)
    {
        slider.maxValue = max;
    }

    public void SetHealth(int current)
    {
        slider.value = current;
    }
}