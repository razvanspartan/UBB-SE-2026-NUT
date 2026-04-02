using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TeamNut.Models;
using TeamNut.Services;

namespace TeamNut.ViewModels
{
    public partial class NutritionistChatViewModel : ObservableObject
    {
        private readonly NutritionistChatService _service;

        public NutritionistChatViewModel()
        {
            _service = new NutritionistChatService();
            Conversations = new ObservableCollection<NutritionistChatConversation>();
            Messages = new ObservableCollection<NutritionistChatMessage>();
        }

        [ObservableProperty]
        private ObservableCollection<NutritionistChatConversation> conversations;

        [ObservableProperty]
        private ObservableCollection<NutritionistChatMessage> messages;

        [ObservableProperty]
        private NutritionistChatConversation selectedConversation;

        [ObservableProperty]
        private string inputText = string.Empty;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        [ObservableProperty]
        private bool isSending;

        [RelayCommand]
        public async Task LoadConversations()
        {
            Conversations.Clear();
            var convs = UserSession.Role == "Nutritionist" ? await _service.GetAllConversationsAsync() : await _service.GetConversationsForCurrentUserAsync();
            foreach (var c in convs.OrderBy(c => c.CreatedAt)) Conversations.Add(c);
        }

        [RelayCommand]
        public async Task LoadMessages()
        {
            if (SelectedConversation == null) return;
            Messages.Clear();
            var msgs = await _service.GetMessagesAsync(SelectedConversation.Id);
            foreach (var m in msgs) Messages.Add(m);
        }

        [RelayCommand(CanExecute = nameof(CanSend))]
        public async Task SendMessage()
        {
            if (SelectedConversation == null)
            {
                StatusMessage = "No conversation selected.";
                return;
            }

            try
            {
                IsSending = true;
                await _service.SendMessageAsync(SelectedConversation.Id, InputText);
                InputText = string.Empty;
                await LoadMessages();
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
            finally
            {
                IsSending = false;
            }
        }

        public bool CanSend()
        {
            if (string.IsNullOrWhiteSpace(InputText)) return false;
            if (InputText.Length > 300) return false; // UI-level fallback rule
            return true;
        }
    }
}
