using System;
using System.Text.Json;
using NotifyService.Models;

namespace NotifyService.src
{
    public static class SSEHttpContextExtensions
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static async Task SSEInitAsync(this HttpResponse response)
        {
            response.Headers.Add("Content-Type", "text/event-stream");
            await response.Body.FlushAsync();
        }

        public static async Task SSESendEventAsync(this HttpResponse response, ISSEEvent e, string? sseEventName = null)
        {
            var eventData = JsonSerializer.Serialize(e.Payload, Options);

            foreach (var line in eventData.Split('\n'))
                await response.WriteAsync($"data: {line}\n");

            await response.WriteAsync("\n");
            await response.Body.FlushAsync();
        }


        public static async Task SSESendCloseAsync(this HttpResponse response)
        {
            await response.SSESendEventAsync(new SSEEvent("close", null));
        }
    }
}