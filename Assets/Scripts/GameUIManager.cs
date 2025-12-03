using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text hpText;
    public TMP_Text livesText;
    public float levelTime = 300f;
    private float timer;

    public PlayerController player;

    void Start()
    {
        timer = levelTime;
        if (player == null)
            player = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f) timer = 0f;

        int min = Mathf.FloorToInt(timer / 60f);
        int sec = Mathf.FloorToInt(timer % 60f);
        if (timerText != null)
            timerText.text = $"{min:00}:{sec:00}";

        if (player != null)
        {
            // Simple HP/lives simulation
            if (hpText != null) hpText.text = $"HP: 100/100";
            if (livesText != null) livesText.text = $"Lives: 3";
        }
    }
}
