using System;
using NotifyService.Models;

namespace NotifyService.src
{
	public interface INotifyService
	{
        IAsyncEnumerable<SSEEvent> ProduceEvents(string id);
    }
}

