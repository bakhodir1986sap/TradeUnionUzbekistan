using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Shops_on_wheels_bot.Models.Commands
{
    public class HelloCommand : Command
    {
        private string _name = "hello";

        public override string Name
        { get
            {
                return _name;
            }
          set
            {
                _name = value;
            }
        }

        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            //TODO: Bot logic

            await client.SendTextMessageAsync(chatId, "hello Baxa", replyToMessageId: messageId);
        }
    }
}