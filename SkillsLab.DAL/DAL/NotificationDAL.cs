using SkillsLab.Common.Models;
using SkillsLabProject.Common.Custom;
using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.DAL
{
    public interface INotificationDAL
    {
        Task<bool> AddAsync(NotificationModel notif);
        Task<IEnumerable<NotificationModel>> GetAllByEmployeeAsync(EmployeeModel employee);
        Task<NotificationModel> GetByIdAsync(int id);
        Task<bool> UpdateAsync(NotificationModel notif);
        Task<bool> DeleteAsync(int notificationId);
    }
    public class NotificationDAL : INotificationDAL
    {
        public async Task<bool> AddAsync(NotificationModel notif)
        {
            const string AddNotificationQuery = @"
                INSERT Notification (EmployeeId, EmployeeRoleId, Message)
                VALUES (@EmployeeId, @EmployeeRoleId, @Message)
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeId", notif.EmployeeId),
                new SqlParameter("@EmployeeRoleId", notif.EmployeeRole),
                new SqlParameter("@Message", notif.Message),
            };

            return await DBCommand.InsertDataAsync(AddNotificationQuery, parameters).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(int notificationId)
        {
            const string DeleteNotificationQuery = @"
                DELETE FROM Notification
                WHERE NotificationId = @NotificationId
            ";
            var parameter = new SqlParameter("@NotificationId", notificationId);

            return await DBCommand.DeleteDataAsync(DeleteNotificationQuery, parameter).ConfigureAwait(false);
        }

        public async Task<IEnumerable<NotificationModel>> GetAllByEmployeeAsync(EmployeeModel employee)
        {
            const string GetAllNotificationQuery = @"
                SELECT NotificationId, EmployeeId, EmployeeRoleId, Message, IsRead, ReceivedOn
                FROM Notification 
                WHERE EmployeeId = @EmployeeId
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeId", employee.EmployeeId),
            };

            var notifications = new List<NotificationModel>();

            using (SqlDataReader dataReader = await DBCommand.GetDataWithConditionAsync(GetAllNotificationQuery, parameters).ConfigureAwait(false))
            {
                while (await dataReader.ReadAsync().ConfigureAwait(false))
                {
                    var notification = new NotificationModel
                    {
                        NotificationId = dataReader.GetInt16(dataReader.GetOrdinal("NotificationId")),
                        EmployeeId = dataReader.GetInt16(dataReader.GetOrdinal("EmployeeId")),
                        EmployeeRole = (Role)dataReader.GetByte(dataReader.GetOrdinal("EmployeeRoleId")),
                        Message = dataReader.GetString(dataReader.GetOrdinal("Message")),
                        ReceivedOn = dataReader.GetDateTime(dataReader.GetOrdinal("ReceivedOn")),
                        IsRead = dataReader.GetBoolean(dataReader.GetOrdinal("IsRead"))
                    };

                    notifications.Add(notification);
                }
            }
            return notifications;
        }

        public async Task<NotificationModel> GetByIdAsync(int id)
        {
            const string GetNotificationQuery = @"
                SELECT NotificationId, EmployeeId, EmployeeRoleId, Message, IsRead, ReceivedOn
                FROM Notification 
                WHERE NotificationId = @NotificationId
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@NotificationId", id),
            };

            using (SqlDataReader dataReader = await DBCommand.GetDataWithConditionAsync(GetNotificationQuery, parameters).ConfigureAwait(false))
            {
                var notification = new NotificationModel();

                if (await dataReader.ReadAsync().ConfigureAwait(false))
                {
                    notification = new NotificationModel()
                    {
                        NotificationId = dataReader.GetInt16(dataReader.GetOrdinal("NotificationId")),
                        EmployeeId = dataReader.GetInt16(dataReader.GetOrdinal("EmployeeId")),
                        EmployeeRole = (Role)dataReader.GetByte(dataReader.GetOrdinal("EmployeeRoleId")),
                        Message = dataReader.GetString(dataReader.GetOrdinal("Message")),
                        ReceivedOn = dataReader.GetDateTime(dataReader.GetOrdinal("ReceivedOn")),
                        IsRead = dataReader.GetBoolean(dataReader.GetOrdinal("IsRead"))
                    };
                }
                return notification;
            }
        }
        public async Task<bool> UpdateAsync(NotificationModel notif)
        {
            const string UpdateAllNotificationQuery = @"
                UPDATE Notification
                SET EmployeeId=@EmployeeId, EmployeeRoleId=@EmployeeRoleId, Message=@Message, IsRead=@IsRead
                WHERE NotificationId=@NotificationId
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@NotificationId", notif.NotificationId),
                new SqlParameter("@EmployeeId", notif.EmployeeId),
                new SqlParameter("@EmployeeRoleId", notif.EmployeeRole),
                new SqlParameter("@Message", notif.Message),
                new SqlParameter("@IsRead", notif.IsRead),
            };

            return await DBCommand.UpdateDataAsync(UpdateAllNotificationQuery, parameters).ConfigureAwait(false);

        }
    }
}
