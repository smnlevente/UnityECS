using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]

[BurstCompile]
public partial class UnitSpawnSystem : SystemBase
{
    [BurstCompile]
    protected override void OnUpdate()
    {
        var buffer = new EntityCommandBuffer(Allocator.Temp);
        foreach (BattlefieldProperties battlefield in SystemAPI.Query<BattlefieldProperties>())
        {
            SpawnPlayer(buffer, ConfigurationLoader.Instance.settings, battlefield, battlefield.enemy);
            this.Enabled = false;
        }

        buffer.Playback(EntityManager);

        foreach(var (unit, health) in SystemAPI.Query<UnitProperties, UnitHealth>())
        {
            UIController.Instance.SpawnHealthbar(unit.entity.Index, health.Max);
        }
    }

    private void SpawnPlayer(EntityCommandBuffer ecb, ConfigurationLoader.Configuration.BattlefieldSettings settings, BattlefieldProperties properties, bool isEnemy)
    {
        var player = isEnemy ? ConfigurationLoader.Instance.enemy : ConfigurationLoader.Instance.player;  
        for (int i = 0; i < player.xGridCount; i++)
        {
            for (int j = 0; j < player.yGridCount; j++)
            {
                if (player.teamStructure[(i* player.xGridCount )+ j] == 0)
                {
                    continue;
                }

                Entity unit = ecb.Instantiate(properties.prefab);
                float3 position = new float3(i * settings.xPadding, settings.offset, j * settings.yPadding);
                position += properties.position;
                ecb.SetComponent(unit, new LocalTransform { Position = position, Rotation = quaternion.identity, Scale = 1f });
                ecb.AddComponent(unit, new UnitProperties
                {
                    speed = player.unitSpeed,
                    entity = unit
                });


                ecb.AddComponent(unit, new UnitWalkProperties
                {
                    speed = player.unitSpeed,
                });
                ecb.AddComponent(unit, new UnitHealth
                {
                    Value = player.unitHealth,
                    Max = player.unitHealth,
                });

                ecb.AddComponent(unit, new UnitFightProperty
                {
                    DPS = player.unitDPS,
                    Range = player.unitRange,
                });
                if (isEnemy)
                {
                    ecb.AddComponent<EnemyUnitTag>(unit);

                }
                else
                {
                    ecb.AddComponent<FriendlyUnitTag>(unit);
                }

                ecb.AddComponent<NewUnitTag>(unit);
                ecb.AddBuffer<DamageBuffer>(unit);
                ecb.AddComponent<UnitTag>(unit);
            }
        }
    }
}
