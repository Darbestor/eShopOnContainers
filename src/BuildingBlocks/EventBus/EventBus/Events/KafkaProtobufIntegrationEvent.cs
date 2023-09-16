namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

public class KafkaIntegrationEvent
{
    public string Topic { get; private init; }
    public string Key { get; private init; }
    public object Message { get; private init; }

    public IEnumerable<KeyValuePair<string, string>> Headers { get; set; }

    public KafkaIntegrationEvent(string topic, string key, object message, IEnumerable<KeyValuePair<string, string>>? headers = null)
    {
        Topic = topic ?? throw new ArgumentNullException(nameof(topic));
        Key = key;
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Headers = headers ?? Array.Empty<KeyValuePair<string, string>>();
    }
    
    public void Deconstruct(out string topic, out string key, out object message,
        out IEnumerable<KeyValuePair<string, string>> headers)
    {
        topic = Topic;
        key = Key;
        message = Message;
        headers = Headers;
    }
}
