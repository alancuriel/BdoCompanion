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
                // TODO WTS: Adding ToastNotification can fail in rare conditions, please handle exceptions as appropriate to your scenario.
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
            //// TODO WTS: Handle activation from toast notification
            //// More details at https://docs.microsoft.com/windows/uwp/design/shell/tiles-and-notifications/send-local-toast

            await Task.CompletedTask;
        }
    }
}
