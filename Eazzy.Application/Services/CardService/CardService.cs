using Eazzy.Application.Models.Customer;
using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Infrastructure.Repository.Interfaces;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eazzy.Application.Services.CardService
{
    public class CardService : ICardService
    {
        private readonly IRepository<Card> _cardRepository;

        public CardService(IRepository<Card> cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public void DeleteCard(Card card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            _cardRepository.Delete(card);
        }

        public Card FindById(int id)
        {
            var card = _cardRepository.Find(id);

            return card;
        }

        public IPagedList<Card> GetAllCards(GetCardsRequest request)
        {
            var cards = _cardRepository.Table.Where(x => x.CustomerId == request.CustomerId);

            return new PagedList<Card>(cards, request.PageIndex, request.PageSize);
        }

        public void InsertCard(Card card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            _cardRepository.Add(card);
        }

        public void InsertCard(IEnumerable<Card> cards)
        {
            if (cards == null)
                throw new ArgumentNullException(nameof(cards));

            _cardRepository.Add(cards);
        }
    }
}
