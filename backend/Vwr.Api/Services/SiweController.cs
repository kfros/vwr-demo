using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vwr.Domain;

namespace Vwr.Api.Services;

[Route("api/auth/siwe/verify")]
[ApiController]
public class SiweController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Verify(SiweRequest r, ISignatureVerifier v)
    {
        var ok = await v.VerifyAsync(r);
        return ok ? Ok(new { jwt = Guid.NewGuid().ToString("N") }) : Unauthorized();
    }
}
