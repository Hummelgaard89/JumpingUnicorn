using JumpingUnicorn.Database;
using Microsoft.AspNetCore.Http;

namespace JumpingUnicorn.Data
{
    public class AvatarService
    {
        public List<AvatarCollection> Avatars = new List<AvatarCollection>();

        private readonly IHttpContextAccessor _httpContextAccessor;


        public AvatarService(IHttpContextAccessor httpContextAccessor) 
        { 
            _httpContextAccessor = httpContextAccessor;
            GetAllAvatars();
        }

        void GetAllAvatars()
        {
            string[] paths = Directory.GetFiles(Directory.GetCurrentDirectory() + "/wwwroot/Images/Unicorns", "*.gif");

            foreach (string path in paths)
            {
                string filename = Path.GetFileName(path);
                int index = 0;

                int numberIndex = 0;

                foreach (char c in filename)
                {
                    if (int.TryParse(c.ToString(), out index))
                    {
                        if (!Avatars.Exists(x => x.Index == index))
                        {
                            Avatars.Add(new AvatarCollection(new List<Avatar>(), index));
                        }
                        break;
                    }
                    numberIndex++;
                }
                
                string type = filename.Substring(numberIndex, filename.Length - numberIndex);
                Avatar.AvatarSpeed foundSpeed = Avatar.AvatarSpeed.Slow;
                if (type.Contains("Fast"))
                {
                    foundSpeed = Avatar.AvatarSpeed.Fast;
                }
                else if (type.Contains("Medium"))
                {
                    foundSpeed = Avatar.AvatarSpeed.Medium;

                }
                else if (type.Contains("Slow"))
                {
                    foundSpeed = Avatar.AvatarSpeed.Slow;
                }

                Avatars.Find(x => x.Index == index).AvatarsPaths.Add(new Avatar("/Images/Unicorns/" + filename, foundSpeed));

            }
        }

        public string FindAvatar(Avatar.AvatarSpeed wantedAvatarSpeed = Avatar.AvatarSpeed.Medium)
        {   
            int avatarIndex = 0;
            if (_httpContextAccessor.HttpContext == null)
                return GetAvatar(0, wantedAvatarSpeed);

            if (_httpContextAccessor.HttpContext.Request.Cookies["avatar"] == null)
                return GetAvatar(0, wantedAvatarSpeed);

            if (!int.TryParse(_httpContextAccessor.HttpContext.Request.Cookies["avatar"], out avatarIndex))
                return GetAvatar(0, wantedAvatarSpeed);

            if (Avatars.Count < avatarIndex || avatarIndex <= -1)
                return GetAvatar(0, wantedAvatarSpeed);

            return GetAvatar(avatarIndex, wantedAvatarSpeed);
        }

        string GetAvatar(int index, Avatar.AvatarSpeed wantedAvatarSpeed)
        {
            return Avatars[index].AvatarsPaths.Find(x => x.Speed == wantedAvatarSpeed).Path;
        }
    }
}
