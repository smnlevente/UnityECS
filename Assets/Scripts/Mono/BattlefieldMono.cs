using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class BattlefieldMono : MonoBehaviour
{
    public GameObject prefab;
    public Transform tform;
    public bool enemy;
}

public class BattlefieldBaker : Baker<BattlefieldMono>
{
    public override void Bake(BattlefieldMono authoring)
    {
        var battlefieldEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(battlefieldEntity, new BattlefieldProperties
        {

            prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
            position = new float3(authoring.tform.position.x, authoring.tform.position.y, authoring.tform.position.z),
            enemy = authoring.enemy,
        });
    }
}
