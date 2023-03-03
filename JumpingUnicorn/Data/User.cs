using Google.Cloud.Firestore;

namespace JumpingUnicorn.Data
{
    /// <summary>
    /// This class is for storing the user data and holds data till it is sent to Firebase
    /// </summary>
    [FirestoreData]
    public class User
    {

        [FirestoreProperty("username")]
        public string Username { get; set; }

        [FirestoreProperty("avatar")]
        public string GoogleAvatar { get; set; }

        [FirestoreProperty("id")]
        public string Id { get; set; }

        [FirestoreProperty("highscore")]
        public int Highscore { get; set; }
    }
}
