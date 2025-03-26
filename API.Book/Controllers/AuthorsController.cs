using APP.Book.Features.Authors;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Book.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ILogger<AuthorsController> _logger;
        private readonly IMediator _mediator;

        public AuthorsController(ILogger<AuthorsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator.Send(new AuthorQueryRequest());
                var list = await response.ToListAsync();
                if (list.Any())
                    return Ok(list);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("AuthorsGet Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during AuthorsGet."));
            }
        }

        // GET: api/Authors/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _mediator.Send(new AuthorQueryRequest());
                var item = await response.SingleOrDefaultAsync(g => g.Id == id);
                if (item is not null)
                    return Ok(item);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("AuthorGetById Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during AuthorsGetById."));
            }
        }

        // POST: api/Authors
        [HttpPost]
        public async Task<IActionResult> Post(AuthorCreateRequest authorCreateRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(authorCreateRequest);
                    if (response.IsSuccessful)
                    {
                        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
                    }
                    ModelState.AddModelError("AuthorPost", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("AuthorPost Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during AuthorPost."));
            }
        }

        // PUT: api/Authors
        [HttpPut]
        public async Task<IActionResult> Put(AuthorUpdateRequest authorUpdateRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(authorUpdateRequest);
                    if (response.IsSuccessful)
                    {
                        return Ok(response);
                    }
                    ModelState.AddModelError("AuthorsPut", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("AuthorsPut Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during AuthorsPut."));
            }

        }

        //DELETE: api/Authors/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new AuthorDeleteRequest() { Id = id });
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                ModelState.AddModelError("AuthorsDelete", response.Message);
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("AuthorsDelete Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during AuthorsDelete."));
            }
        }
    }
}
