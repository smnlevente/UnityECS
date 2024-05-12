using Unity.Entities;

[InternalBufferCapacity(8)]
public struct DamageBuffer : IBufferElementData
{
    public float Value;
}
