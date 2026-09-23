using UnityEngine;
using TMPro;

public class StudentFollow : MonoBehaviour
{
    public Transform player;

    public float speed = 2f;
    public float leaveSpeed = 4f;
    public float despawnLimit = 10f;

    public TMP_Text requestBubbleText;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveDirection;
    private bool isLeaving = false;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (transform.position.x > 0)
        {
            moveDirection = Vector2.left;
        }
        else
        {
            moveDirection = Vector2.right;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX =
                moveDirection == Vector2.right;
        }

        requestedCharacter =
            Random.Range(0, characterNames.Length);

        if (requestBubbleText != null)
        {
            requestBubbleText.text =
                "SHOW ME " +
                characterNames[requestedCharacter] +
                "!";

            requestBubbleText.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        float currentSpeed =
            isLeaving ? leaveSpeed : speed;

        rb.linearVelocity =
            moveDirection * currentSpeed;

        if (moveDirection == Vector2.left &&
            transform.position.x <= -despawnLimit)
        {
            Destroy(gameObject);
        }

        if (moveDirection == Vector2.right &&
            transform.position.x >= despawnLimit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLeaving)
        {
            rb.linearVelocity = Vector2.zero;

            if (requestBubbleText != null)
            {
                requestBubbleText.gameObject.SetActive(true);
            }

            BinderManager binderManager =
                FindAnyObjectByType<BinderManager>();

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
    }
}