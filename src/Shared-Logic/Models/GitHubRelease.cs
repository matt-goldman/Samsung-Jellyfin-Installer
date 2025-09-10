using Newtonsoft.Json;

namespace Samsung_Jellyfin_Installer.Shared.Models
{
    public class GitHubRelease
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("tag_name")]
        public string TagName { get; set; } = string.Empty;

        [JsonProperty("published_at")]
        public string PublishedAt { get; set; } = string.Empty;

        [JsonProperty("assets")]
        public List<Asset> Assets { get; set; } = new List<Asset>();

        public string? PrimaryDownloadUrl => Assets?.FirstOrDefault()?.DownloadUrl;
    }

    public class Asset
    {
        [JsonProperty("name")]
        public string FileName { get; set; } = string.Empty;

        [JsonProperty("browser_download_url")]
        public string DownloadUrl { get; set; } = string.Empty;

        [JsonProperty("size")]
        public long Size { get; set; }

        // Friendly display text
        public string DisplayText => $"{FileName} ({FormatFileSize(Size)})";

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double len = bytes;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}
