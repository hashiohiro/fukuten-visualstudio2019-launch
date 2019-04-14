using System.Windows;
using VsUpdateNotifier.Views;

namespace VsUpdateNotifier.ViewModels
{
    public class ConfirmMessageArea
    {
        public ConfirmMessageArea(MainWindow window)
        {
            this.window = window;
            this.Init();
        }

        public Choice Choice
        {
            get { return this._choice; }
            set
            {
                this._choice = value;

                var content = this._choice == Choice.Yes ? "＞ はい\r\n　 いいえ" : "　 はい\r\n＞ いいえ";
                this.window.btnConfirmMessageArea.Content = content;
            }
        }

        private Choice _choice;

        private MainWindow window;

        public void Init()
        {
            this.Choice = Choice.Yes;
        }

        public void Show()
        {
            this.window.btnConfirmMessageArea.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            this.window.btnConfirmMessageArea.Visibility = Visibility.Hidden;
        }
    }

    public enum Choice
    {
        Yes,
        No
    }
}
