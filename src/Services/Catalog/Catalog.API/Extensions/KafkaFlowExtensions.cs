using Confluent.SchemaRegistry;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer.SchemaRegistry;

namespace Catalog.API.Extensions;

/// <summary>
/// No needed
/// </summary>
public static class ConsumerConfigurationBuilderExtensions
{
    /// <summary>
    /// Registers a middleware to deserialize protobuf messages using schema registry
    /// </summary>
    /// <param name="middlewares">The middleware configuration builder</param>
    /// <returns></returns>
    public static IConsumerMiddlewareConfigurationBuilder AddSchemaRegistryProtobufCustomSerializer(
        this IConsumerMiddlewareConfigurationBuilder middlewares)
    {
        return middlewares.Add(
            resolver => new DeserializerConsumerMiddleware(
                new ConfluentProtobufSerializer(resolver),
                new SchemaRegistryTypeResolver(
                    new ConfluentProtobufTypeResolver(resolver.Resolve<ISchemaRegistryClient>()))));
    }
}

public class DeserializerConsumerMiddleware : IMessageMiddleware
{
    private readonly ISerializer serializer;
    private readonly IAsyncMessageTypeResolver typeResolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializerConsumerMiddleware"/> class.
    /// </summary>
    /// <param name="serializer">Instance of <see cref="ISerializer"/></param>
    /// <param name="typeResolver">Instance of <see cref="IAsyncMessageTypeResolver"/></param>
    public DeserializerConsumerMiddleware(
        ISerializer serializer,
        IAsyncMessageTypeResolver typeResolver)
    {
        this.serializer = serializer;
        this.typeResolver = typeResolver;
    }
    
    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        if (context.Message.Value is null)
        {
            await next(context).ConfigureAwait(false);
            return;
        }

        if (context.Message.Value is not byte[] rawData)
        {
            throw new InvalidOperationException(
                $"{nameof(context.Message)} must be a byte array to be deserialized and it is '{context.Message.GetType().FullName}'");
        }

        if (rawData.Length == 0)
        {
            await next(context.SetMessage(context.Message.Key, null)).ConfigureAwait(false);
            return;
        }

        var messageType = await this.typeResolver.OnConsumeAsync(context);

        if (messageType is null)
        {
            await next(context).ConfigureAwait(false);
            return;
        }

        using var stream = new MemoryStream(rawData);

        var data = await this.serializer
            .DeserializeAsync(
                stream,
                messageType,
                new SerializerContext(context.ConsumerContext.Topic))
            .ConfigureAwait(false);

        await next(context.SetMessage(context.Message.Key, data)).ConfigureAwait(false);
    }
}


public class ConfluentProtobufTypeResolver : IAsyncSchemaRegistryTypeNameResolver
{
    private readonly ISchemaRegistryClient client;

    public ConfluentProtobufTypeResolver(ISchemaRegistryClient client)
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
