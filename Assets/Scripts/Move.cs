using UnityEngine;

public class PhysicsPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 15f;
    public float deceleration = 20f;


    [Header("Jump")]
    public float jumpForce = 12f;
    public LayerMask groundLayer;

    [Header("Visuals")]
    public SpriteRenderer characterSprite;
    public Sprite jumpStartSprite; // Спрайт при начале прыжка
    public Sprite fallSprite;     // Спрайт при падении
    public Sprite idleSprite;     // Спрайт при стоянии

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private bool isGrounded;
    private float verticalVelocity;
    private const float GRAVITY = 9.81f;
    private bool isFacingRight = true;
    private bool wasGrounded = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        rb.gravityScale = 0; // Отключаем встроенную гравитацию
    }

    private void Update()
    {
        // Проверка земли
        wasGrounded = isGrounded;
        isGrounded = Physics2D.BoxCast(
            col.bounds.center,
            col.bounds.size,
            0f,
            Vector2.down,
            0.1f,
            groundLayer
        );

        // Прыжок
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            verticalVelocity = jumpForce;
            characterSprite.sprite = jumpStartSprite; // Устанавливаем спрайт прыжка
        }
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            characterSprite.sprite = fallSprite;
        }
        // Возврат к idle при приземлении
        else if (isGrounded && !wasGrounded)
        {
            characterSprite.sprite = idleSprite;
        }
    }

    private void FixedUpdate()
    {
        // Горизонтальное движение (V = V0 + at)
        float moveInput = Input.GetAxisRaw("Horizontal");
        float targetSpeed = moveInput * moveSpeed;
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float horizontalVelocity = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accelerationRate * Time.fixedDeltaTime);

        // Вертикальное движение с гравитацией (V = V0 + at)
        if (!isGrounded)
        {
            verticalVelocity -= GRAVITY * Time.fixedDeltaTime; // a = g
        }
        else if (verticalVelocity < 0)
        {
            verticalVelocity = 0;
        }

        if (Mathf.Abs(horizontalVelocity) > 0.1f)
        {
            bool shouldFaceRight = horizontalVelocity > 0;
            if (shouldFaceRight != isFacingRight)
            {
                FlipCharacter();
            }
        }

        // Применяем скорость
        rb.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);
    }

    private void FlipCharacter()
    {
        isFacingRight = !isFacingRight;
        characterSprite.flipX = !isFacingRight;

    }

}