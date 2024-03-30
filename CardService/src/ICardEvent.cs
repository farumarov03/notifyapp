using System;
using CardService.src.Models;

namespace CardService.src
{
    public interface ICardEvent
    {
        Task SaveCardEvent(string userId, EventModel model, string transactionId);
    }
}

