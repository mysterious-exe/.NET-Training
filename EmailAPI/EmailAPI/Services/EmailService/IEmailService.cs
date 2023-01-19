using EmailAPI.Models;

namespace EmailAPI.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(Emaildto request);
    }
}
