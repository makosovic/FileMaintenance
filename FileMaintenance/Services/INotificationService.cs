
namespace FileMaintenance.Services
{
    public interface INotificationService
    {
        void Send(string title, string message);
        void SendAsync(string title, string message);
    }
}
