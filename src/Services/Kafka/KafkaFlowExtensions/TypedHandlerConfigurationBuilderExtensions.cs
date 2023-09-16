using KafkaFlow;
using KafkaFlow.TypedHandler;

namespace Microsoft.eShopOnContainers.Kafka.KafkaFlowExtensions;

public static class TypedHandlerConfigurationBuilderExtensions
{
    public static TypedHandlerConfigurationBuilder AddNoHandlerFoundLogging(this TypedHandlerConfigurationBuilder builder)
    {
        builder.WhenNoHandlerFound(mc =>
        {
            var cc = mc.ConsumerContext;
            Console.Error.WriteLine(
                $"Message handler not found. Message will be skipped: ConsumerName [{cc.ConsumerName}], GroupId [{cc.GroupId}]" +
                $" Topic [{cc.Topic}], Offset [{cc.Offset}], Partition [{cc.Partition}], Message [{mc.Message.Value}]");
            cc.StoreOffset();
        });
        return builder;
    }
}
