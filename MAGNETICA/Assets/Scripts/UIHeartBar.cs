using UnityEngine;
using UnityEngine.UI;

public class UIHeartBar : MonoBehaviour
{
    public Image[] hearts;      // 5개의 하트 이미지
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public void UpdateHearts(float health)
    {
        float hp = health;  // 체력 복사

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hp >= 1f)
            {
                hearts[i].sprite = fullHeart;
                hp -= 1f;
            }
            else if (hp >= 0.5f)
            {
                hearts[i].sprite = halfHeart;
                hp -= 0.5f;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
