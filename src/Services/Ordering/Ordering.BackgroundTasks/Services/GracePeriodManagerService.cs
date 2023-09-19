using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.Kafka.Producers;
using Microsoft.Extensions.Options;
using Npgsql;
using Ordering.BackgroundTasks.Events;

namespace Ordering.BackgroundTasks.Services
{
    public class GracePeriodManagerService : BackgroundService
    {
        private readonly ILogger<GracePeriodManagerService> _logger;
        private readonly BackgroundTaskSettings _settings;
        private readonly IEShopOnContainersProducer _producer;

        public GracePeriodManagerService(IOptions<BackgroundTaskSettings> settings, IEShopOnContainersProducer producer, ILogger<GracePeriodManagerService> logger)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _producer = producer ?? throw new ArgumentNullException(nameof(producer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("GracePeriodManagerService is starting.");

            stoppingToken.Register(() => _logger.LogDebug("#1 GracePeriodManagerService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("GracePeriodManagerService background task is doing background work.");

                CheckConfirmedGracePeriodOrders();
                await Task.Delay(_settings.CheckUpdateTime, stoppingToken);
            }

            _logger.LogDebug("GracePeriodManagerService background task is stopping.");
        }

        private void CheckConfirmedGracePeriodOrders()
        {
            _logger.LogDebug("Checking confirmed grace period orders");

            var orderIds = GetConfirmedGracePeriodOrders();

            foreach (var orderId in orderIds)
            {
                var confirmGracePeriodEvent = new GracePeriodConfirmedIntegrationEvent(orderId);

                _logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", confirmGracePeriodEvent.Key, confirmGracePeriodEvent);

                _producer.Produce(confirmGracePeriodEvent);
            }
        }

        private IEnumerable<int> GetConfirmedGracePeriodOrders()
        {
            IEnumerable<int> orderIds = new List<int>();

            using var conn = new NpgsqlConnection(_settings.ConnectionString);
            try
            {
                conn.Open();
                orderIds = conn.Query<int>(
                    @"SELECT id FROM orders 
                        WHERE ((DATE_PART('Day', NOW() - order_date) * 24 +
                              DATE_PART('Hour', NOW() - order_date)) * 60 + 
                              DATE_PART('Minute', NOW() - order_date)) >= @GracePeriodTime
                        AND order_status_id = 1",
                    new { _settings.GracePeriodTime });
            }
            catch (NpgsqlException exception)
            {
                _logger.LogCritical(exception, "Fatal error establishing database connection");
            }


            return orderIds;
        }
    }
}
