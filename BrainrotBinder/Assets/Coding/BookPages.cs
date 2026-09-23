using UnityEngine;

public class BookPages : MonoBehaviour
{
    public GameObject[] pages;

    private int currentPage = 0;

    private void Start()
    {
        ResetPages();
    }

    public void NextPage()
    {
        currentPage++;

        if (currentPage >= pages.Length)
        {
            currentPage = 0;
        }

        ShowPage(currentPage);
    }

    public void PreviousPage()
    {
        currentPage--;

        if (currentPage < 0)
        {
            currentPage = pages.Length - 1;
        }

        ShowPage(currentPage);
    }

    public void ResetPages()
    {
        currentPage = 0;
        ShowPage(currentPage);
    }

    private void ShowPage(int pageNumber)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == pageNumber);
        }
    }
}