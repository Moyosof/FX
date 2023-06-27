using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FX.DTO;
using FX.DTO.OAuth;

namespace FX.Application.Contracts
{
    /// <summary>
    /// This service is used in generating jwt token and refresh token
    /// </summary>
    public interface ITokenGenerator
    {
        /// <summary>
        /// Generate jwt token
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        Task<AuthResponse> GenerateJwtToken(string user_id, string phoneNumber, string username, string email, IList<string> roles = null);

        /// <summary>
        /// This service is used in verifying the refresh token to generate new jwt
        /// </summary>
        /// <param name="RefreshToken"></param>
        /// <returns></returns>
        Task<Result<AuthResponse>> VerifyRefreshToken(string RefreshToken);
    }
}
