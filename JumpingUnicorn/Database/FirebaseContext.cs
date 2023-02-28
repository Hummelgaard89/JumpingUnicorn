using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using JumpingUnicorn.Data;

namespace JumpingUnicorn.Database
{
    public class FirebaseContext
    {
        public string ConnectionString { get; private set; } = "jumpingunicorn";
        
        FirestoreDb db;


        public FirebaseContext()
        {
           db = FirestoreDb.Create(ConnectionString);
        }

        public async Task AddUserAsync(User userInfo)
        {
            CollectionReference collection = db.Collection("users");
            DocumentReference document = collection.Document();
            Dictionary<string, object> user = new Dictionary<string, object>()
            {
                {"userId",userInfo.Id },
                {"username", userInfo.Username },
                {"avatar", userInfo.GoogleAvatar }
            };
            await document.SetAsync(user);
        }

        public async Task<bool> DoesUsernameExistAsync(string username)
        {
            CollectionReference citiesRef = db.Collection("users");
            Query query = citiesRef.WhereEqualTo("username", username);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            return querySnapshot.Count > 0;
        }

        public async Task<bool> DoesUserExistAsync(string id)
        {
            CollectionReference citiesRef = db.Collection("users");
            Query query = citiesRef.WhereEqualTo("userId", id);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            return querySnapshot.Count > 0;
        }

        public async Task<bool> DoesUserHasUsername(string id)
        {
            CollectionReference citiesRef = db.Collection("users");
            Query query = citiesRef.WhereEqualTo("userId", id);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            DocumentSnapshot document = querySnapshot.Documents.First();
            string a = document.GetValue<string>("username");
            return string.IsNullOrEmpty(a);
        }
    }
}
