using UnityEngine;
using TMPro;

public class WinZone : MonoBehaviour
{
    public TextMeshProUGUI winText;

    void Start()
    {
        // Скрываем текст при старте
        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что в зону вошел игрок
        if (other.CompareTag("Player"))
        {
            // Показываем текст победы
            if (winText != null)
            {
                winText.gameObject.SetActive(true);
                winText.text = "You win!";
            }
        }
    }
}