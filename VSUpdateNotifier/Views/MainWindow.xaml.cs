using System;
using System.Windows;
using System.Windows.Input;
using VsUpdateNotifier.Behaviors;
using VsUpdateNotifier.Models;
using VsUpdateNotifier.ViewModels;

namespace VsUpdateNotifier.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Init();
        }

        private KeyPressCommandBinder keyPressCommandBinder;

        #region  紙芝居

        private ConfirmMessageArea confirmMessageArea;

        private MessageArea messageArea;

        private PictureStoryShow pictureStoryShow;

        public void Next()
        {
            var paper = this.pictureStoryShow.Next();

            if (paper.IsLastPage)

            this.confirmMessageArea.Init();

            // 次のページが存在しない場合は何もしない
            if (paper is EndOfStoryPaper)
            {
                return;
            }

            this.Render(paper);
        }

        public void Prev()
        {
            var paper = this.pictureStoryShow.Prev();

            // 前のページが存在しない場合は何もしない
            if (paper is EndOfStoryPaper)
            {
                return;
            }

            this.Render(paper);

        }

        public void TransitionShooting()
        {
        }

        protected void Init()
        {
            // Yes/No確認メッセージエリアを初期化する
            this.confirmMessageArea = new ConfirmMessageArea(this);
            this.confirmMessageArea.Init();

            // メッセージエリアを初期化する
            this.messageArea = new MessageArea(this);
            this.messageArea.Init();

            // キー押下コマンドバインダーを初期化する
            this.keyPressCommandBinder = new KeyPressCommandBinder();
            this.keyPressCommandBinder.AddCommand(Key.Return, new EnterKeyDownCommand());
            this.keyPressCommandBinder.AddCommand(Key.Up, new UpDownKeyDownCommand());
            this.keyPressCommandBinder.AddCommand(Key.Down, new UpDownKeyDownCommand());
            this.keyPressCommandBinder.AddCommand(Key.Left, new LeftKeyDownCommand());

            // 紙芝居を初期化する
            this.pictureStoryShow = new RecommendUpdateStory();
            this.Render(this.pictureStoryShow.RootPaper);
        }

        protected void Render(PictureStoryPaper paper)
        {
            this.messageArea.ShowMessage(paper.Text);

            if (paper is QuestionPaper)
            {
                this.confirmMessageArea.Show();
            }
            else if (paper is KorashimePaper)
            {
                this.Hide();

                var desktopCharacter = new DesktopCharacter();
                var desktop = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                desktopCharacter.Top = desktop.Height - (desktopCharacter.Height + 100);
                desktopCharacter.Left = desktop.Width - (desktopCharacter.Width + 100);
                desktopCharacter.Topmost = true;
                desktopCharacter.Show();
            }
            else
            {
                this.confirmMessageArea.Hide();
            }
        }

        #region イベントのディスパッチ

        #region KeyDownイベント
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var commands = this.keyPressCommandBinder.ListCommand(e.Key);
            foreach (var cmd in commands)
            {
                if (!cmd.CanExecute(this))
                {
                    break;
                }

                cmd.Execute(this);
            }
        }
        #endregion

        #endregion

        #region KeyDownコマンド

        #region Enterキー
        private class EnterKeyDownCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                var wnd = parameter as MainWindow;

                // 質問の紙の場合は次の遷移先を決める
                if (wnd.pictureStoryShow.CurrentPaper is QuestionPaper)
                {
                    var q = wnd.pictureStoryShow.CurrentPaper as QuestionPaper;
                    var c = wnd.confirmMessageArea;
                    if (c.Choice == Choice.Yes)
                    {
                        q.Next = q.OnYes(q);
                    }
                    else
                    {
                        q.Next = q.OnNo(q);
                    }
                }

                // 次のページへ進む
                wnd.Next();
            }
        }
        #endregion

        #region Up / Downキー
        private class UpDownKeyDownCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                var wnd = parameter as MainWindow;
                if (wnd.pictureStoryShow.CurrentPaper is QuestionPaper)
                {
                    var q = wnd.pictureStoryShow.CurrentPaper as QuestionPaper;
                    var c = wnd.confirmMessageArea;
                    if (c.Choice == Choice.Yes)
                    {
                        c.Choice = Choice.No;
                    }
                    else
                    {
                        c.Choice = Choice.Yes;
                    }
                }
            }
        }

        #endregion

        #region Leftキー
        private class LeftKeyDownCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                var wnd = parameter as MainWindow;
                wnd.Prev();
            }
        }
        #endregion

        #endregion

        #endregion
    }
}