using FHT.Application.Core;
using FHT.Application.Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FHT.Api.Core.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        protected readonly NotificationDomainHandler _notifications;

        protected BaseController(INotificationHandler<NotificationDomain> notifications, IMediator handle)
        {
            _notifications = (NotificationDomainHandler)notifications;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public new IActionResult Response(object result = null)
        {
            if (OperacaoEhValida())
            {
                if (result != null && result.GetType() == typeof(CommandResult))
                {
                    CommandResult commandResult = (CommandResult)result;

                    return commandResult.Data != null ? Created("", commandResult.Data) : NoContent();
                }
                else
                {
                    return result == null ? NotFound() : Ok(result);
                }
            }
            else
            {
                return BadRequest(new CommandResult(_notifications.GetNotifications()));
            }
        }

        protected bool OperacaoEhValida()
        {
            return !_notifications.HasNotifications();
        }
    }
}
