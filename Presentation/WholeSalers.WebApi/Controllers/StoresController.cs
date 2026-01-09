using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WholeSalers.Application.Abstracts.Services;
using WholeSalers.Application.DTOs.Store;
using WholeSalers.Application.GlobalAppException;

namespace WholeSalers.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly ILogger<StoresController> _logger;

        public StoresController(IStoreService storeService, ILogger<StoresController> logger)
        {
            _storeService = storeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _storeService.GetAllAsync();
                return Ok(new { StatusCode = StatusCodes.Status200OK, Data = result });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Store-lar gətirilərkən xəta baş verdi!");
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Store-lar gətirilərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { StatusCode = StatusCodes.Status500InternalServerError, Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!" });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var result = await _storeService.GetByIdAsync(id);
                return Ok(new { StatusCode = StatusCodes.Status200OK, Data = result });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Store gətirilərkən xəta baş verdi!");
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Store gətirilərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { StatusCode = StatusCodes.Status500InternalServerError, Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateStoreDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, Error = "Yanlış daxiletmə məlumatı!" });

            try
            {
                await _storeService.CreateAsync(dto);
                return StatusCode(StatusCodes.Status201Created,
                    new { StatusCode = StatusCodes.Status201Created, Message = "Store uğurla yaradıldı!" });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Store yaradılarkən xəta baş verdi!");
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Store yaradılarkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { StatusCode = StatusCodes.Status500InternalServerError, Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] UpdateStoreDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, Error = "Yanlış daxiletmə məlumatı!" });

            try
            {
                await _storeService.UpdateAsync(dto);
                return Ok(new { StatusCode = StatusCodes.Status200OK, Message = "Store uğurla yeniləndi!" });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Store yenilənərkən xəta baş verdi!");
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Store yenilənərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { StatusCode = StatusCodes.Status500InternalServerError, Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _storeService.DeleteAsync(id);
                return Ok(new { StatusCode = StatusCodes.Status200OK, Message = "Store uğurla silindi!" });
            }
            catch (GlobalAppException ex)
            {
                _logger.LogError(ex, "Store silinərkən xəta baş verdi!");
                return BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Store silinərkən gözlənilməz xəta baş verdi!");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { StatusCode = StatusCodes.Status500InternalServerError, Error = "Gözlənilməz xəta baş verdi. Zəhmət olmasa, yenidən cəhd edin!" });
            }
        }
    }
}
