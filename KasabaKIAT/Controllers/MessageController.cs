using log4net;
using Shops_on_wheels_bot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Telegram.Bot.Types;

namespace Shops_on_wheels_bot.Controllers
{
    public class MessageController : ApiController
    {
        ILog log = log4net.LogManager.GetLogger(typeof(MessageController));

        [Route(@"api/message/update")] //web hook uri part
        public async Task<OkResult> Update([FromBody]Update update)
        {
            var commands = Bot.Commands;
            var message = update.Message;
            var client = await Bot.Get();

            log.Info(message.Text);

            foreach (var command in commands)
            {
                if (command.Contains(message.Text))
                {
                    command.Execute(message, client);
                    break;
                }
            }

            return Ok();
        }


    }
}
