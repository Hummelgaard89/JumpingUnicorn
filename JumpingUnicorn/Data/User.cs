namespace JumpingUnicorn.Data
{
    public class User
    {
        public string Username { get; set; }

        public string GoogleAvatar { get; set; }

        public string Id { get; set; }


        public User(string username, string googleavatar, string id)
        {
            Username = username;
            GoogleAvatar = googleavatar;
            Id = id;

        }
    }
}
