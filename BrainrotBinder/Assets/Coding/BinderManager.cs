using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BinderManager : MonoBehaviour
{
    public GameObject binderPanel;
    public GameObject wrongFeedback;
    public Slider brainrotBar;
    public TMP_Text requestText;

    public BookPages bookPages;

    public int brainrotPerStudent = 20;
    public int wrongPenalty = 10;

    // Character Sounds
    public AudioSource characterAudioSource;

    public AudioClip tungTungSound;
    public AudioClip ballerinaSound;
    public AudioClip tralaleroSound;
    public AudioClip cappuccinoSound;

    // Wrong Answer Sound
    public AudioClip wrongBuzzSound;

    private GameObject currentStudent;
    private int correctCharacter;

    private string[] characterNames =
    {
        "TUNG TUNG TUNG SAHUR",
        "BALLERINA CAPPUCCINA",
        "TRALALERO TRALALA",
        "CAPPUCCINO ASSASSINO"
    };

    private void Start()
    {
        brainrotBar.value = 0;

        wrongFeedback.SetActive(false);
        binderPanel.SetActive(false);

        if (requestText != null)
        {
            requestText.text = "";
        }

        Time.timeScale = 1f;
    }

    public void OpenBinder(
        GameObject student,
        int requestedCharacter
    )
    {
        currentStudent = student;
        correctCharacter = requestedCharacter;

        if (
            requestText != null &&
            requestedCharacter >= 0 &&
            requestedCharacter < characterNames.Length
        )
        {
            requestText.text =
                "STUDENT WANTS:\n" +
                characterNames[requestedCharacter];
        }

        binderPanel.SetActive(true);

        if (bookPages != null)
        {
            bookPages.ResetPages();
        }

        // يوقف الطلاب والحركة
        // لكن التايمر يستمر
        Time.timeScale = 0f;
    }

    public void ChooseCharacter(int choice)
    {
        // يشغل صوت الشخصية
        PlayCharacterSound(choice);

        if (choice == correctCharacter)
        {
            CorrectChoice();
        }
        else
        {
            WrongChoice();
        }
    }

    private void PlayCharacterSound(int choice)
    {
        if (characterAudioSource == null)
        {
            return;
        }

        if (choice == 0 && tungTungSound != null)
        {
            characterAudioSource.PlayOneShot(tungTungSound);
        }
        else if (choice == 1 && ballerinaSound != null)
        {
            characterAudioSource.PlayOneShot(ballerinaSound);
        }
        else if (choice == 2 && tralaleroSound != null)
        {
            characterAudioSource.PlayOneShot(tralaleroSound);
        }
        else if (choice == 3 && cappuccinoSound != null)
        {
            characterAudioSource.PlayOneShot(cappuccinoSound);
        }
    }

    private void CorrectChoice()
    {
        Debug.Log("Correct!");

        StopAllCoroutines();

        wrongFeedback.SetActive(false);

        brainrotBar.value += brainrotPerStudent;

        binderPanel.SetActive(false);

        if (currentStudent != null)
        {
            StudentFollow studentFollow =
                currentStudent.GetComponent<StudentFollow>();

            if (studentFollow != null)
            {
                studentFollow.Leave();
            }

            currentStudent = null;
        }

        // Check if Brainrot Bar is full
        if (brainrotBar.value >= brainrotBar.maxValue)
        {
            GameTimer gameTimer =
                FindAnyObjectByType<GameTimer>();

            if (gameTimer != null)
            {
                gameTimer.WinGame();
            }

            return;
        }

        Time.timeScale = 1f;
    }

    private void WrongChoice()
    {
        Debug.Log("Wrong!");

        // BUZZ SOUND
        if (
            characterAudioSource != null &&
            wrongBuzzSound != null
        )
        {
            characterAudioSource.PlayOneShot(wrongBuzzSound);
        }

        brainrotBar.value -= wrongPenalty;

        StopAllCoroutines();

        StartCoroutine(HandleWrongChoice());
    }

    private IEnumerator HandleWrongChoice()
    {
        wrongFeedback.SetActive(true);

        RectTransform binderRect =
            binderPanel.GetComponent<RectTransform>();

        Vector2 originalPosition =
            binderRect.anchoredPosition;

        float duration = 0.4f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float randomX =
                Random.Range(-12f, 12f);

            float randomY =
                Random.Range(-12f, 12f);

            binderRect.anchoredPosition =
                originalPosition +
                new Vector2(randomX, randomY);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        binderRect.anchoredPosition =
            originalPosition;

        yield return new WaitForSecondsRealtime(0.3f);

        wrongFeedback.SetActive(false);

        binderPanel.SetActive(false);

        if (currentStudent != null)
        {
            StudentFollow studentFollow =
                currentStudent.GetComponent<StudentFollow>();

            if (studentFollow != null)
            {
                studentFollow.Leave();
            }

            currentStudent = null;
        }

        Time.timeScale = 1f;
    }
}