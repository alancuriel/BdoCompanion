using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using uwpUI.Activation;

using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;

namespace uwpUI.Services
{
    internal partial class ToastNotificationsService : ActivationHandler<ToastNotificationActivatedEventArgs>
    {
        public void ShowToastNotification(ToastNotification toastNotification)
        {
            try
            {
                ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
            }
            catch (Exception)
            {

            }
        }

        public void ScheduleToastNotification(ScheduledToastNotification toastNotification)
        {
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toastNotification);
        }

        public void ScheduleToastNotifications(IEnumerable<ScheduledToastNotification> toastNotifications)
        {
            var toastNotifier = ToastNotificationManager.CreateToastNotifier();

            foreach (var toastNotification in toastNotifications)
            {
                toastNotifier.AddToSchedule(toastNotification);
            }
        }

        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
        {
            await Task.CompletedTask;
        }
    }
}
