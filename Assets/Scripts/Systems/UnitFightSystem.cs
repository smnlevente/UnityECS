using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(TransformSystemGroup))]
public partial struct UnitFightSystem : ISystem
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
        var deltaTime = SystemAPI.Time.DeltaTime;
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (transform, unitFight, target) in SystemAPI.Query<RefRW<LocalTransform>, UnitFightProperty, TargetEntity>())
        {
            float3 targetPosition = SystemAPI.GetComponent<LocalToWorld>(target.Value).Position;
            if (math.distancesq(transform.ValueRO.Position, targetPosition) >= unitFight.Range)
            {
                continue;
            }

            var damage = unitFight.DPS * deltaTime;
            var currentDamage = new DamageBuffer { Value = damage };
            ecb.AppendToBuffer(target.Value, currentDamage);
        }
    }
}
