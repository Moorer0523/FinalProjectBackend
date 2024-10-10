using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace anonymous_chats_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] --REMOVE AFTER MVP
    public class ApiBaseController : ControllerBase
    {
        protected string GetCurrentUserID()
        {
            if (User.Identity is ClaimsIdentity identity)
            {
                // Extract the user ID from the claims (assuming it's stored in the "sub" claim)
                var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }
            }

            return "1";  // --REMOVE AFTER MVP
            // return null;
        }
    }
}
