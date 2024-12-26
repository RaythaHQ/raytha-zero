using RaythaZero.Domain.Common;

namespace RaythaZero.Application.Common.Interfaces;

public interface IEmailer
{
    void SendEmail(EmailMessage message);
}
