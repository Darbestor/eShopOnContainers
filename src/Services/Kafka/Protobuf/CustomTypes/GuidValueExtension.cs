namespace Microsoft.eShopOnContainers.Services.Kafka.Protobuf;

public partial class UuidValue
{
    public UuidValue(string guid)
    {
        Uuid = guid;
    }

    public static implicit operator Guid(UuidValue grpcUuid)
    {
        return Guid.Parse(grpcUuid.Uuid);
    }

    public static implicit operator UuidValue(Guid value)
    {
        return new UuidValue(value.ToString());
    }
}
