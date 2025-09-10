namespace Samsung_Jellyfin_Installer.Shared.Models
{
    public class SamsungAuth
    {
        public string access_token { get; set; } = string.Empty;
        public string token_type { get; set; } = string.Empty;
        public string access_token_expires_in { get; set; } = string.Empty;
        public string refresh_token { get; set; } = string.Empty;
        public string refresh_token_expires_in { get; set; } = string.Empty;
        public string userId { get; set; } = string.Empty;
        public string client_id { get; set; } = string.Empty;
        public string inputEmailID { get; set; } = string.Empty;
        public string api_server_url { get; set; } = string.Empty;
        public string auth_server_url { get; set; } = string.Empty;
        public bool close { get; set; }
        public string closedAction { get; set; } = string.Empty;
        public string state { get; set; } = string.Empty;
    }
}
