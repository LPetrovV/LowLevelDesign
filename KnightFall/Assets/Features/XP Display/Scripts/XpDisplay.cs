using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XpDisplay : MonoBehaviour
{
    public Slider xpBar;
    public TMP_Text levelText;

    void OnEnable()  { PlayerStats.OnXpChanged += Refresh; }
    void OnDisable() { PlayerStats.OnXpChanged -= Refresh; }

    void Start()   // Start, not OnEnable, so PlayerStats.Awake has already run
    {
        var s = PlayerStats.Instance;
        if (s != null) Refresh(s.level, s.xp, s.XpToNextLevel);
    }

    void Refresh(int level, int xp, int next)
    {
        xpBar.maxValue = next;
        xpBar.value = xp;
        levelText.text = "Lv " + level;
    }
}
