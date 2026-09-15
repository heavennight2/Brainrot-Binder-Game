using UnityEngine;
using TMPro;

public class StudentFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public TMP_Text requestBubbleText;

    private Rigidbody2D rb;

    private bool isLeaving = false;
    private Vector2 leaveTarget;

    private int requestedCharacter;

 private string[] characterNames =
{
    "TUNG TUNG TUNG SAHUR",
    "BALLERINA CAPPUCCINA",
    "TRALALERO TRALALA",
    "CAPPUCCINO ASSASSINO"
};

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        requestedCharacter =
            Random.Range(0, characterNames.Length);

        requestBubbleText.text =
            "SHOW ME " +
            characterNames[requestedCharacter] +
            "!";
    }

    private void FixedUpdate()
    {
        if (isLeaving)
        {
            Vector2 leaveDirection =
                (leaveTarget - rb.position).normalized;

            rb.linearVelocity = leaveDirection * speed;

            if (Vector2.Distance(rb.position, leaveTarget) < 0.2f)
            {
                Destroy(gameObject);
            }

            return;
        }

        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            ((Vector2)player.position - rb.position).normalized;

        rb.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLeaving)
        {
            BinderManager binderManager =
                FindFirstObjectByType<BinderManager>();

            binderManager.OpenBinder(
                gameObject,
                requestedCharacter
            );
        }
    }

    public void Leave()
    {
        isLeaving = true;

        if (requestBubbleText != null)
        {
            requestBubbleText.gameObject.SetActive(false);
        }

        Collider2D studentCollider =
            GetComponent<Collider2D>();

        if (studentCollider != null)
        {
            studentCollider.enabled = false;
        }

        Vector2 awayDirection =
            ((Vector2)transform.position -
             (Vector2)player.position).normalized;

        if (awayDirection == Vector2.zero)
        {
            awayDirection = Vector2.right;
        }

        leaveTarget =
            (Vector2)transform.position +
            awayDirection * 10f;
    }
}