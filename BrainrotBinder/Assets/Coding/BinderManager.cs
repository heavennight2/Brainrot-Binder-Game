using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BinderManager : MonoBehaviour
{
    public GameObject binderPanel;
    public GameObject wrongFeedback;
    public Slider brainrotBar;

    public int brainrotPerStudent = 20;
    public int wrongPenalty = 10;

    private GameObject currentStudent;
    private int correctCharacter;

    private void Start()
    {
        brainrotBar.value = 0;
        wrongFeedback.SetActive(false);
    }

    public void OpenBinder(
        GameObject student,
        int requestedCharacter
    )
    {
        currentStudent = student;
        correctCharacter = requestedCharacter;

        binderPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ChooseCharacter(int choice)
    {
        if (choice == correctCharacter)
        {
            CorrectChoice();
        }
        else
        {
            WrongChoice();
        }
    }

    private void CorrectChoice()
    {
        Debug.Log("Correct!");

        StopAllCoroutines();
        wrongFeedback.SetActive(false);

        brainrotBar.value += brainrotPerStudent;

        binderPanel.SetActive(false);
        Time.timeScale = 1f;

        currentStudent
            .GetComponent<StudentFollow>()
            .Leave();

        currentStudent = null;

        if (brainrotBar.value >= 100)
        {
            Debug.Log(
                "THE WHOLE SCHOOL IS BRAINROTTED!"
            );
        }
    }

    private void WrongChoice()
    {
        Debug.Log("Wrong!");

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
            float randomX = Random.Range(-12f, 12f);
            float randomY = Random.Range(-12f, 12f);

            binderRect.anchoredPosition =
                originalPosition +
                new Vector2(randomX, randomY);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        binderRect.anchoredPosition = originalPosition;

        yield return new WaitForSecondsRealtime(0.3f);

        wrongFeedback.SetActive(false);
        binderPanel.SetActive(false);

        Time.timeScale = 1f;

        if (currentStudent != null)
        {
            currentStudent
                .GetComponent<StudentFollow>()
                .Leave();

            currentStudent = null;
        }
    }
}