using UnityEngine;
using UnityEngine.SceneManagement;

public enum Polarity { N, S }

public class PlayerController : MonoBehaviour
{ 
    [Header("Move Settings")]
    public float runSpeed = 25f;

    [Header("Speed Settings")]
    public float speedIncreaseInterval = 25f;
    public float speedIncreaseAmount = 3f;
    public float maxRunSpeed = 100f;

    [Header("State")]
    public bool isAlive = true;
    public bool canRun = false;
    public Polarity currentPolarity = Polarity.N;

    [Header("Death Settings")]
    public float minY = -28f;
    public float maxY = 35f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;

    [Header("Components")]
    public PolygonCollider2D poly;
    public SpriteRenderer sr;

    private float speedTimer = 0f;
    private Rigidbody2D rb;
    private Collider2D currentTile = null;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jumpSfx;

    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (rb != null)
        {
            rb.gravityScale = 20f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    private void Update()
    {
        if (!isAlive || !canRun) return;

        // 속도 증가
        speedTimer += Time.deltaTime;
        if (speedTimer >= speedIncreaseInterval)
        {
            runSpeed += speedIncreaseAmount;
            runSpeed = Mathf.Clamp(runSpeed, 0f, maxRunSpeed);
            speedTimer = 0f;
        }

        // Space 입력으로 자성 전환 + 애니메이션 변경
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayJumpSound();
            TogglePolarity();
            ChangeAnimation();
        }

        // Y 한계치 이탈 사망
        if (transform.position.y < minY || transform.position.y > maxY)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        if (!isAlive || !canRun) return;

        // X 방향 이동
        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);
    }

    void TogglePolarity()
    {
        // 자성 전환
        currentPolarity = (currentPolarity == Polarity.N) ? Polarity.S : Polarity.N;

        // 중력 반전
        rb.gravityScale *= -1;

        //반전 후, 표면 보정
        float margin = 0.05f;
        float rayLength = 1f;

        // 위/아래로 레이캐스트 (중력 방향에 따라)
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.up * Mathf.Sign(rb.gravityScale),
            rayLength,
            groundLayer
        );

        if (hit.collider != null)
        {
            currentTile = hit.collider;

            // 캐릭터를 타일 밖으로 이동시키는 보정
            Vector3 pos = transform.position;
            pos.y = hit.point.y - Mathf.Sign(rb.gravityScale) * (GetComponent<Collider2D>().bounds.extents.y + margin);
            transform.position = pos;
        }
        else
        {
            currentTile = null; // 공중
        }
    }

    void ChangeAnimation()
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Run_N Animation"))
            animator.SetTrigger("ChangeToS");
        else if (state.IsName("Run_S Animation"))
            animator.SetTrigger("ChangeToN");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            GetComponent<PlayerHealth>().TakeDamage(1f);
            return;
        }

        // 닿은 타일 저장
        currentTile = collision.collider;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (currentTile == collision.collider)
        {
            currentTile = null;
        }
    }

    public void Die()
    {
        if (!isAlive) return;

        isAlive = false;
        canRun = false;

        Destroy(gameObject);
        SceneManager.LoadScene("GameOver");
    }

    void PlayJumpSound()
    {
        if (audioSource != null && jumpSfx != null)
        {
            audioSource.PlayOneShot(jumpSfx);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
