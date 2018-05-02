using Bot_LineMessagingApi.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_LineMessagingApi.SDK.Managers
{
    public interface ITokenManager
    {
        Task<AccessToken> GetAccessTokenFromCache();
    }
}
