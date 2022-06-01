using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static zmdh.Areas.Identity.Pages.Account.RegisterModel;

public class ClientApi
{
    public static readonly HttpClient HttpClient = new HttpClient();    

    public static async Task<int> PostClient(ApiInput apiInput)
    {
        var fullname = apiInput.FullName;
        var iban = apiInput.Iban;
        var bsn = apiInput.BSN.ToString();
        var birthdate = apiInput.Birthdate.ToString("dddd, MMMM d, yyyy");

        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("volledigenaam", fullname), 
            new KeyValuePair<string, string>("IBAN", iban), 
            new KeyValuePair<string, string>("BSN", bsn), 
            new KeyValuePair<string, string>("gebdatum", birthdate)
        });

        string url = "https://orthopedagogie-zmdh.herokuapp.com/clienten?sleutel=166435290";

        var response = await HttpClient.PostAsync(url, formContent);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            return int.Parse(responseBody.Substring(18));
        }
        return 0;
    }
    public static async Task<int> DeleteClient(int clientId)
    {
        string key = "166435290";

        string keyParameter = "sleutel=" + key;
        string clientIdParameter = "clientid=" + clientId.ToString();

        var url = "https://orthopedagogie-zmdh.herokuapp.com/clienten/?" + keyParameter + "&" + clientIdParameter;

        var response = await HttpClient.DeleteAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            return int.Parse(responseBody.Substring(18));
        }
        return 0;
    }

    public static async Task<List<int>> FetchClientIds()
    {
        List<int> clientids = new List<int>();
        string url = "https://orthopedagogie-zmdh.herokuapp.com/clienten?sleutel=166435290";

        var response = await HttpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            clientids = JsonConvert.DeserializeObject<List<int>>(responseBody);
        }
        return clientids;
    }

    public static async Task<Boolean> CheckBSN(List<int> clientids, string bsn)
    {
        foreach(var clientid in clientids)
        {
            var client = await PullSpecificClient(clientid);

            if(bsn.Equals(client.BSN))
                return true;
        }
        return false;
    }

    public static async Task<SpecificApiClient> PullSpecificClient(int clientId)
    {
        string key = "166435290";

        string keyParameter = "sleutel=" + key;
        string clientIdParameter = "clientid=" + clientId.ToString();

        var url = "https://orthopedagogie-zmdh.herokuapp.com/clienten/?" + keyParameter + "&" + clientIdParameter;

        var response = await HttpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SpecificApiClient>(responseBody);
            return result;
        }
        else
        {
            throw new Exception((int)response.StatusCode + "-" + response.StatusCode.ToString());
        }
    }
}