using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(TransformSystemGroup))]
public partial struct FindTargetSystem : ISystem
{
    private EntityQuery friendQuery;
    private EntityQuery enemyQuery;

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
        var targetFriendliesQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform>().WithAll<UnitProperties>().WithAll<FriendlyUnitTag>().Build();
        var targetEnemiesQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform>().WithAll<UnitProperties>().WithAll<EnemyUnitTag>().Build();

        var targetFriendlyEntities = targetFriendliesQuery.ToEntityArray(state.WorldUpdateAllocator);
        var targetEnemyEntities = targetEnemiesQuery.ToEntityArray(state.WorldUpdateAllocator);
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

        var jobFindFriendly = new FindFriendlyJob
        {
            TargetEntities = targetFriendlyEntities,
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
        };

        var jobFindEnemy = new FindEnemyJob
        {
            TargetEntities = targetEnemyEntities,
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
        };


        friendQuery = SystemAPI.QueryBuilder().WithAll<UnitProperties>().WithAll<LocalTransform>().WithAll<FriendlyUnitTag>().WithNone<TargetEntity>().Build();

        enemyQuery = SystemAPI.QueryBuilder().WithAll<UnitProperties>().WithAll<LocalTransform>().WithAll<EnemyUnitTag>().WithNone<TargetEntity>().Build();
        state.Dependency = jobFindFriendly.ScheduleParallel(enemyQuery, state.Dependency);
        state.Dependency = jobFindEnemy.ScheduleParallel(friendQuery, state.Dependency);
    }

    [BurstCompile]
    public partial struct FindEnemyJob : IJobEntity
    {
        [ReadOnly] public NativeArray<Entity> TargetEntities;
        public EntityCommandBuffer.ParallelWriter ECB;

        [BurstCompile]
        public void Execute(in UnitProperties properties, in LocalTransform transform, in FriendlyUnitTag friendly, [EntityIndexInQuery] int sortKey)
        { 
            if (TargetEntities.Length <= 0)
            {
                return;
            }

            Unity.Mathematics.Random random = new Unity.Mathematics.Random(123);
            int ran = random.NextInt(0, TargetEntities.Length);
            ECB.AddComponent(sortKey, properties.entity, new TargetEntity
            {
                Value = TargetEntities[ran]
            });
        }
    }

    [BurstCompile]
    public partial struct FindFriendlyJob : IJobEntity
    {
        [ReadOnly] public NativeArray<Entity> TargetEntities;
        public EntityCommandBuffer.ParallelWriter ECB;

        [BurstCompile]
        public void Execute(in UnitProperties properties, in LocalTransform transform, in EnemyUnitTag enemy, [EntityIndexInQuery] int sortKey)
        {
            if (TargetEntities.Length <= 0)
            {
                return;
            }

            Unity.Mathematics.Random random = new Unity.Mathematics.Random(123);
            int ran = random.NextInt(0, TargetEntities.Length);
            ECB.AddComponent(sortKey, properties.entity, new TargetEntity
            {
                Value = TargetEntities[ran]
            });
        }
    }
}
