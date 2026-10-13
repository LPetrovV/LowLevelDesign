using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{

    public string skillName = "Placeholder Skill";
    public bool isUnlocked = false;

    // previous skill required to unlock the next skill in chain
    public SkillNode prerequisite;

    public Slider speedSlider;
    
    public void TryUnlock()
    {
        if(isUnlocked) return;
        if(prerequisite != null && !prerequisite.isUnlocked)
        {
            Debug.Log("Unlock the previous skill first.");
            return;
        }
        if (SkillPointManager.Instance.TrySpendSkillPoint())
        {
            isUnlocked = true;
            speedSlider.value++;
            Debug.Log($"{skillName} isUnlocked!");
        }
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
        //if (Keyboard.current.lKey.wasPressedThisFrame)
        //{
        //    TryUnlock();
        //}
    }
}
