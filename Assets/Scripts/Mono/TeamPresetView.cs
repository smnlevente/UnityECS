using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamPresetView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text buttonText;

    [SerializeField]
    private Button button;

    public void SetUp(int teamIndex)
    { 
        this.buttonText.text = $"Team {teamIndex+1}";
        button.onClick.AddListener(() => OnClicked(teamIndex));
    }

    public void OnClicked(int index)
    {
        ConfigurationLoader.Instance.enemy.teamStructure = ConfigurationLoader.Instance.teamPresets[index].team;
        MainMenuController.Instance.Restart();
    }
}
