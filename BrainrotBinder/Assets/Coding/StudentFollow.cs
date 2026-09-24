using UnityEngine;
using TMPro;

public class StudentFollow : MonoBehaviour
{
    public Transform player;

    public float speed = 2f;
    public float leaveSpeed = 4f;
    public float despawnLimit = 10f;

    public TMP_Text requestBubbleText;

    // حطي هنا Animator Controllers حق شخصيات الطلاب
    public RuntimeAnimatorController[] studentAnimators;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

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
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // يختار شكل طالب عشوائي
        if (animator != null &&
            studentAnimators != null &&
            studentAnimators.Length > 0)
        {
            int randomStudent =
                Random.Range(0, studentAnimators.Length);

            animator.runtimeAnimatorController =
                studentAnimators[randomStudent];
        }

        // يبدأ يمشي باتجاه داخل الفصل
        if (transform.position.x > 0)
        {
            moveDirection = Vector2.left;
        }
        else
        {
            moveDirection = Vector2.right;
        }

        UpdateSpriteDirection();

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

        // يحذف الطالب إذا خرج بعيد
        if (transform.position.x <= -despawnLimit ||
            transform.position.x >= despawnLimit)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // إذا صقع في عائق
        if (collision.gameObject.CompareTag("Obstacle") &&
            !isLeaving)
        {
            ChooseNewDirection();
        }
    }

    private void ChooseNewDirection()
    {
        Vector2[] directions =
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        Vector2 newDirection = moveDirection;

        // يحاول يختار اتجاه مختلف عن الحالي
        while (newDirection == moveDirection)
        {
            newDirection =
                directions[Random.Range(0, directions.Length)];
        }

        moveDirection = newDirection;

        UpdateSpriteDirection();
    }

    private void UpdateSpriteDirection()
    {
        if (spriteRenderer == null)
            return;

        if (moveDirection == Vector2.right)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDirection == Vector2.left)
        {
            spriteRenderer.flipX = false;
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

            if (binderManager != null)
            {
                binderManager.OpenBinder(
                    gameObject,
                    requestedCharacter
                );
            }
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