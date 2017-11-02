using System;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Demo.AspNetCore.Mvc.CosmosDB.Http;
using Demo.AspNetCore.Mvc.CosmosDB.Model;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;
using Demo.AspNetCore.Mvc.CosmosDB.Exceptions;
using Microsoft.AspNetCore.JsonPatch;

namespace Demo.AspNetCore.Mvc.CosmosDB.Controllers
{
    [Route("api/[controller]")]
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
        [HttpGet]
        public async Task<IEnumerable<Character>> Get()
        {
            return await _mediator.Send(new GetCollectionRequest<Character>());
        }

        // GET api/characters/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Character character = await _mediator.Send(new GetSingleRequest<Character>(id));
            if (character == null)
            {
                return NotFound();
            }

            return new ObjectResult(character);
        }

        // POST api/characters
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Character character)
        {
            IActionResult modelValidationResult = await ValidateModelAsync(character);
            if (modelValidationResult != null)
            {
                return modelValidationResult;
            }

            character = await _mediator.Send(new CreateRequest<Character>(character));

            return CreatedAtAction(nameof(Get), new { Id = character.Id }, character);
        }

        // PUT api/characters/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]Character update)
        {
            IActionResult modelValidationResult = await ValidateModelAsync(update, id);
            if (modelValidationResult != null)
            {
                return modelValidationResult;
            }

            Character character = null;
            try
            {
                character = await _mediator.Send(CreateUpdateRequest(id, update));
            }
            catch (PreconditionFailedException)
            {
                return StatusCode(StatusCodes.Status412PreconditionFailed);
            }

            if (character == null)
            {
                return NotFound();
            }

            return Ok(character);
        }

        // DELETE api/characters/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Character character = await _mediator.Send(new GetSingleRequest<Character>(id));
            if (character == null)
            {
                return NotFound();
            }

            await _mediator.Send(new DeleteRequest<Character>(character));

            return NoContent();
        }
        #endregion

        #region Methods
        private async Task<IActionResult> ValidateModelAsync(Character character, string id = null)
        {
            if (character == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _mediator.Send(new CheckExistsRequest<Character>(character.Name, id)))
            {
                return StatusCode((int)HttpStatusCode.Conflict);
            }

            return null;
        }

        private UpdateRequest<T> CreateUpdateRequest<T>(string id, T update)
        {
            HttpRequestConditions requestConditions = Request.GetRequestConditions();

            return new UpdateRequest<T>(id, update, requestConditions.IfMatch, requestConditions.IfUnmodifiedSince);
        }
        #endregion
    }
}
