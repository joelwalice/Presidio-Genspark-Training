using JobPortalAPI.DTOs;
using JobPortalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using JobPortalAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using JobPortalAPI.Hubs;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IHubContext<JobHub> _hubContext;

    public NotificationController(IHubContext<JobHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        return Ok("Notification sent.");
    }
}
