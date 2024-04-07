using System;

namespace Dreamscape.Application.Services
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
