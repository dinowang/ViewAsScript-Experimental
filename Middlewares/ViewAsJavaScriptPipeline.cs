using Microsoft.AspNetCore.Builder;

namespace ViewAsScript.Middlewares
{
    public class ViewAsJavaScriptPipeline
    {
        public void Configure(IApplicationBuilder builder)
        {
            builder.UseMiddleware<ViewAsJavaScriptMiddleware>();
            // builder.UseMiddleware<ViewAsJavaScriptExperimentalMiddleware>();
        }
    }
}