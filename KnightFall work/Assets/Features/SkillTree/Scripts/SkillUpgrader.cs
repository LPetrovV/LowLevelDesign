using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUpgrader : MonoBehaviour
{
    // variables
    public Slider skillSlider;
    public string skillName = "Skill";
    public enum StatType {Speed, Dexterity, Combat}
    public StatType statToUpgrade;
    public int amountToAdd = 1;
    public TMP_Text counterText;

    public void TryUpgrade()
    {
        // when skill alr has max points, nothing happens
        if(skillSlider.value >= skillSlider.maxValue)
        {
            Debug.Log($"{skillName} maxed out rn");
            return;
        }
        // if skill point is spent successfully, add 1 to skill
        if (SkillPointManager.Instance.TrySpendSkillPoint())
        {
            skillSlider.value++;
            counterText.text = $"{skillSlider.value}/{skillSlider.maxValue}";
            switch (statToUpgrade)
            {
                case StatType.Speed:
                PlayerStats.Instance.speed += amountToAdd; break;
                case StatType.Dexterity:
                PlayerStats.Instance.dexterity += amountToAdd; break;
                case StatType.Combat:
                PlayerStats.Instance.combat += amountToAdd; break;
            }
        }
        // if there isnt enough skill points, nothing happens
        else
        {
            Debug.Log("Not enough skill points.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
