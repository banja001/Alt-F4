using booking.application.usecases;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPF.ViewModels.Guest1
{
    public class Guest1ViewViewModel
    {
        private readonly NotificationsService _notificationsService;
        public Guest1ViewViewModel(int userId)
        {
            _notificationsService = new NotificationsService();

            _notificationsService.NotifyGuest1(userId);
        }


    }
}
