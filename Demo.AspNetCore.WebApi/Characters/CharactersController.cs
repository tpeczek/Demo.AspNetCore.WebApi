using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Demo.AspNetCore.WebApi.Characters.Actions;
using Demo.AspNetCore.WebApi.Http.ConditionalRequests;

namespace Demo.AspNetCore.WebApi.Characters
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : Controller
    {
        #region Fields
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public CharactersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        #region Actions
        // GET api/characters
        [HttpGet, HttpHead]
        public IAsyncEnumerable<Character> Get()
        {
            // I'm cheating here as I know that GetAllHandler is returning IAsyncEnumerable<Character> synchronously.
            // Otherwise it would have to be like this:
            // await foreach (Character character in await _mediator.Send(new GetAllRequest()))
            // {
            //     yield return character;
            // }

            return _mediator.Send(new GetAllRequest()).Result;
        }

        // GET api/characters/{id}
        [HttpGet("{id}"), HttpHead("{id}")]
        public async Task<ActionResult<Character>> Get(string id)
        {
            Character character = await _mediator.Send(new GetByIdRequest(id));
            if (character is null)
            {
                return NotFound();
            }

            return character;
        }

        // POST api/characters
        [HttpPost]
        public async Task<IActionResult> Post(Character character)
        {
            if (await _mediator.Send(new ExistsRequest(character.Name)))
            {
                return StatusCode((int)HttpStatusCode.Conflict);
            }

            character = await _mediator.Send(new CreateRequest(character));

            return CreatedAtAction(nameof(Get), new { character.Id }, character);
        }

        // PUT api/characters/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Character>> Put(string id, Character update)
        {
            if (await _mediator.Send(new ExistsRequest(update.Name, id)))
            {
                return StatusCode((int)HttpStatusCode.Conflict);
            }

            Character character;
            try
            {
                character = await _mediator.Send(new UpdateRequest(id, update, Request.GetRequestConditions()));
            }
            catch (PreconditionFailedException)
            {
                return StatusCode(StatusCodes.Status412PreconditionFailed);
            }

            if (character is null)
            {
                return NotFound();
            }

            return character;
        }

        // DELETE api/characters/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Character character = await _mediator.Send(new GetByIdRequest(id));
            if (character == null)
            {
                return NotFound();
            }

            await _mediator.Send(new DeleteRequest(id));

            return NoContent();
        }
        #endregion
    }
}
