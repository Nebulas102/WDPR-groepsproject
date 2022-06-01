using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class EmailApi
{
    public static async Task SendMail(MailRequest request)
    {
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        HttpClient HttpClient = new HttpClient(clientHandler);

        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("ToEmail", request.ToEmail), 
            new KeyValuePair<string, string>("Subject", request.Subject), 
            new KeyValuePair<string, string>("Body", request.Body)
        });

        string url = "http://groepd5-001-site1.itempurl.com/api/mail/send";

        await HttpClient.PostAsync(url, formContent);
    }
}