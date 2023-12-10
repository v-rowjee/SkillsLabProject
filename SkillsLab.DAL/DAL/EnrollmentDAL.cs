using Firebase.Auth;
using Firebase.Storage;
using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using SkillsLabProject.Common.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.DAL
{
    public interface IEnrollmentDAL : IDAL<EnrollmentModel>
    {
        bool Add(EnrollmentViewModel model);
        Task<string> UploadAndGetDownloadUrlAsync(FileStream stream, string fileName);
    }
    public class EnrollmentDAL : IEnrollmentDAL
    {
        public IEnumerable<EnrollmentModel> GetAll()
        {
            const string GetAllEnrollmentsQuery = @"
                SELECT EnrollmentId, EmployeeId, EnrollmentId, StatusId
                FROM [dbo].[Enrollment]
            ";
            var dt = DBCommand.GetData(GetAllEnrollmentsQuery);
            var Enrollments = new List<EnrollmentModel>();
            EnrollmentModel Enrollment;
            foreach (DataRow row in dt.Rows)
            {
                Enrollment = new EnrollmentModel();
                Enrollment.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                Enrollment.EmployeeId = int.Parse(row["EmployeeId"].ToString());
                Enrollment.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                Enrollment.Status =  (Status) int.Parse(row["StatusId"].ToString());

                Enrollments.Add(Enrollment);
            }
            return Enrollments;
        }
        public EnrollmentModel GetById(int EnrollmentId)
        {
            const string GetEnrollmentQuery = @"
                SELECT EnrollmentId, EmployeeId, EnrollmentId, StatusId
                FROM [dbo].[Enrollment]
                WHERE [EnrollmentId] = @EnrollmentId
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentId", EnrollmentId)
            };
            var dt = DBCommand.GetDataWithCondition(GetEnrollmentQuery, parameters);
            var enrollment = new EnrollmentModel();
            foreach (DataRow row in dt.Rows)
            {
                enrollment.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                enrollment.EmployeeId = int.Parse(row["EmployeeId"].ToString());
                enrollment.TrainingId = int.Parse(row["TrainingId"].ToString());
                enrollment.Status = (Status) int.Parse(row["PriorityDepartmentId"].ToString());
            }
            return enrollment;
        }
        public bool Add(EnrollmentModel model)
        {
            const string AddEnrollmentQuery = @"
                INSERT [dbo].[Enrollment] (EmployeeId, TrainingId, StatusId) VALUES (@EmployeeId, @TrainingId, @StatusId);
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeId", model.EmployeeId),
                new SqlParameter("@TrainingId", model.TrainingId),
                new SqlParameter("@StatusId", model.Status)
            };
            return DBCommand.InsertUpdateData(AddEnrollmentQuery, parameters);
        }
        public bool Add(EnrollmentViewModel model)
        {
            const string AddEnrollmentQuery = @"
                BEGIN TRANSACTION
                    INSERT INTO [dbo].[Enrollment] (EmployeeId, TrainingId, StatusId) 
                    VALUES (@EmployeeId, @TrainingId, @StatusId);

                    INSERT INTO Proof (EnrollmentId, Attachment)
                    SELECT SCOPE_IDENTITY(), value
                    FROM STRING_SPLIT(@Proofs, ',');
                COMMIT
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeId", model.EmployeeId),
                new SqlParameter("@TrainingId", model.TrainingId),
                new SqlParameter("@StatusId", model.Status),
                new SqlParameter("@Proofs", string.Join(",",model.ProofUrls))
            };
            return DBCommand.InsertUpdateData(AddEnrollmentQuery, parameters);
        }
        public bool Update(EnrollmentModel model)
        {
            const string UpdateEnrollmentQuery = @"
                UPDATE [dbo].[Enrollment]
                SET EmployeeId=@EmployeeId, EnrollmentId=@EnrollmentId, StatusId=@StatusId
                WHERE EnrollmentId=@EnrollmentId;
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentId", model.EnrollmentId),
                new SqlParameter("@Title", model.EmployeeId),
                new SqlParameter("@Deadline", model.TrainingId),
                new SqlParameter("@Capacity", model.Status)
            };
            return DBCommand.InsertUpdateData(UpdateEnrollmentQuery, parameters);
        }
        public bool Delete(int EnrollmentId)
        {
            const string DeleteEnrollmentQuery = @"
                BEGIN TRANSACTION
                    DELETE FROM [dbo].[DeclinedEnrollment] WHERE EnrollmentId=@EnrollmentId;
                    DELETE FROM [dbo].[Proof] WHERE EnrollmentId=@EnrollmentId;
                    DELETE FROM [dbo].[Enrollment] WHERE EnrollmentId=@EnrollmentId
                COMMIT
            ";
            var parameter = new SqlParameter("@EnrollmentId", EnrollmentId);
            return DBCommand.DeleteData(DeleteEnrollmentQuery, parameter);
        }
        public async Task<string> UploadAndGetDownloadUrlAsync(FileStream stream, string fileName)
        {
            var task = new FirebaseStorage("gs://skillslab-89154.appspot.com")
                .Child("images")
                .Child(fileName)
                .PutAsync(stream);

            try
            {
                string downloadUrl = await task;
                return downloadUrl;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}