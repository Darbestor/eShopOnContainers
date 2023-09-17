using System.Globalization;
using Confluent.SchemaRegistry;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using KafkaFlow;

namespace Microsoft.eShopOnContainers.Kafka.Consumers;

public class ProtobufTypeNameResolver : IAsyncSchemaRegistryTypeNameResolver
{
    private readonly ISchemaRegistryClient client;

    public ProtobufTypeNameResolver(ISchemaRegistryClient client)
    {
        this.client = client;
    }

    public async Task<string> ResolveAsync(int id)
    {
        var schemaString = (await this.client.GetSchemaAsync(id, "serialized")).SchemaString;

        var protoFields = FileDescriptorProto.Parser.ParseFrom(ByteString.FromBase64(schemaString));

        string packageName;
        if (protoFields.Options is not null && protoFields.Options.HasCsharpNamespace)
        {
            packageName = protoFields.Options.CsharpNamespace;
        }
        else
        {
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            packageName = textInfo.ToTitleCase(protoFields.Package);
        }

        return $"{packageName}.{protoFields.MessageType.FirstOrDefault()?.Name}";
    }
}
