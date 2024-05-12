using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text tmpText;

    public void UpdateText(float amount)
    {
        tmpText.text = (math.ceil(amount)).ToString();
    }
}
