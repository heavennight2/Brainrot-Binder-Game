using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public AudioSource bellAudio;

    public GameObject winPanel;
    public GameObject losePanel;

    public float timeRemaining = 30f;

    private bool gameEnded = false;

    private void Start()
    {
        Time.timeScale = 1f;

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (gameEnded)
        {
            return;
        }

        // مهم: يخلي التايمر يستمر حتى لو الطلاب موقفين
        timeRemaining -= Time.unscaledDeltaTime;

        int displayedTime =
            Mathf.CeilToInt(timeRemaining);

        timerText.text =
            "BELL: " + displayedTime;

        if (timeRemaining <= 0)
        {
            LoseGame();
        }
    }

    public void WinGame()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Debug.Log("YOU BRAINROTTED THE WHOLE SCHOOL!");
    }

    private void LoseGame()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;
        timeRemaining = 0;

        timerText.text = "BELL: 0";

        if (bellAudio != null)
        {
            bellAudio.Play();
        }

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Debug.Log("YOU FAILED!");
    }

    public void AddTime(float seconds)
    {
        if (gameEnded)
        {
            return;
        }

        timeRemaining += seconds;
    }
}