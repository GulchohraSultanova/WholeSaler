using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.DTOs.Account;
using WholeSalers.Domain.Entities;

namespace WholeSalers.Application.Abstracts.Services
{
    public interface ITokenService
    {
        TokenResponseDto CreateToken(Admin admin, string role, int expireDate = 1440);

    }
}
