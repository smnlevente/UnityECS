using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [SerializeField]
    private HealthBarController prefab;

    public Dictionary<int, HealthBarController> healthbarDictionary = new Dictionary<int, HealthBarController>();

    public void SpawnHealthbar(int unit, float maxHealthValue)
    {
        healthbarDictionary[unit] = Instantiate<HealthBarController>(prefab, this.transform);
        healthbarDictionary[unit].transform.position = new Vector3(200, 0, 200);
    }

    public void Reset()
    {
        foreach(HealthBarController healthBar in healthbarDictionary.Values)
        {
            healthBar.transform.position = new Vector3(200, 0, 200);
            healthBar.gameObject.SetActive(true);
        }
    }
}
