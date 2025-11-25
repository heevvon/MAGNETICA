using UnityEngine;
using TMPro; // TextMeshPro 사용

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text scoreText;       // Canvas에 연결된 Score Text

    [Header("Score Settings")]
    private int score = 0;           // 실제 점수
    private int displayScore = 0;    // 화면에 표시되는 점수
    public float countSpeed = 500f;  // 1초에 표시되는 점수 증가 속도
    public float scoreInterval = 1f; // 점수 자동 증가 간격 (초)
    public int scorePerInterval = 10; // 매 간격마다 증가하는 점수

    private float timer = 0f;
    private float displayScoreFloat = 0f;

    void Start()
    {
        // 초기화
        score = 0;
        displayScore = 0;
        UpdateScoreText();
    }

    void Update()
    {
        // 시간 기반으로 점수 증가
        timer += Time.deltaTime;
        if (timer >= scoreInterval)
        {
            AddScore(scorePerInterval);
            timer = 0f;
        }

        // displayScore가 score를 따라가도록 애니메이션
        if (displayScoreFloat < score)
    {
        displayScoreFloat += countSpeed * Time.deltaTime;
        if (displayScoreFloat > score)
            displayScoreFloat = score;

        displayScore = Mathf.FloorToInt(displayScoreFloat); // 화면에 표시되는 점수
        UpdateScoreText();
    }
    }

    // UI 텍스트 업데이트
    void UpdateScoreText()
    {
        // 오른쪽 정렬 + 고정 자리수 9자리
        scoreText.text = displayScore.ToString("D9");
    }

    // 점수 추가 함수
    public void AddScore(int amount)
    {
        score += amount;
    }

    // 점수 초기화
    public void ResetScore()
    {
        score = 0;
        displayScore = 0;
        UpdateScoreText();
    }
}
