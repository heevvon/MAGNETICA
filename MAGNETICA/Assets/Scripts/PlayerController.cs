using NUnit.Framework.Constraints;
using TreeEditor;
using UnityEngine;

public enum Polarity { N, S }

public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    public float runSpeed = 5f;

    [Header("Speed Settings")]
    public float speedIncreaseInterval = 10f; // 시간 간격
    public float speedIncreaseAmount = 5f;    // 속도 증가량
    public float maxRunSpeed = 100f;          // 속도 최대량

    [Header("State")]
    public bool isAlive = true;
    public bool canRun = false;

    public Polarity currentPolarity = Polarity.N;  // 플레이어 현재 극성 상태

    private float speedTimer = 0f; // 경과 시간 체크용
    private Rigidbody2D rb;

    // 현재 밟고 있는 타일
    private Collider2D currentTile = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (rb != null)
        {
            rb.gravityScale = 20f; // 중력 빠르게 적용
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void Update()
    {
        if (!isAlive || !canRun) return;

        // 일정 시간마다 이동 속도 증가
        speedTimer += Time.deltaTime;
        if (speedTimer >= speedIncreaseInterval)
        {
            runSpeed += speedIncreaseAmount;
            runSpeed = Mathf.Clamp(runSpeed, 0f, maxRunSpeed);
            speedTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePolarityAndFlip();
        }
    }

    void TogglePolarityAndFlip()
    {
        // 자성 전환
        currentPolarity = (currentPolarity == Polarity.N) ? Polarity.S : Polarity.N;

        // 중력 반전
        rb.gravityScale *= -1;

        // 플레이어 뒤집기(y축)
        Vector3 scale = transform.localScale;
        scale.y *= -1;
        transform.localScale = scale;

        // 현재 몸이 어디 붙어있나 확인
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up * Mathf.Sign(rb.gravityScale), 0.2f);
        if (hit.collider != null)
        {
            currentTile = hit.collider;
            Debug.Log("플레이어가 새로운 표면에 붙음: " + currentTile.name);
        }
        else
        {
            currentTile = null; // 떠 있음
        }
    }
    private void FixedUpdate()
    {
        if (!isAlive || !canRun) return;

        // X 방향 자동 이동
        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 타일 충돌 감지
        currentTile = collision.collider;
        // 밟은 타일 이름 출력 (디버깅용)
        Debug.Log("밟은 타일: " + currentTile.name);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (currentTile == collision.collider)
        {
            currentTile = null;
            Debug.Log("타일 벗어남");
        }
    }

    public void Die()
    {
        if (!isAlive) return;
        isAlive = false;
        canRun = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
}
