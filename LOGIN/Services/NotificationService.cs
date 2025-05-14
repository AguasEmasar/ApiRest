using AutoMapper;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using LOGIN.Dtos;
using LOGIN.Dtos.Communicates;
using LOGIN.Dtos.ScheduleDtos.RegistrationWater;
using LOGIN.Entities;
using LOGIN.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LOGIN.Services
{
    public class NotificationService : INotificationService
    {
        private readonly FirebaseMessaging _messaging;

        // Constructor que inicializa Firebase con tu archivo de credenciales
        public NotificationService(IConfiguration configuration)
        {
            // Obtener la ruta desde la configuración
            string firebaseCredentialFile = configuration["Firebase:CredentialPath"] ??
                Path.Combine(Directory.GetCurrentDirectory(), "firebase-config.json");

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(firebaseCredentialFile)
                });
            }

            _messaging = FirebaseMessaging.DefaultInstance;
        }

        // Método para enviar notificación de comunicado
        public async Task SendCommunicateNotificationAsync(CommunicateDto communicate)
        {
            var message = new Message
            {
                // Topic para enviar a todos los dispositivos suscritos a este tema
                Topic = "comunicados",
                Notification = new Notification
                {
                    Title = communicate.Tittle,
                    Body = $"Nuevo comunicado: {communicate.Type_Statement}"
                },
                Data = new Dictionary<string, string>
                {
                    { "id", communicate.Id.ToString() },
                    { "date", communicate.Date.ToString("yyyy-MM-dd HH:mm") },
                    { "type", communicate.Type_Statement },
                    { "content", communicate.Content },
                    { "notificationType", "comunicado" }
                }
            };

            string response = await _messaging.SendAsync(message);
            Console.WriteLine($"Successfully sent message: {response}");
        }

        // Método para enviar notificación de registro de agua
        public async Task SendWaterRegistrationNotificationAsync(RegistrationWaterDto waterRegistration)
        {
            // Obtener nombres de barrios/colonias para incluir en la notificación
            var neighborhoods = string.Join(", ",
                waterRegistration.NeighborhoodColonies.Select(nc => nc.Name).ToArray());

            var message = new Message
            {
                // Puedes enviar a todas los dispositivos o hacer que los usuarios
                // se suscriban a barrios específicos
                Topic = "agua_registros",
                Notification = new Notification
                {
                    Title = "Nuevo registro de agua",
                    Body = $"Se ha registrado agua para: {neighborhoods}"
                },
                Data = new Dictionary<string, string>
                {
                    { "id", waterRegistration.Id.ToString() },
                    { "date", waterRegistration.Date.ToString("yyyy-MM-dd HH:mm") },
                    { "observations", waterRegistration.Observations },
                    { "neighborhoods", neighborhoods },
                    { "notificationType", "agua" }
                }
            };

            string response = await _messaging.SendAsync(message);
            Console.WriteLine($"Successfully sent message: {response}");
        }

        // Método para enviar notificación a un dispositivo específico
        public async Task<string> SendToDeviceAsync(string deviceToken, string title, string body, Dictionary<string, string> data)
        {
            var message = new Message
            {
                Token = deviceToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data ?? new Dictionary<string, string>()
            };

            string response = await _messaging.SendAsync(message);
            Console.WriteLine($"Successfully sent message to device: {response}");
            return response;
        }
    }
}