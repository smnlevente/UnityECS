using Unity.Entities;

public struct UnitFightProperty : IComponentData, IEnableableComponent
{
    public float DPS;
    public float Range;
}
