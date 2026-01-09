using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.Contact;
using WholeSalers.Application.GlobalAppException;

namespace WholeSalers.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(IContactService contactService, ILogger<ContactsController> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }

        /// <summary>
        /// Bütün contact mesajlarını gətir (Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _contactService.GetAllAsync();
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Contact-lar gətirilərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Contact-lar gətirilərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// ID ilə contact mesajını gətir (Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var result = await _contactService.GetByIdAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Contact gətirilərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Contact gətirilərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// Yeni contact mesajı yarat (Public)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = "Yanlış daxiletmə məlumatı!"
                });
            }

            try
            {
                await _contactService.CreateAsync(dto);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Mesajınız uğurla göndərildi!"
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Contact yaradılarkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Contact yaradılarkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// Contact mesajını sil (Soft delete) (Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _contactService.DeleteAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Contact uğurla silindi!"
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Contact silinərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Contact silinərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }
    }
}
