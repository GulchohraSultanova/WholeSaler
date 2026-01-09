using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.Account;

namespace WholeSalers.Application.Abstracts.Services
{
    public interface IAdminService
    {
        Task RegisterAdminAsync(RegisterDto registerDto);
        Task<TokenResponseDto> LoginAdminAsync(LoginDto loginDto);
        Task DeleteAllAdminsAsync();
    }
}
