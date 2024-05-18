using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace http_1
{
    internal class Program
    {
        public static async Task<string> get_web_content(string url)
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
            HttpResponseMessage a = await httpclient.GetAsync(url);

            show_header(a.Headers);

            string html = await a.Content.ReadAsStringAsync();

            return html;


        }

        public static async Task<byte[]> dowload_web(string url)
        {
            var httpclient = new HttpClient();
             
            HttpResponseMessage a = await httpclient.GetAsync(url);

            show_header(a.Headers);

            byte[] html = await a.Content.ReadAsByteArrayAsync();

            return html;


        }

        static void show_header(HttpResponseHeaders header)
        {
            Console.WriteLine("CAC HEADER");
            foreach(var item in header)
            {
                Console.Write($"{item.Key}: ");
                foreach (var value in item.Value)
                {
                    Console.Write(value + ", ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("CAC HEADER");

        }

        public static async Task dowload_stream(string url, string file_name)
        {
            HttpClient httpClient = new HttpClient();
            var res = await httpClient.GetAsync(url);
            var stream = await res.Content.ReadAsStreamAsync();

            var stream_write = File.OpenWrite(file_name);

            int stze = 500;
            var buffer = new byte[stze];

            bool flag = false;
            do
            {
                int numbyte = await stream.ReadAsync(buffer, 0, stze);
                if(numbyte == 0)
                {
                    flag = true;
                }
                else
                {
                    await stream_write.WriteAsync(buffer, 0, numbyte);
                }
            }
            while (!flag);

            int num =  await stream.ReadAsync(buffer, 0, stze);
        }

        static async Task Main(string[] args)
        {
            var url = @"https://postman-echo.com/post";

            var handler = new SocketsHttpHandler();

            HttpClient httpClient = new HttpClient();
            HttpRequestMessage httpRequestMessage  = new HttpRequestMessage();   
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(url);

            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("k1", "v1"));
            param.Add(new KeyValuePair<string, string>("k2", "v2"));

            httpRequestMessage.Content = new FormUrlEncodedContent(param);
            var res = await httpClient.SendAsync(httpRequestMessage);

            var html = await res.Content.ReadAsStringAsync();
            Console.WriteLine(html);


            /*
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;

            request.RequestUri = new Uri(@"https://postman-echo.com/post");
            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("k1", "v1"));
            param.Add(new KeyValuePair<string, string>("k2", "v2"));
            param.Add(new KeyValuePair<string, string>("k3", "v3"));

            var content = new FormUrlEncodedContent(param);
            request.Content = content;

            request.Headers.Add("Accept", "application/json");

            var mess = await client.SendAsync(request);
            var html = await mess.Content.ReadAsStringAsync();
            Console.WriteLine(html);

            /*
            var url = @"https://www.hplovecraft.com/writings/texts/fiction/nc.aspx";

            await dowload_stream(url, "2.png");
            /*
            Byte[] b = await dowload_web(url);
            var stream = new FileStream("1.png", FileMode.Create, FileAccess.Write);
            stream.Write(b, 0, b.Length);

            //var task = await get_web_content(url);
            //Console.WriteLine(task);

            /*
            string url = "https://xuanthulab.net/lap-trinh/csharp/?page=3#acff";
            var uri = new Uri(url);
            var uritype = typeof(Uri);
            uritype.GetProperties().ToList().ForEach(property => {
                Console.WriteLine($"{property.Name,15} {property.GetValue(uri)}");
            });
            Console.WriteLine($"Segments: {string.Join(",", uri.Segments)}");
            */
        }
    }
}
