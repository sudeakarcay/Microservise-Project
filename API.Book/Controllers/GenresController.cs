﻿using APP.Book.Features.Genres;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Book.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ILogger<GenresController> _logger;
        private readonly IMediator _mediator;

        public GenresController(ILogger<GenresController> logger, IMediator mediator)
        {
            _logger = logger; 
            _mediator = mediator;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator.Send(new GenreQueryRequest());
                var list = await response.ToListAsync();
                if (list.Any())
                    return Ok(list);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("GenresGet Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during GenresGet."));
            }
        }

        // GET: api/Genres/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _mediator.Send(new GenreQueryRequest());
                var item = await response.SingleOrDefaultAsync(g => g.Id == id);
                if (item is not null)
                    return Ok(item);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("GenresGetById Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during GenresGetById."));
            }
        }

        // POST: api/Genres
        [HttpPost]
        public async Task<IActionResult> Post(GenreCreateRequest genreCreateRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(genreCreateRequest);
                    if (response.IsSuccessful)
                    {
                        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
                    }
                    ModelState.AddModelError("GenresPost", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("GenresPost Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during GenresPost."));
            }
        }


        // PUT: api/Genres
        [HttpPut]
        public async Task<IActionResult> Put(GenreUpdateRequest genreUpdateRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(genreUpdateRequest);
                    if (response.IsSuccessful)
                    {
                        return Ok(response);
                    }
                    ModelState.AddModelError("GenresPut", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("GenresPut Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during GenresPut."));
            }

        }

        //DELETE: api/Books/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new GenreDeleteRequest() { Id = id });
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                ModelState.AddModelError("GenresDelete", response.Message);
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("GenresDelete Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during GenresDelete."));
            }
        }
    }
}
