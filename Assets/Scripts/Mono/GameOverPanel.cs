using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text victoryText;
    [SerializeField]
    private GameObject content;

    public void SetUp(bool playerWon)
    {
        victoryText.text = playerWon ? "Victory, you won!" : "You lost";
        content.SetActive(true);
    }

    public void Click()
    {
        content.SetActive(false);
        MainMenuController.Instance.Restart();
    }
}
