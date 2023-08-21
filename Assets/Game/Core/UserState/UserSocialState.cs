using System;

namespace Core
{
    [Serializable]
    public sealed class UserSocialState
    {
        public string FacebookId;
        public string Name;
        public string AvatarPath;

        public bool IsFacebook => string.IsNullOrWhiteSpace(FacebookId) == false;
    }
}
