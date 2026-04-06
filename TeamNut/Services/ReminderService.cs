using Microsoft.WindowsAppSDK.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamNut.Models;
using TeamNut.Repositories;

namespace TeamNut.Services
{
    public class ReminderService
    {
        private readonly ReminderRepository _reminderRepository;
        public static event EventHandler<int>? RemindersChanged;

        public ReminderService()
        {
            _reminderRepository = new ReminderRepository();
            
        }

        public async Task<Reminder?> GetNextReminder(int userId)
        {
            return await _reminderRepository.GetNextReminder(userId);
        }

        
        public async Task<string> SaveReminder(Reminder reminder)
        {
            
            if ((reminder.UserId == 0 || reminder.UserId == default) && TeamNut.Models.UserSession.UserId != null)
            {
                reminder.UserId = TeamNut.Models.UserSession.UserId ?? reminder.UserId;
            }

            if (string.IsNullOrWhiteSpace(reminder.Name) || reminder.Name.Length > 50)
            {
                return "Error: Name must be between 1 and 50 characters.";
            }

            if (reminder.Id == 0)
                await _reminderRepository.Add(reminder);
            else
                await _reminderRepository.Update(reminder);

            return "Success";

            
          
        }

        
        public async Task ConfirmConsumption(int userId, int mealId)
        {

            Console.WriteLine($"User {userId} confirmed meal {mealId}. Updating logs...");
        }

        public async Task<IEnumerable<Reminder>> GetUserReminders(int userId)
        {
            
            return await _reminderRepository.GetAllByUserId(userId);
        }
        

        public async Task DeleteReminder(int id)
        {
            await _reminderRepository.Delete(id);
        }
    }
}
