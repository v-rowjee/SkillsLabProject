using Firebase.Storage;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.DAL
{
    public class Firebase
    {
        private readonly string _bucket;
        public Firebase()
        {
            _bucket = ConfigurationManager.AppSettings["FirebaseBucket"].ToString();
        }

        public async Task<string> UploadFileAsync(FileStream stream, string fileName)
        {
            var task = new FirebaseStorage(_bucket)
                .Child("uploads")
                .Child(fileName)
                .PutAsync(stream);

            string downloadUrl = await task;
            return downloadUrl;

        }
    }
}
