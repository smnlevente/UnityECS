using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[BurstCompile]
public partial class AliveSystem : SystemBase
{
    [BurstCompile]
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (transform, health, entity) in SystemAPI.Query<RefRO<LocalTransform>,UnitHealth>().WithEntityAccess())
        {
            var healthbar = UIController.Instance.healthbarDictionary[entity.Index];
            var healthbarTransform = healthbar.transform;
            healthbarTransform.position = transform.ValueRO.Position;
            healthbarTransform.position += 1.5f * (Vector3)transform.ValueRO.Up();
            healthbarTransform.rotation = transform.ValueRO.Rotation;
            healthbar.UpdateText(health.Value);

            if (health.Value <= 0f)
            {
                healthbar.gameObject.SetActive(false);
                foreach (var (unit, target) in SystemAPI.Query<UnitProperties, TargetEntity>())
                {
                    if (target.Value == entity)
                    {
                        ecb.RemoveComponent<TargetEntity>(unit.entity);
                    }
                }

                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(EntityManager);
    }
}

