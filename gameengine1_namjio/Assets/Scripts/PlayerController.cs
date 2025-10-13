using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5.0f;

    [Header("점프 설정")]
    public float jumpForce = 10.0f;

    // Animator 컴포넌트 참조 (private - Inspector에 안 보임)
    private Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded = false; // 바닥에 닿아있는지 여부

    void Start()
    {
        // Animator 컴포넌트 찾기
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // 디버그: Animator가 제대로 연결되었는지 확인
        if (animator != null)
        {
            Debug.Log("Animator 컴포넌트를 찾았습니다!");
        }
        else
        {
            Debug.LogError("Animator 컴포넌트가 없습니다!");
        }

        // 디버그: Rigidbody2D가 제대로 연결되었는지 확인
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D가 없습니다! Player에 추가하세요.");
        }
    }

    void Update()
    {
        // 좌우 이동
        float moveX = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveX += 1f;  // 왼쪽으로 이동
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveX -= 1f;   // 오른쪽으로 이동
        }

        // 이동 적용: Rigidbody2D를 사용해 물리적으로 이동
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // 애니메이션 처리
        if (moveX != 0)
        {
            // 이동 중이면 "Run" 애니메이션 활성화
            animator.SetFloat("Speed", Mathf.Abs(moveX));
            
            // 방향 전환 (스프라이트 좌우 반전)
            if (moveX < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // 왼쪽으로 갈 때
            }
            else if (moveX > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);  // 오른쪽으로 갈 때
            }
        }
        else
        {
            // 이동하지 않으면 "Idle" 애니메이션 (속도 0)
            animator.SetFloat("Speed", 0);
        }

        // 점프 입력 감지
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // 점프 애니메이션 실행
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("Jump", true);
            Debug.Log("점프!");
        }
    }

    // 바닥에 닿았을 때 (충돌 시작)
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌 시작: " + collision.gameObject.name);
        isGrounded = true;
        animator.SetBool("Jump", false);  // 점프 상태가 아니면 "Jump" 애니메이션을 종료
    }

    // 바닥에서 떨어졌을 때 (충돌 종료)
    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("충돌 종료: " + collision.gameObject.name);
        isGrounded = false;
    }
}
