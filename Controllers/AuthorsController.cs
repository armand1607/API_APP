using API_APP.Contracts;
using API_APP.Data;
using API_APP.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_APP.Controllers
{
    /// <summary>
    /// Endpoint used to interact with the Authors in the book store's Database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository,
                                 ILoggerService logger,
                                 IMapper mapper)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Get All Authors
        /// </summary>
        /// <returns>List of Authors</returns>
        [HttpGet]

        [AllowAnonymous]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted Call");
                var authors = await _authorRepository.FindAll();
                var response = _mapper.Map<IList<AuthorDTO>>(authors);
                _logger.LogInfo($"{location}: Succesfull Call");
                return Ok(response);

            }
            catch (Exception e)
            {
                return InternalError($"{location} : {e.Message} - {e.InnerException}");
            }

        }
        /// <summary>
        /// Gets an Author by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]

        [AllowAnonymous]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted Call for id:{id}");
                var author = await _authorRepository.FindById(id);
                if (author == null)
                {
                    _logger.LogWarn($"{location}: Failed to retreive for id:{id}");
                    return NotFound();
                }
                var response = _mapper.Map<AuthorDTO>(author);
                _logger.LogInfo($"{location}: Succesfull Call for id:{id}");
                return Ok(response);

            }
            catch (Exception e)
            {
                return InternalError($"{location} : {e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Create a New Author
        /// </summary>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpPost]

        [Authorize(Roles = "Administrator")]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDTO authorDTO)
        {
            var location = GetControllerActionNames();
            try
            {

                _logger.LogInfo($"{location} : Create Attempted");
                if (authorDTO == null)
                {
                    _logger.LogWarn($"{location}: Empty Request was Made");
                    return BadRequest(ModelState);
                }
                if(!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Data was Incomplete");
                    return BadRequest(ModelState);
                }
                var author = _mapper.Map<Author>(authorDTO);
                var isSuccess = await _authorRepository.Create(author);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Creation Failed");
                }
                InternalError($"{location}: Creation Succesful");
                return Created("Create", new { author });
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }


#pragma warning restore CS1572 // XML comment has a param tag, but there is no parameter by that name
#pragma warning disable CS1572 // XML comment has a param tag, but there is no parameter by that name
        /// <summary>
        /// Updates an Author
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]

        [Authorize(Roles = "Administrator")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorUpdateDTO authorDTO)
        {
            var location = GetControllerActionNames();
            try
            {

                _logger.LogInfo($"{location}:Update Attempted on record with id: {id}");
                if (id <1 || authorDTO == null || id != authorDTO.Id)
                {

                    _logger.LogInfo($"{location}: Update Failed with bad data - id: {id}");
                    return BadRequest();
                }
                var isExists = await _authorRepository.isExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id: {id}");
                    return NotFound();
                }
                if(!ModelState.IsValid)
                {

                    _logger.LogInfo($"{location}: Data was Incomplete");
                    return BadRequest(ModelState);
                }
                var author = _mapper.Map<Author>(authorDTO);
                var isSuccess = await _authorRepository.Update(author);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Operation Failed");
                }

                _logger.LogInfo($"{location}: Operation Succesfull");
                return NoContent();
            }
            catch (Exception e)
            {

                return InternalError($"{e.Message} - {e.InnerException}");

            }

        }

        /// <summary>
        /// Deletes an Author by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]

        [Authorize(Roles = "Administrator")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionNames();
            try
            {

                _logger.LogInfo($"{location}:Delete Attempted on record with id: {id}");
                if (id<1)
                {

                    _logger.LogInfo($"{location}:Delete failed with bad data on record with id: {id}");
                    return BadRequest();
                }
                var isExists = await _authorRepository.isExists(id);
                if (!isExists)
                {
                    _logger.LogInfo($"{location}:Record with id: {id} was not found");
                    return NotFound();
                }
                var author = await _authorRepository.FindById(id);            
                var isSuccess = await _authorRepository.Delete(author);
                if(!isSuccess)
                {

                    _logger.LogInfo($"{location}:Delete Attempt with id: {id} failed");
                }

                _logger.LogInfo($"{location}:Delete Attempt with id: {id} succeded");
                return NoContent();

            }
            catch (Exception e)
            {

                return InternalError($"{e.Message} - {e.InnerException}");

            }
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;
            return $"{controller} - {action}";
        }



        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong. Please contact the Administrator");
        }
    }
}
