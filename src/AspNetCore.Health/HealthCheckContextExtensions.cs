using System.Text;
using AspNetCore.Health.Internal;

namespace AspNetCore.Health
{
    public static class HealthCheckContextExtensions
    {
        public static HealthCheckContext AddUrlCheck(
            this HealthCheckContext checkContext,
            string url, bool isCritical = true)
        {
            checkContext.Add(url, async () =>
            {
                try
                {
                    var response = await HttpClientSingleton.Instance.GetAsync(url).ConfigureAwait(false);

                    return response.IsSuccessStatusCode
                        ? HealthCheckResult.Healthy($"{url}", isCritical)
                        : HealthCheckResult.Unhealthy($"{url}");
                }
                catch
                {
                    return HealthCheckResult.Unhealthy($"{url}", isCritical);
                }
            });

            return checkContext;
        }
        /// <summary>
        /// Checks an array of urls.
        /// </summary>
        /// <param name="checkContext"></param>
        /// <param name="urls"></param>
        /// <param name="group"></param>
        /// <param name="isCritical">Mark false if that array does not break your application</param>
        /// <returns></returns>
        public static HealthCheckContext AddUrlChecks(
            this HealthCheckContext checkContext,
            string[] urls,
            string group, bool isCritical = true)
        {
            checkContext.Add(group, async () =>
            {
                var successfulChecks = 0;
                var description = new StringBuilder();

                foreach (var url in urls)
                {
                    try
                    {
                        var response = await HttpClientSingleton.Instance.GetAsync(url).ConfigureAwait(false);

                        if (response.IsSuccessStatusCode)
                        {
                            successfulChecks++;
                            description.Append($"{url} OK");
                        }
                        else
                        {
                            description.Append($"{url} ERROR");
                        }
                    }
                    catch
                    {
                        description.Append($"{url} ERROR");
                    }
                }

                if (successfulChecks == urls.Length)
                {
                    return HealthCheckResult.Healthy(description.ToString(), isCritical);
                }

                if (successfulChecks > 0)
                {
                    return HealthCheckResult.Warning(description.ToString(), isCritical);
                }

                return HealthCheckResult.Unhealthy(description.ToString(), isCritical);
            });

            return checkContext;
        }
    }
}
