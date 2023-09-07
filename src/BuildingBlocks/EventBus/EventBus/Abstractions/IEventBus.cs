namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;

/// <summary>
/// Абстракция для общего интерфейса
/// брокеров сообщений
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Отправка события
    /// </summary>
    /// <param name="event"></param>
    void Publish(IntegrationEvent @event);

    /// <summary>
    /// Подписка на интересующее событие
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TH"></typeparam>
    void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    void Unsubscribe<T, TH>()
        where TH : IIntegrationEventHandler<T>
        where T : IntegrationEvent;
}
