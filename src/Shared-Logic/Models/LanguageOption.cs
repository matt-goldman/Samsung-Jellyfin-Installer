namespace Samsung_Jellyfin_Installer.Shared.Models
{
    public class LanguageOption
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public override string ToString() => Name;

        public override bool Equals(object? obj)
        {
            if (obj is LanguageOption other)
            {
                return Code == other.Code;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Code?.GetHashCode() ?? 0;
        }
    }
}
