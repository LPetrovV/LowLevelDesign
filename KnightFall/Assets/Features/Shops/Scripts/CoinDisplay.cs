using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    private TextMeshProUGUI coinText;

    void Awake()
    {
        coinText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (PlayerStats.Instance != null)
            coinText.text = "Coins: " + PlayerStats.Instance.coins;
    }
}