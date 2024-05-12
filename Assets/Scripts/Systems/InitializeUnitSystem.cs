using Unity.Burst;
using Unity.Entities;
using Unity.Collections;

[BurstCompile]
public partial struct InitializeUnitSystem : ISystem
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
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var unit in SystemAPI.Query<UnitProperties>().WithAll<NewUnitTag>())
        {
            ecb.RemoveComponent<NewUnitTag>(unit.entity);
            ecb.SetComponentEnabled<UnitWalkProperties>(unit.entity, false);
        }

        ecb.Playback(state.EntityManager);
    }
}
