using System.Text.Json;
using NotifyService.Models;
using StackExchange.Redis;

namespace NotifyService.src.Managers
{
    public class NotifyManager : INotifyService
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly RedisChannel channel = "user-card-events-";

        public NotifyManager(IConnectionMultiplexer connection)
        {
            _connection = connection;
        }

        public async IAsyncEnumerable<SSEEvent> ProduceEvents(string id)
        {
            var subscriber = _connection.GetSubscriber();
            var redisChannel = channel + id;

            var subscription = subscriber.Subscribe(redisChannel);

            while (true)
            {
                var message = await subscription.ReadAsync();
                var eventData = JsonSerializer.Deserialize<EventModel>(message.Message);
                yield return new SSEEvent(eventData.OrderType, eventData);
            }
        }
    }
}