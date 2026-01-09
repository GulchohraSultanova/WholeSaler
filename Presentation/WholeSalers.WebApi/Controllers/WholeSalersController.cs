using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.WholeSaler;
using WholeSalers.Application.GlobalAppException;

namespace WholeSalers.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WholeSalersController : ControllerBase
    {
        private readonly IWholeSalerService _wholeSalerService;
        private readonly ILogger<WholeSalersController> _logger;

        public WholeSalersController(IWholeSalerService wholeSalerService, ILogger<WholeSalersController> logger)
        {
            _wholeSalerService = wholeSalerService;
            _logger = logger;
        }

        /// <summary>
        /// Bütün wholesaler-ları gətir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _wholeSalerService.GetAllAsync();
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "WholeSaler-lar gətirilərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WholeSaler-lar gətirilərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// ID ilə wholesaler gətir
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var result = await _wholeSalerService.GetByIdAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "WholeSaler gətirilərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WholeSaler gətirilərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// Yeni wholesaler yarat
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateWholeSalerDto dto)
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
                await _wholeSalerService.CreateAsync(dto);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "WholeSaler uğurla yaradıldı!"
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "WholeSaler yaradılarkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WholeSaler yaradılarkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// Wholesaler yenilə
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] UpdateWholeSalerDto dto)
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
                await _wholeSalerService.UpdateAsync(dto);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "WholeSaler uğurla yeniləndi!"
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "WholeSaler yenilənərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WholeSaler yenilənərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }

        /// <summary>
        /// Wholesaler sil (Soft delete)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _wholeSalerService.DeleteAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "WholeSaler uğurla silindi!"
                });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "WholeSaler silinərkən xəta baş verdi!");
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WholeSaler silinərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!"
                });
            }
        }
    }
}
