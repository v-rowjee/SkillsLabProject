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
                SELECT EnrollmentId, EmployeeId, TrainingId, StatusId, UpdatedOn
                FROM [dbo].[Enrollment]
            ";
            var dt = DBCommand.GetData(GetAllEnrollmentsQuery);
            var enrollments = new List<EnrollmentModel>();
            EnrollmentModel enrollment;
            foreach (DataRow row in dt.Rows)
            {
                enrollment = new EnrollmentModel();
                enrollment.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                enrollment.EmployeeId = int.Parse(row["EmployeeId"].ToString());
                enrollment.TrainingId = int.Parse(row["TrainingId"].ToString());
                enrollment.Status =  (Status) int.Parse(row["StatusId"].ToString());
                enrollment.UpdatedOn = DateTime.Parse(row["UpdatedOn"].ToString());
                enrollments.Add(enrollment);
            }
            return enrollments;
        }
        public EnrollmentModel GetById(int EnrollmentId)
        {
            const string GetEnrollmentQuery = @"
                SELECT EnrollmentId, EmployeeId, TrainingId, StatusId, UpdatedOn
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
                enrollment.Status = (Status) int.Parse(row["StatusId"].ToString());
                enrollment.UpdatedOn = DateTime.Parse(row["UpdatedOn"].ToString());
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
                new SqlParameter("@EmployeeId", model.Employee.EmployeeId),
                new SqlParameter("@TrainingId", model.Training.TrainingId),
                new SqlParameter("@StatusId", model.Status),
                new SqlParameter("@Proofs", string.Join(",",model.Proofs.Select(p => p.Attachment)))
            };
            return DBCommand.InsertUpdateData(AddEnrollmentQuery, parameters);
        }
        public bool Update(EnrollmentModel model)
        {
            const string UpdateEnrollmentQuery = @"
                BEGIN TRANSACTION
                    IF EXISTS (SELECT 1 FROM [dbo].[Status] WHERE StatusId = @StatusId)
                    BEGIN
                        UPDATE [dbo].[Enrollment]
                        SET StatusId=@StatusId
                        WHERE EnrollmentId=@EnrollmentId;
                    END

                    IF @StatusId = 3     -- Declined
                    BEGIN
                        UPDATE [dbo].[DeclinedEnrollment]
                        SET Reason=@Reason
                        WHERE EnrollmentId=@EnrollmentId;

                        IF @@ROWCOUNT = 0
                        BEGIN
                            INSERT INTO [dbo].[DeclinedEnrollment] (EnrollmentId, Reason)
                            VALUES (@EnrollmentId, @Reason);
                        END
                    END
                    ELSE
                    BEGIN
                        DELETE FROM [dbo].[DeclinedEnrollment]
                        WHERE EnrollmentId = @EnrollmentId;
                    END
                COMMIT
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentId", model.EnrollmentId),
                new SqlParameter("@StatusId", (int) model.Status)
            };
            if (model.DeclinedReason != null)
            {
                parameters.Add(new SqlParameter("@Reason", model.DeclinedReason));
            }
            else
            {
                parameters.Add(new SqlParameter("@Reason", DBNull.Value));
            }
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
            Firebase firebase = new Firebase();
            return await firebase.UploadFileAsync(stream, fileName);
        }
    }
}