namespace JumpingUnicorn.Data
{
    public class AvatarCollection
    {
        public List<Avatar> AvatarsPaths { get; set; }

        public int Index { get; set; }

        public AvatarCollection(List<Avatar> avatarPaths, int index)
        {
            AvatarsPaths = avatarPaths;
            Index = index;
        }

    }
}
