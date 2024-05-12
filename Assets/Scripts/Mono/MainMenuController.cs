using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class MainMenuController : Singleton<MainMenuController>
{
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private GameOverPanel gameOverPanel;
    [SerializeField]
    private TeamPresetView teamPresetButtonPrefab;
    [SerializeField]
    private GameObject scrollViewContent;

    private void Start()
    {
        populatePremadeTeams(ConfigurationLoader.Instance.teamPresets);
    }

    private void populatePremadeTeams(ConfigurationLoader.Configuration.TeamPresets[] teamPresets)
    {
        for (int i = 0; i < teamPresets.Length; i++)
        {
            TeamPresetView button = GameObject.Instantiate<TeamPresetView>(teamPresetButtonPrefab, scrollViewContent.transform);
            button.transform.position = new Vector3(button.transform.position.x, scrollViewContent.transform.position.y, button.transform.position.z);
            button.SetUp(i);
        }
    }
    
    public void ClickStartBattle()
    {
        var query = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<UnitProperties>());
        var entities = query.ToEntityArray(Allocator.Temp);
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var entity in entities)
        {
            ecb.SetComponentEnabled<UnitWalkProperties>(entity, true);

        }
        ecb.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);

        this.content.gameObject.SetActive(false);
    }

    public void ShowEnd(bool won)
    {
        gameOverPanel.SetUp(won);
    }

    public void Restart()
    {
        var query = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<UnitProperties>());
        var entities = query.ToEntityArray(Allocator.Temp);
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var entity in entities)
        {
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);

        World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UnitSpawnSystem>().Enabled = true;
        UIController.Instance.Reset();
        this.content.gameObject.SetActive(true);
    }
}
