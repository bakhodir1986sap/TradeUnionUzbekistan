using Shops_on_wheels_bot.Models.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Shops_on_wheels_bot.Models
{
    public static class Bot
    {
        private static List<Command> commandList;
        public static IReadOnlyList<Command> Commands
        {
            get
            {
                return commandList.AsReadOnly();
            }
        }
        private static TelegramBotClient client;

        public static async Task<TelegramBotClient> Get()
        {
            if (client != null)
            {
                return client;
            }

            commandList = new List<Command>();
            commandList.Add(new HelloCommand());
            //TODO: Add more commands

            client = new TelegramBotClient(AppSettings.Key);
            var hook = string.Format(AppSettings.Url, "api/message/update");

            await client.SetWebhookAsync(hook);

            return client;
        }
    }
}