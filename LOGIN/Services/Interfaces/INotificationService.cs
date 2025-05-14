using FirebaseAdmin.Messaging;
using LOGIN.Dtos.Communicates;
using LOGIN.Dtos.NotificationDtos;
using LOGIN.Dtos.ScheduleDtos.RegistrationWater;

namespace LOGIN.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendCommunicateNotificationAsync(CommunicateDto communicate);
        Task SendWaterRegistrationNotificationAsync(RegistrationWaterDto waterRegistration);
    }
}
