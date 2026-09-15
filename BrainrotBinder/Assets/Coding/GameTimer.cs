using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public AudioSource bellAudio;

    public float timeRemaining = 60f;

    private bool timerFinished = false;

    private void Update()
    {
        if (timerFinished)
        {
            return;
        }

        timeRemaining -= Time.unscaledDeltaTime;

        int displayedTime =
            Mathf.CeilToInt(timeRemaining);

        timerText.text =
            "BELL: " + displayedTime;

        if (timeRemaining <= 0)
        {
            EndTimer();
        }
    }

    private void EndTimer()
    {
        timerFinished = true;
        timeRemaining = 0;

        timerText.text = "BELL: 0";

        if (bellAudio != null)
        {
            bellAudio.Play();
        }

        Time.timeScale = 0f;

        Debug.Log("THE BELL RANG!");
    }

    public void AddTime(float seconds)
    {
        if (timerFinished)
        {
            return;
        }

        timeRemaining += seconds;

        Debug.Log(
            "Added " + seconds + " seconds!"
        );
    }
}