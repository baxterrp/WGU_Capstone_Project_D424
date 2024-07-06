using FishingTournamentTracker.Api.Exeptions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace FishingTournamentTracker.Api.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected async Task<IActionResult> ExecuteControllerAction(Func<Task<IActionResult>> func, [CallerMemberName] string callerMemberName = "")
        {
            var stopwatch = new Stopwatch();

            try
            {
                Console.WriteLine($"Executing api action {callerMemberName}");
                stopwatch.Start();
                return await func();
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
                return BadRequest(argumentException.Message);
            }
            // no message needed here, just return a 401
            catch (UnauthorizedLoginAttemptException)
            {
                return Unauthorized();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"Request from {callerMemberName} completed in {stopwatch.Elapsed.TotalSeconds} seconds");
            }
        }
    }
}
