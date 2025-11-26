using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip startSfx;
    public AudioClip exitSfx;

    // 메인 씬으로 넘어가기
    public void StartGame()
    {
        Time.timeScale = 1.0f;
        PlaySfx(startSfx);
        SceneManager.LoadScene("Main");   // 씬 이름은 너가 사용하는 이름으로 변경 가능
    }

    public void Update()
    {
        // ⚡ R 키 누르면 메인 메뉴로 복귀
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartGame();
        }
    }

    // 게임 종료
    public void ExitGame()
    {
        PlaySfx(exitSfx);
        Application.Quit();

#if UNITY_EDITOR
        // 유니티 에디터에서 종료 시 플레이모드 멈추기
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // 버튼 클릭 사운드 재생
    void PlaySfx(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
