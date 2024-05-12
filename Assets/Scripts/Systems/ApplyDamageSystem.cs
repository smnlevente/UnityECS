using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
[UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
public partial struct ApplyDamageSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Dependency.Complete();
        foreach (var unit in SystemAPI.Query<UnitAspect>())
        {
            unit.DamageUnit();
        }
    }
}
