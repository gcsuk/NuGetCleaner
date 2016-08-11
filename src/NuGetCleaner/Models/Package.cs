namespace NuGetCleaner.Models
{
    public class Package
    {
        public string Id { get; set; }

        public string Version { get; set; }

        public string DirectoryName => $"{Id}.{Version}";
    }
}
