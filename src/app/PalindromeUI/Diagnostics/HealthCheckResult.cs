namespace PalindromeUI.Diagnostics
{
    public class HealthCheckResult
    {
        public string AssemblyName { get; set; }
        public string Version { get; set; }
        public bool Healthy { get; set; }
    }
}