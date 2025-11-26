using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class UIManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip Settingsfx;
    public AudioClip restartsfx;
    public AudioClip exitsfx;
    public GameObject settingPanel;
    public KeyCode toggleKey = KeyCode.Escape;

    private bool isOpen = false;

    public PlayerController playerController;

    public static UIManager Instance;
    public UIHeartBar heartBar;

    void Start()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            PlaySfx(Settingsfx);
            ToggleSettingPanel();
        }
    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ToggleSettingPanel()
    {
        isOpen = !isOpen;
        settingPanel.SetActive(isOpen);

        if (isOpen)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        PlaySfx(exitsfx);
        SceneManager.LoadScene("Start");
    }

    public void RestartGame()
    {
        PlaySfx(restartsfx);
        playerController.Restart();
    }

    void PlaySfx(AudioClip clip)
    {
        if (audioSource != null && clip != null) 
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void UpdateHeartUI(float health)
    {
        heartBar.UpdateHearts(health);
    }

}
