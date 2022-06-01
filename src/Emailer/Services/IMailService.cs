using System.Threading.Tasks;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}