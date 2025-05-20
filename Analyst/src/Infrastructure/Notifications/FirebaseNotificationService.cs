using Application.Common.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.Notifications
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private static readonly object _lock = new();
        private static bool _initialized = false;

        public FirebaseNotificationService(IOptions<FirebaseOptions> firebaseOptions)
        {
            if (!_initialized)
            {
                lock (_lock)
                {
                    if (!_initialized)
                    {
                        FirebaseApp.Create(new AppOptions
                        {
                            Credential = GoogleCredential.FromFile(firebaseOptions.Value.CredentialsPath)
                        });

                        _initialized = true;
                    }
                }
            }
        }

        public async Task<string> SendAsync(string title, string body, string deviceToken)
        {
            var message = new Message
            {
                Token = deviceToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                }
            };

            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}