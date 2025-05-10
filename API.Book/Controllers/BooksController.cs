using APP.Book.Features.Books;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Book.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILogger<BooksController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator.Send(new BookQueryRequest());
                var list = await response.ToListAsync();
                if (list.Any())
                    return Ok(list);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("BooksGet Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during BooksGet."));
            }
        }

        // GET: api/Books/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _mediator.Send(new BookQueryRequest());
                var item = await response.SingleOrDefaultAsync(r => r.Id == id);
                if (item is not null)
                    return Ok(item);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("BooksGetById Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during BooksGetById."));
            }
        }

        // POST: api/Books
        [HttpPost]
        public async Task<IActionResult> Post(BookCreateRequest bookCreateRequest)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(bookCreateRequest);
                    if (response.IsSuccessful)
                    {
                        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
                    }
                    ModelState.AddModelError("BooksPost", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("BooksPost Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during BooksPost."));
            }
        }


        // PUT: api/Books
        [HttpPut]
        public async Task<IActionResult> Put(BookUpdateRequest bookUpdateRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(bookUpdateRequest);
                    if (response.IsSuccessful)
                    {
                        return Ok(response);
                    }
                    ModelState.AddModelError("BooksPut", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("BooksPut Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during BooksPut."));
            }

        }

        //DELETE: api/Books/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new BookDeleteRequest() { Id = id });
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                ModelState.AddModelError("BooksDelete", response.Message);
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("BooksDelete Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during BooksDelete."));
            }
        }

        // api/Books/Filter

        [HttpPost("[action]")]
        public async Task<IActionResult> Filter(BookQueryRequest request)
        {
            var query = await _mediator.Send(request);
            var list = query.ToList();
            if (list.Any())
                return Ok(list);
            return NotFound();
        }
    }
}
