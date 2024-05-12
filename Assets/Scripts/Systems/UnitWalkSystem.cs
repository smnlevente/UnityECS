using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(TransformSystemGroup))]
[UpdateAfter(typeof(FindTargetSystem))]
public partial struct UnitWalkSystem : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UnitProperties>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, unit, walk, target) in SystemAPI.Query<RefRW<LocalTransform>, UnitProperties, UnitWalkProperties, TargetEntity>())
        {
            if (!SystemAPI.Exists(target.Value))
            {
                ecb.RemoveComponent<TargetEntity>(unit.entity);
                continue;
            }

            float3 targetPosition = SystemAPI.GetComponent<LocalToWorld>(target.Value).Position;
            if (math.distancesq(transform.ValueRO.Position, targetPosition) < 3f)
            {
                ecb.SetComponentEnabled<UnitFightProperty>(unit.entity, true);
                continue;
            }

            transform.ValueRW.Position = transform.ValueRO.Position + (transform.ValueRO.Forward() * walk.speed * deltaTime);
            transform.ValueRW.Rotation = quaternion.LookRotation((targetPosition - transform.ValueRO.Position), transform.ValueRO.Up());
        }
    }
}
