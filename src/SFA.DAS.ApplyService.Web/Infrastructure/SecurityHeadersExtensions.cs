using Microsoft.AspNetCore.Builder;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public static class SecurityHeadersExtensions
    {
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, string cdnBaseUrl)
        {
            var frontEndCdn = string.IsNullOrWhiteSpace(cdnBaseUrl) ? string.Empty : cdnBaseUrl.TrimEnd('/');

            app.Use(async (context, next) =>
            {
                context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
                context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                context.Response.Headers["Content-Security-Policy"] =
                    $"default-src 'self' {frontEndCdn}; " +
                    $"img-src 'self' {frontEndCdn} *.googletagmanager.com *.google-analytics.com; " +
                    $"script-src 'self' 'unsafe-inline' 'unsafe-eval' {frontEndCdn} *.googletagmanager.com *.google-analytics.com *.googleapis.com; " +
                    $"style-src 'self' {frontEndCdn}; " +
                    $"font-src 'self' data: {frontEndCdn}; " +
                    "connect-src 'self' 'unsafe-inline' *.googletagmanager.com *.google-analytics.com;";
                context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
                context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.Response.Headers["X-Permitted-Cross-Domain-Policies"] = "none";
                context.Response.Headers["Pragma"] = "no-cache";
                await next();
            });

            return app;
        }
    }
}
