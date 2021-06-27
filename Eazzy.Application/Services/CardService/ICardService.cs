using Eazzy.Application.Models.Customer;
using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Application.Services.CardService
{
    public interface ICardService
    {
        Card FindById(int id);

        void InsertCard(Card card);

        void InsertCard(IEnumerable<Card> cards);

        void DeleteCard(Card card);

        IPagedList<Card> GetAllCards(GetCardsRequest request);
    }
}
