using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SkillPointManager : MonoBehaviour
{
    // i dont know what this is rn but bear with me for a second
    public static SkillPointManager Instance;
    public int AvailableSkillPoints { get; private set; } = 0;
    public static event Action<int> OnSkillPointsChanged;


    void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Keyboard.current.kKey.wasPressedThisFrame) // placeholder test key
        // {
        //     AddSkillPoint();
        // } 
    }

    public void AddSkillPoint()
    {
        AvailableSkillPoints++;
        OnSkillPointsChanged?.Invoke(AvailableSkillPoints);
        Debug.Log($"Skill point granted. Total: {AvailableSkillPoints}");
    }

    public bool TrySpendSkillPoint()
    {
        if(AvailableSkillPoints <= 0) return false;

        AvailableSkillPoints--;
        OnSkillPointsChanged?.Invoke(AvailableSkillPoints);
        return true;
    }
}
