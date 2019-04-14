using System;

namespace VsUpdateNotifier.Models
{
    #region モデル

    #region 紙芝居

    #region 抽象
    public abstract class PictureStoryShow
    {
        public PictureStoryPaper RootPaper {
            get
            {
                return this._rootPaper;
            }
            set
            {
                this._rootPaper = value;
                this.CurrentPaper = value;
            }
        }

        private PictureStoryPaper _rootPaper;

        public PictureStoryPaper CurrentPaper { get; private set; }

        public int Page { get; private set; }

        public PictureStoryPaper Next()
        {
            if (this.CurrentPaper.IsLastPage)
            {
                return new EndOfStoryPaper();
            }

            var nextPaper = this.CurrentPaper.Next;

            this.CurrentPaper = nextPaper;
            return nextPaper;
        }

        public PictureStoryPaper Prev()
        {
            if (this.CurrentPaper.IsFirstPage)
            {
                return new EndOfStoryPaper();
            }

            var prevPaper = this.CurrentPaper.Prev;
            this.CurrentPaper = prevPaper;
            return prevPaper;
        }
    }
    #endregion

    #endregion

    #region 紙芝居の紙

    #region 抽象

    public abstract class PictureStoryPaper
    {
        public bool IsFirstPage
        {
            get { return this.Prev == null; }
        }
        public bool IsLastPage
        {
            get { return this.Next == null; }
        }

        public string Text { get; set; }

        public PictureStoryPaper Next { get; set; }

        public PictureStoryPaper Prev { get; set; }
    }

    #endregion

    #region テキストだけの紙

    public class TextPaper : PictureStoryPaper
    {
        public TextPaper(string text)
        {
            this.Text = text;
        }
    }

    #endregion

    #region 空の紙

    public class EmptyPaper : PictureStoryPaper
    {
        public EmptyPaper()
        {
            this.Text = string.Empty;
        }
    }

    #endregion

    #region こらしめの紙
    public class KorashimePaper : EmptyPaper
    {

    }
    #endregion

    #region 終わりの紙
    public class EndOfStoryPaper : EmptyPaper
    {
    }
    #endregion

    #region 質問の紙

    public class QuestionPaper : PictureStoryPaper
    {

        public Func<QuestionPaper, PictureStoryPaper> OnYes { get; set; }

        public Func<QuestionPaper, PictureStoryPaper> OnNo { get; set; }
    }


    #endregion

    #endregion

    #endregion
    
    #region ストーリー

    #region アップデートを勧める

    public class RecommendUpdateStory : PictureStoryShow
    {
        public RecommendUpdateStory()
        {
            // RootPaperの設定
            var rootPaper = new QuestionPaper { Text = "ふるいVSをつかっているんですね。\r\nバージョンアップしましょう。"  };
            rootPaper.Prev = rootPaper;
            rootPaper.Next = rootPaper;

            // Updateしますか？(Yes)のPaper
            rootPaper.OnYes = this.ChooseYes;

            // Updateしますか？(No)のPaper2
            rootPaper.OnNo = this.ChooseNo1;
            this.RootPaper = rootPaper;
        }

        private PictureStoryPaper ChooseYes(QuestionPaper questionPaper)
        {
            var updateYesPaper = new TextPaper("やったー");
            updateYesPaper.Prev = questionPaper;
            return updateYesPaper;
        }

        private PictureStoryPaper ChooseNo1(QuestionPaper questionPaper)
        {
            // UpdateQuestionPaperの設定
            var updateNoPaper = new QuestionPaper { Text = "フフ。すなおじゃないですね。\r\nじつはあたらしいVSにきょうみあるんでしょう。"  };
            updateNoPaper.Prev = questionPaper;
            updateNoPaper.OnYes = this.ChooseYes;
            updateNoPaper.OnNo = this.ChooseNo2;
            return updateNoPaper;
        }

        private PictureStoryPaper ChooseNo2(QuestionPaper questionPaper)
        {
            // UpdateQuestionPaperの設定
            var updateNoPaper = new QuestionPaper { Text = "ごじょうだんを！\r\nあたらしいVSがいいにきまっているでしょう！"  };
            updateNoPaper.Prev = questionPaper;
            updateNoPaper.OnYes = this.ChooseYes;
            updateNoPaper.OnNo = this.ChooseNo3;
            return updateNoPaper;
        }

        private PictureStoryPaper ChooseNo3(QuestionPaper questionPaper)
        {
            var updateYesPaper = new TextPaper("なっとくしてもらうには どうやらちからわざがひつようですね。");
            updateYesPaper.Prev = questionPaper;
            updateYesPaper.Next = this.ChooseNo4(questionPaper);
            return updateYesPaper;
        }

        private PictureStoryPaper ChooseNo4(QuestionPaper questionPaper)
        {
            var updateYesPaper = new TextPaper("こうかいしても しりませんよ！");
            updateYesPaper.Prev = questionPaper;

            var k1 = new KorashimePaper();
            updateYesPaper.Next = k1; 
            k1.Prev = updateYesPaper;
            return updateYesPaper;
        }
    }
    #endregion

    #endregion
}
