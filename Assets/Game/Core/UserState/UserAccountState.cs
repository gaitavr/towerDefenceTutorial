namespace Core
{
    public sealed class UserAccountState
    {
        public string Id { get; set; }
        public string FacebookId { get; set; }
        public string Name { get; set; }
        public string AvatarPath { get; set; }

        public bool IsFacebook => string.IsNullOrWhiteSpace(FacebookId) == false;
    }
}