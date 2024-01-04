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
        Task<bool> AddAsync(EnrollmentViewModel model);
        Task<string> UploadAndGetDownloadUrlAsync(FileStream stream, string fileName);
    }
    public class EnrollmentDAL : IEnrollmentDAL
    {
        public async Task<IEnumerable<EnrollmentModel>> GetAllAsync()
        {
            const string GetAllEnrollmentsQuery = @"
                SELECT EnrollmentId, EmployeeId, TrainingId, StatusId, UpdatedOn, CreatedOn
                FROM [dbo].[Enrollment]
            ";

            var enrollments = new List<EnrollmentModel>();

            using (SqlDataReader dataReader = await DBCommand.GetDataAsync(GetAllEnrollmentsQuery).ConfigureAwait(false))
            {
                while (await dataReader.ReadAsync().ConfigureAwait(false))
                {
                    var enrollment = new EnrollmentModel
                    {
                        EnrollmentId = dataReader.GetInt32(dataReader.GetOrdinal("EnrollmentId")),
                        EmployeeId = dataReader.GetInt32(dataReader.GetOrdinal("EmployeeId")),
                        TrainingId = dataReader.GetInt32(dataReader.GetOrdinal("TrainingId")),
                        Status = (Status)dataReader.GetInt32(dataReader.GetOrdinal("StatusId")),
                        UpdatedOn = dataReader.GetDateTime(dataReader.GetOrdinal("UpdatedOn")),
                        CreatedOn = dataReader.GetDateTime(dataReader.GetOrdinal("CreatedOn"))
                    };

                    enrollments.Add(enrollment);
                }
            }

            return enrollments;
        }

        public async Task<EnrollmentModel> GetByIdAsync(int enrollmentId)
        {
            const string GetEnrollmentQuery = @"
                SELECT EnrollmentId, EmployeeId, TrainingId, StatusId, UpdatedOn, CreatedOn
                FROM [dbo].[Enrollment]
                WHERE [EnrollmentId] = @EnrollmentId
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentId", enrollmentId)
            };

            var enrollment = new EnrollmentModel();

            using (SqlDataReader dataReader = await DBCommand.GetDataWithConditionAsync(GetEnrollmentQuery, parameters).ConfigureAwait(false))
            {
                while (await dataReader.ReadAsync().ConfigureAwait(false))
                {
                    enrollment.EnrollmentId = dataReader.GetInt32(dataReader.GetOrdinal("EnrollmentId"));
                    enrollment.EmployeeId = dataReader.GetInt32(dataReader.GetOrdinal("EmployeeId"));
                    enrollment.TrainingId = dataReader.GetInt32(dataReader.GetOrdinal("TrainingId"));
                    enrollment.Status = (Status)dataReader.GetInt32(dataReader.GetOrdinal("StatusId"));
                    enrollment.UpdatedOn = dataReader.GetDateTime(dataReader.GetOrdinal("UpdatedOn"));
                    enrollment.CreatedOn = dataReader.GetDateTime(dataReader.GetOrdinal("CreatedOn"));
                }
            }

            return enrollment;
        }

        public async Task<bool> AddAsync(EnrollmentModel model)
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

            return await DBCommand.InsertDataAsync(AddEnrollmentQuery, parameters).ConfigureAwait(false);
        }

        public async Task<bool> AddAsync(EnrollmentViewModel model)
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
                new SqlParameter("@Proofs", string.Join(",", model.Proofs.Select(p => p.Attachment)))
            };

            return await DBCommand.InsertDataAsync(AddEnrollmentQuery, parameters).ConfigureAwait(false);
        }

        public async Task<bool> UpdateAsync(EnrollmentModel model)
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
                new SqlParameter("@StatusId", (int)model.Status)
            };

            if (model.DeclinedReason != null)
            {
                parameters.Add(new SqlParameter("@Reason", model.DeclinedReason));
            }
            else
            {
                parameters.Add(new SqlParameter("@Reason", DBNull.Value));
            }

            return await DBCommand.UpdateDataAsync(UpdateEnrollmentQuery, parameters).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(int enrollmentId)
        {
            const string DeleteEnrollmentQuery = @"
                BEGIN TRANSACTION
                    DELETE FROM [dbo].[DeclinedEnrollment] WHERE EnrollmentId=@EnrollmentId;
                    DELETE FROM [dbo].[Proof] WHERE EnrollmentId=@EnrollmentId;
                    DELETE FROM [dbo].[Enrollment] WHERE EnrollmentId=@EnrollmentId
                COMMIT
            ";

            var parameter = new SqlParameter("@EnrollmentId", enrollmentId);

            return await DBCommand.DeleteDataAsync(DeleteEnrollmentQuery, parameter).ConfigureAwait(false);
        }

        public async Task<string> UploadAndGetDownloadUrlAsync(FileStream stream, string fileName)
        {
            Firebase firebase = new Firebase();
            return await firebase.UploadFileAsync(stream, fileName).ConfigureAwait(false);
        }
    }
}