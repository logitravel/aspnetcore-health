namespace AspNetCore.Health
{
    public class HealthCheckResult
    {
        public string Name { get; }

        public HealthCheckStatus Status { get; }
        /// <summary>
        /// Set as false if that service does not critical to your application. Default is true.
        /// </summary>
        public bool IsCritical { get; }

        public HealthCheckResult(string name, HealthCheckStatus status, bool isCritical)
        {
            Name = name;
            Status = status;
            IsCritical = isCritical;
        }

        public static HealthCheckResult Healthy(string description, bool isCritical = true)
        {
            return new HealthCheckResult($"{description}", HealthCheckStatus.Healthy, isCritical);
        }

        public static HealthCheckResult Unhealthy(string description, bool isCritical = true)
        {
            return new HealthCheckResult($"{description}", HealthCheckStatus.Unhealthy, isCritical);
        }

        public static HealthCheckResult Warning(string description, bool isCritical = true)
        {
            return new HealthCheckResult($"{description}", HealthCheckStatus.Warning, isCritical);
        }
    }
}