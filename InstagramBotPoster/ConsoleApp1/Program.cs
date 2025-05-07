using System.Net;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var proxyIp = "88.198.212.91:3128"; 

            var handler = new HttpClientHandler
            {
                Proxy = new WebProxy($"https://{proxyIp}"),
                UseProxy = true
            };

            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            try
            {
                var response = await client.GetAsync("https://api.myip.com");
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"✅ Прокси работает. Ответ: {content}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Прокси не работает: {proxyIp}\nОшибка: {ex.Message}");
            }
        }
    }
}
