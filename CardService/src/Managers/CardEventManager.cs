using System;
using CardService.src.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace CardService.src.Managers
{
    public class CardEventManager : ICardEvent
    {
        private readonly IConnectionMultiplexer _redisConn;
        private readonly RedisChannel channel = "user-card-events-";

        public CardEventManager(IConnectionMultiplexer redisConn)
        {
            _redisConn = redisConn;
        }

        public async Task<IEnumerable<EventModel>> GetEventsModel()
        {
            var db = _redisConn.GetDatabase();
            var completedSet = await db.SetMembersAsync("EventsData");

            if (completedSet?.Length > 0)
            {
                return completedSet.Select(val => JsonSerializer.Deserialize<EventModel>(val)).ToList();
            }

            return Enumerable.Empty<EventModel>();
        }

        public async Task SaveCardEvent(string userId, EventModel model, string transactionId)
        {
            //get db connection
            var db = _redisConn.GetDatabase();
            //convert to json
            var jsonData = JsonSerializer.Serialize(model);
            // init operation task list
            var transactionTasks = new List<Task>();

            //imitating some operations
            await Task.Delay(5000);

            //add data to db and return on response user id
            transactionTasks.Add(db.StringSetAsync(transactionId.ToString(), jsonData));
            transactionTasks.Add(db.SetAddAsync("EventsData", jsonData));

            //create user channel
            var userChannel = (channel + userId);
            //publish event
            transactionTasks.Add(db.PublishAsync(userChannel, jsonData));

            await Task.WhenAll(transactionTasks);
        }

    }
}
