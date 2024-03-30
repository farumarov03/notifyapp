using System;
namespace NotifyService.Models
{
    public interface ISSEEvent
    {
        string Name { get; }
        EventModel Payload { get; }
    }
}

