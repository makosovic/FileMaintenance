
namespace FileMaintenance.Services
{
    public interface INotificationService
    {
        /// <summary>
        /// Send a message.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        void Send(string title, string message);

        /// <summary>
        /// Send a message asynchronously.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        void SendAsync(string title, string message);
    }
}
