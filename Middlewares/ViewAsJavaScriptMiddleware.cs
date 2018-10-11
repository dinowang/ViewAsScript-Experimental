using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ViewAsScript.Middlewares
{
    public class ViewAsJavaScriptMiddleware
    {
        private readonly RequestDelegate _next;

        public ViewAsJavaScriptMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBody = context.Response.Body;

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    context.Response.Body = memoryStream;

                    await _next(context);

                    memoryStream.Position = 0;
                    using (var reader = new StreamReader(memoryStream))
                    {
                        var html = reader.ReadToEnd();
                        html = Regex.Replace(html, @"(\r\n|\n|\r)", "\\n"); // 換行字元都改掉
                        html = Regex.Replace(html, @"\""", "\\\"");         // 要考慮逸出字元
                        html = $"document.write(\"{html}\");";

                        context.Response.ContentType = "text/javascript";
            
                        using (var writer = new StreamWriter(originalBody))
                        {
                            await writer.WriteAsync(html);
                        }
                    }
                }
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }
}