using Microsoft.Net.Http.Headers;
using UAParser;

namespace Middleware
{

    public class CustomMiddle
    {
        RequestDelegate _next;
        public CustomMiddle(RequestDelegate next)
        {
            _next = next;   
        }

        public Task Invoke(HttpContext context)
        {
            Console.WriteLine("request:" + context.Request + " " + DateTime.Now);

            var userAgent = context.Request.Headers["User-Agent"];

            string uaString = Convert.ToString(userAgent[0]);

            var uaParser = Parser.GetDefault();
            ClientInfo client = uaParser.Parse(uaString);
            string browserName = client.UA.Family;

            if (browserName.Contains("IE") || browserName.Contains("Edge"))
            {
                context.Response.Redirect("https://www.mozilla.org/pl/firefox/new/");
            }
            return _next(context);
        }
    }

    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddle>();
        }
    }
}