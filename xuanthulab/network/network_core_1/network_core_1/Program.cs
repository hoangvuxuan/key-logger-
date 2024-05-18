// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Text;

Console.WriteLine("Hello, World!");

var url = "https://google.com";
var cokies = new CookieContainer();

/*
SocketsHttpHandler handler = new SocketsHttpHandler();
handler.AllowAutoRedirect = true;
handler.AutomaticDecompression = System.Net.DecompressionMethods.Deflate;
handler.UseCookies = true;
handler.CookieContainer = cokies;
*/

var bottomHandler = new MyHttpClientHandler(cokies);              // handler đáy (cuối)
var changeUriHandler = new ChangeUri(bottomHandler);
var denyAccessFacebook = new DenyAccessFacebook(changeUriHandler); // handler đỉnh

HttpClient client = new HttpClient(denyAccessFacebook);
HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

var param = new List<KeyValuePair<string, string>>();
param.Add(new KeyValuePair<string, string>("k1", "v1"));
param.Add(new KeyValuePair<string, string>("k2", "v2"));

request.Content = new FormUrlEncodedContent(param);
var res = await client.SendAsync(request);

foreach (var item in cokies.GetCookies(new Uri(url)).ToList())
{
    Console.WriteLine(item.Name + "  " + item.Value);
}
var html = await res.Content.ReadAsStringAsync();

Console.WriteLine(html);



public class MyHttpClientHandler : HttpClientHandler
{
    public MyHttpClientHandler(CookieContainer cookie_container)
    {

        
        CookieContainer = cookie_container;     // Thay thế CookieContainer mặc định
        AllowAutoRedirect = false;                // không cho tự động Redirect
        AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
        UseCookies = true;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken)
    {
        Console.WriteLine("Bất đầu kết nối " + request.RequestUri.ToString());
        // Thực hiện truy vấn đến Server
        var response = await base.SendAsync(request, cancellationToken);
        Console.WriteLine("Hoàn thành tải dữ liệu");
        return response;
    }
}

public class ChangeUri : DelegatingHandler
{
    public ChangeUri(HttpMessageHandler innerHandler) : base(innerHandler) { }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                           CancellationToken cancellationToken)
    {
        var host = request.RequestUri.Host.ToLower();
        Console.WriteLine($"Check in  ChangeUri - {host}");
        if (host.Contains("google.com"))
        {
            // Đổi địa chỉ truy cập từ google.com sang github
            request.RequestUri = new Uri("https://github.com/");
        }
        // Chuyển truy vấn cho base (thi hành InnerHandler)
        return base.SendAsync(request, cancellationToken);
    }
}


public class DenyAccessFacebook : DelegatingHandler
{
    public DenyAccessFacebook(HttpMessageHandler innerHandler) : base(innerHandler) { }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken)
    {

        var host = request.RequestUri.Host.ToLower();
        Console.WriteLine($"Check in DenyAccessFacebook - {host}");
        if (host.Contains("facebook.com"))
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(Encoding.UTF8.GetBytes("Không được truy cập"));
            return await Task.FromResult<HttpResponseMessage>(response);
        }
        // Chuyển truy vấn cho base (thi hành InnerHandler)
        return await base.SendAsync(request, cancellationToken);
    }
}