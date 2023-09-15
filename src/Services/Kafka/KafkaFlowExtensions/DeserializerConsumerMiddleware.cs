using KafkaFlow;

namespace Microsoft.eShopOnContainers.Kafka.KafkaFlowExtensions;

public class DeserializerConsumerMiddleware : IMessageMiddleware
{
    private readonly ISerializer _serializer;
    private readonly IAsyncMessageTypeResolver _typeResolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializerConsumerMiddleware"/> class.
    /// </summary>
    /// <param name="serializer">Instance of <see cref="ISerializer"/></param>
    /// <param name="typeResolver">Instance of <see cref="IAsyncMessageTypeResolver"/></param>
    public DeserializerConsumerMiddleware(
        ISerializer serializer,
        IAsyncMessageTypeResolver typeResolver)
    {
        this._serializer = serializer;
        this._typeResolver = typeResolver;
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

        var messageType = await this._typeResolver.OnConsumeAsync(context);

        if (messageType is null)
        {
            await next(context).ConfigureAwait(false);
            return;
        }

        using var stream = new MemoryStream(rawData);

        var data = await this._serializer
            .DeserializeAsync(
                stream,
                messageType,
                new SerializerContext(context.ConsumerContext.Topic))
            .ConfigureAwait(false);

        await next(context.SetMessage(context.Message.Key, data)).ConfigureAwait(false);
    }
}
