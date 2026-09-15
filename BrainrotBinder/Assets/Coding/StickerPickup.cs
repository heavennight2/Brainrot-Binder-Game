using UnityEngine;
using System.Collections;

public class StickerPickup : MonoBehaviour
{
    public float extraTime = 5f;
    public float lifetime = 4f;

    private void Start()
    {
        StartCoroutine(DisappearAfterTime());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameTimer gameTimer =
                FindFirstObjectByType<GameTimer>();

            gameTimer.AddTime(extraTime);

            Destroy(gameObject);
        }
    }

    private IEnumerator DisappearAfterTime()
    {
        yield return new WaitForSecondsRealtime(lifetime);

        Destroy(gameObject);
    }
}