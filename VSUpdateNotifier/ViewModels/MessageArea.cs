using VsUpdateNotifier.Views;

namespace VsUpdateNotifier.ViewModels
{
    public class MessageArea
    {
        public MessageArea(MainWindow window)
        {
            this.window = window;
        }

        private MainWindow window;

        public void Init()
        {

        }

        /// <summary>
        /// メッセージを表示します
        /// </summary>
        /// <param name="message">表示メッセージ</param>
        public void ShowMessage(string message)
        {
            this.window.btnMessageArea.Content = message;
        }
    }
}
