using System;
namespace NotifyService.Models
{
    public record struct SSEEvent(string Name, EventModel? Payload) : ISSEEvent;
}

