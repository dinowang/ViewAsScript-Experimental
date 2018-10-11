using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ViewAsScript.Middlewares
{
    /// <summary>
    /// ViewAsJavaScript middleware 的實驗版本，希望有比較小的 memory footprint
    /// </summary>    
    public class ViewAsJavaScriptExperimentalMiddleware
    {
        private readonly RequestDelegate _next;

        public ViewAsJavaScriptExperimentalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBody = context.Response.Body;

            try
            {
                context.Response.ContentType = "text/javascript";

                using (var stream = new JavaScriptStream(originalBody))
                {
                    context.Response.Body = stream;

                    await _next(context);

                    context.Response.Body = originalBody;
                }
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }
}