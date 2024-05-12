using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class UISystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireForUpdate<UnitTag>();
    }

    protected override void OnUpdate()
    {

        var friendlyCount = GetEntityQuery(ComponentType.ReadOnly<FriendlyUnitTag>()).CalculateEntityCount();
        var enemyCount = GetEntityQuery(ComponentType.ReadOnly<EnemyUnitTag>()).CalculateEntityCount();

        if (enemyCount == 0 || friendlyCount == 0)
        {
            MainMenuController.Instance.ShowEnd(enemyCount == 0);
        }
    }
}