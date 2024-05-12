using Unity.Entities;
using Unity.Mathematics;

public struct BattlefieldProperties : IComponentData
{
    public Entity prefab;
    public float3 position;
    public bool enemy;
}
