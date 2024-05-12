using Unity.Entities;

public struct UnitProperties : IComponentData
{
    public float speed;
    public Entity entity;
}

public struct UnitWalkProperties : IComponentData, IEnableableComponent
{
    public float speed;
}

public struct UnitTag : IComponentData { }
public struct FriendlyUnitTag : IComponentData { }
public struct  EnemyUnitTag : IComponentData { }

public struct NewUnitTag : IComponentData { }
public struct TargetEntity : IComponentData
{
    public Entity Value;
}

