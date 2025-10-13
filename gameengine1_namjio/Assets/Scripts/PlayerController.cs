using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5.0f;

    [Header("점프 설정")]
    public float jumpForce = 10.0f;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; 
    
    private int score = 0; 
    private bool isGrounded = false; 

    private float horizontalInput = 0f; // FixedUpdate에서 사용할 입력 값 저장

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    void Update()
    {
        // 1. 입력 감지 및 애니메이션/방향 처리 (이전과 동일)
        horizontalInput = Input.GetAxisRaw("Horizontal"); 

        if (horizontalInput != 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            spriteRenderer.flipX = (horizontalInput < 0);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        // 2. 🟢 점프 입력 및 적용 (Update에서 처리)
        // 작동하던 점프 코드를 그대로 사용합니다.
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Y축 속도에 jumpForce를 부여합니다.
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("Jump", true);
        }
    }
    
    // 📢 FixedUpdate: X축 이동만 처리 (Y축 속도 보호)
    void FixedUpdate()
    {
        // X축 이동 명령을 FixedUpdate에 배치하여 물리 엔진과 동기화합니다.
        // 이 코드는 현재 Y축 속도(점프 중이거나 낙하 중인 속도)를 그대로 유지합니다.
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }
    
    // ... (OnCollisionEnter2D, OnCollisionExit2D, OnTriggerEnter2D는 그대로 유지)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            score++;  
            Debug.Log("코인 획득! 현재 점수: " + score);
            Destroy(other.gameObject);  
        }
    }
}