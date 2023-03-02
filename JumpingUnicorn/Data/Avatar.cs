namespace JumpingUnicorn.Data
{
    public class Avatar
    {
        public string Path { get; set; }

        public Avatar.AvatarSpeed Speed { get; set; }

        public enum AvatarSpeed
        {
            Slow,
            Medium,
            Fast
        }

        public Avatar(string path, AvatarSpeed avatarSpeed)
        {
            Path = path;
            Speed = avatarSpeed;
        }
    }
}
