using Eazzy.Application.Models.Customer;
using Eazzy.Application.Services.CardService;
using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Infrastructure;
using Eazzy.Models.Customers;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Eazzy.V1.Controllers
{
    [Route("v1/card")]
    [Authorize]
    public class CardController : WebApiController
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<Card>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetCards([FromQuery]SortAndPaged sortAndPaged)
        {
            var customer = GetCurrentCustomer();
            var cards = _cardService.GetAllCards(new GetCardsRequest
            {
                CustomerId = customer.Id,
                Sort = sortAndPaged.Sort,
                SortBy = sortAndPaged.SortBy,
                PageSize = sortAndPaged.PageSize,
                PageIndex = sortAndPaged.PageIndex
            });

            if (cards == null || !cards.Any())
            {
                return Fail(HttpStatusCode.NotFound, "Cards weren't found.");
            }

            return Ok(cards);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        public IActionResult AddCard([FromBody]AddCardModel model)
        {
            var customer = GetCurrentCustomer();
            var card = new Card()
            {
                CardHolder = model.CardHolder,
                CCV = model.CCV,
                Expires = model.Expires,
                NumberMask = model.NumberMask,
                Guid = new Guid(),
                Customer = customer
            };

            _cardService.InsertCard(card);

            return StatusCode(StatusCodes.Status201Created);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult DeleteCard(int id)
        {
            var card = _cardService.FindById(id);

            if (card == null)
            {
                return Fail(HttpStatusCode.NotFound, "Card wasn't found.");
            }

            _cardService.DeleteCard(card);

            return NoContent();
        }
    }
}
