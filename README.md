# AlifSolution
Simple Notify Solution

## Dependencies
* .Net Core 7 (SDK)
* StackExchange.Redis (NuGet)
* Redis (Database)

## Service Description
* ### CardService 
> Service for processing and publishing events to Redis.
* ### NotifyService 
> Service for intercepting publications from the Redis and sending them to the user.

## Process Description
> The CardService receives a request for a card transaction, saves it to the database and publishes the event to the channel with the user ID.

> The NotificationService provides the user with an API with the “SSE” technology to subscribe to a channel under their identifier to receive notifications from the server.