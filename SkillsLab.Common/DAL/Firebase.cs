using Firebase.Auth;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.DAL
{
    public class Firebase
    {
        private readonly string _apiKey;
        private readonly string _bucket;
        private readonly string _email;
        private readonly string _password;
        public Firebase()
        {
            _apiKey = ConfigurationManager.AppSettings["FirebaseApiKey"].ToString();
            _bucket = ConfigurationManager.AppSettings["FirebaseBucket"].ToString();
            _email = ConfigurationManager.AppSettings["AdminEmail"].ToString();
            _password = ConfigurationManager.AppSettings["AdminPassword"].ToString();
        }

        public async Task<string> UploadFileAsync(FileStream stream, string fileName)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_apiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(_email, _password);

            var task = new FirebaseStorage(
                _bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true,
                })
                .Child("images")
                .Child(fileName)
                .PutAsync(stream);

            string downloadUrl = await task;
            return downloadUrl;

        }
    }
}
