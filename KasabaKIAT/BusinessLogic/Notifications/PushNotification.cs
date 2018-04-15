using KasabaKIAT.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Notifications
{
    public enum MessageType
    {
        General,
        Urgent
    }


    public class PushNotification
    {
        public MessageType _msgType { get; set; }

        public bool msgSendActive = false;

        public PushNotification(MessageType messageTp)
        {
            _msgType = messageTp;
        }

        public void SendMsg2All(string msg)
        {
            if (string.IsNullOrEmpty(msg)) return;

            if (_msgType == MessageType.Urgent)
            {
                UrgentSendMessage(msg);
            }
            else
            {
                SendMessage(msg);
            }
        }

        private void SendMessage(string message)
        {
            // Получаем контекст хаба
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            // отправляем сообщение
            context.Clients.All.displayMessage(message);
        }

        private void UrgentSendMessage(string message)
        {
            // Получаем контекст хаба
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            // отправляем сообщение
            context.Clients.All.displayMessage(string.Format("99{0}", message));
        }
    }
}