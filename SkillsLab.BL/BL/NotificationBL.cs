using SkillsLab.Common.Models;
using SkillsLabProject.Common.Custom;
using SkillsLabProject.Common.Models;
using SkillsLabProject.DAL.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillsLabProject.BL.BL
{
    public interface INotificationBL
    {
        Task<bool> SendNotificationAsync(NotificationModel notif);
        Task<IEnumerable<NotificationModel>> GetAllByEmployeeAsync(EmployeeModel employee);
        Task<Result> MarkAsReadAsync(int notificationId);
        Task<Result> DeleteAsync(int notificationId);
    }
    public class NotificationBL : INotificationBL
    {
        private readonly INotificationDAL _notificationDAL;

        public NotificationBL(INotificationDAL notificationDAL)
        {
            _notificationDAL = notificationDAL;
        }

        public async Task<Result> DeleteAsync(int notificationId)
        {
            var result = await _notificationDAL.DeleteAsync(notificationId);
            return new Result()
            {
                IsSuccess = result,
                Message = result ? "Notification deleted" : "Enable to delete notification."
            };
        }

        public async Task<IEnumerable<NotificationModel>> GetAllByEmployeeAsync(EmployeeModel employee)
        {
            return await _notificationDAL.GetAllByEmployeeAsync(employee);
        }

        public async Task<Result> MarkAsReadAsync(int notificationId)
        {
            var notif = await _notificationDAL.GetByIdAsync(notificationId);
            notif.IsRead = true;
            var result = await _notificationDAL.UpdateAsync(notif);

            return new Result()
            {
                IsSuccess = result,
                Message = result ? "Marked as read." : "Enable to mark as read."
            };
        }

        public async Task<bool> SendNotificationAsync(NotificationModel notif)
        {
            return await _notificationDAL.AddAsync(notif);
        }
    }
}
