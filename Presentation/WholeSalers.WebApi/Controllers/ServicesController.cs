using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.Service;
using WholeSalers.Application.GlobalAppException;

namespace WholeSalers.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        private readonly ILogger<ServicesController> _logger;

        public ServicesController(IServiceService serviceService, ILogger<ServicesController> logger)
        {
            _serviceService = serviceService;
            _logger = logger;
        }

        /// <summary>
        /// Bütün servisləri gətir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _serviceService.GetAllAsync();
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Servislər gətirilərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Servislər gətirilərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// ID ilə service gətir
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var result = await _serviceService.GetByIdAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Service gətirilərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service gətirilərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// Yeni service yarat
        /// </summary>
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateServiceDto dto)
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
                await _serviceService.CreateAsync(dto);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Service uğurla yaradıldı!"
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Service yaradılarkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service yaradılarkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// Service yenilə
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] UpdateServiceDto dto)
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
                await _serviceService.UpdateAsync(dto);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Service uğurla yeniləndi!"
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Service yenilənərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service yenilənərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// Service sil (Soft delete)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _serviceService.DeleteAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Service uğurla silindi!"
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Service silinərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service silinərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }
    }
}
