using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using JumpingUnicorn.Data;

namespace JumpingUnicorn.Database
{
    /// <summary>
    /// This class connects to Firestore and read and write to it. All the methods has to be async because the Firestore Library is forcing you to use async
    /// </summary>
    public class FirebaseContext
    {
        public string ConnectionString { get; private set; } = "jumpingunicorn";
        
        FirestoreDb db;

        public FirebaseContext()
        {
           db = FirestoreDb.Create(ConnectionString);
        }

        private async Task<DocumentSnapshot> GetUserDocumentAsync(string id)
        {
            CollectionReference users = db.Collection("users");
            Query query = users.WhereEqualTo("userId", id);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            return querySnapshot.Documents.First();
        }

        public async Task AddUserAsync(User userInfo)
        {
            CollectionReference collection = db.Collection("users");
            DocumentReference document = collection.Document();
            Dictionary<string, object> user = new Dictionary<string, object>()
            {
                {"userId",userInfo.Id },
                {"username", userInfo.Username },
                {"avatar", userInfo.GoogleAvatar },
                {"highscore", userInfo.Highscore }
            };
            await document.SetAsync(user);
        }

        public async Task<bool> DoesUsernameExistAsync(string username)
        {
            CollectionReference users = db.Collection("users");
            Query query = users.WhereEqualTo("username", username);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            return querySnapshot.Count > 0;
        }

        public async Task<bool> DoesUserExistAsync(string id)
        {
            CollectionReference users = db.Collection("users");
            Query query = users.WhereEqualTo("userId", id);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            return querySnapshot.Count > 0;
        }

        public async Task<bool> DoesUserHasUsernameAsync(string id)
        {
            DocumentSnapshot snapshot = await GetUserDocumentAsync(id);
            string a = snapshot.GetValue<string>("username");
            return !string.IsNullOrEmpty(a);
        }

        public async Task SetUsernameAsync(string id, string username)
        {
            DocumentSnapshot snapshot = await GetUserDocumentAsync(id);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "username", username }
            };
            await snapshot.Reference.UpdateAsync(updates);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            CollectionReference users = db.Collection("users");
            Query query = users.OrderByDescending("highscore").Limit(20);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            List<DocumentSnapshot> documents = querySnapshot.Documents.ToList();
            List<User> usersList = new List<User>();

            foreach (DocumentSnapshot document in documents)
            {
                usersList.Add(document.ConvertTo<User>());
            }
            return usersList;
        }
        
        public async Task SetHighscore(string id, int highscore)
        {
            DocumentSnapshot snapshot = await GetUserDocumentAsync(id);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "highscore", highscore }
            };
            await snapshot.Reference.UpdateAsync(updates);
        }

        public async Task<User> GetUserAsync(string id)
        {
            DocumentSnapshot document = await GetUserDocumentAsync(id);
            return document.ConvertTo<User>();
        }
    }
}
