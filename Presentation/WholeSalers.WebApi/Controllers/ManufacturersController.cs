using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.Manufacturer;
using WholeSalers.Application.GlobalAppException;

namespace WholeSalers.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly ILogger<ManufacturersController> _logger;

        public ManufacturersController(IManufacturerService manufacturerService, ILogger<ManufacturersController> logger)
        {
            _manufacturerService = manufacturerService;
            _logger = logger;
        }

        /// <summary>
        /// Bütün manufacturer-ları gətir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _manufacturerService.GetAllAsync();
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Manufacturer-lar gətirilərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Manufacturer-lar gətirilərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// ID ilə manufacturer gətir
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var result = await _manufacturerService.GetByIdAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Manufacturer gətirilərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Manufacturer gətirilərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// Yeni manufacturer yarat
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateManufacturerDto dto)
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
                await _manufacturerService.CreateAsync(dto);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Manufacturer uğurla yaradıldı!"
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Manufacturer yaradılarkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Manufacturer yaradılarkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// Manufacturer yenilə
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] UpdateManufacturerDto dto)
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
                await _manufacturerService.UpdateAsync(dto);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Manufacturer uğurla yeniləndi!"
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Manufacturer yenilənərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Manufacturer yenilənərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// Manufacturer sil (Soft delete)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _manufacturerService.DeleteAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Manufacturer uğurla silindi!"
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Manufacturer silinərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Manufacturer silinərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }
    }
}
