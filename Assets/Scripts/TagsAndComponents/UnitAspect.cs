using Unity.Entities;

public readonly partial struct UnitAspect : IAspect
{
    public readonly Entity entity;
    readonly RefRO<UnitProperties> _properties;
    private readonly RefRW<UnitHealth> health;
    private readonly DynamicBuffer<DamageBuffer> damageBuffer;

    public float speed => _properties.ValueRO.speed;

    public void DamageUnit()
    {
        foreach(var damageBuffer in damageBuffer)
        {
            health.ValueRW.Value -= damageBuffer.Value;
        }

        damageBuffer.Clear();
    }
}