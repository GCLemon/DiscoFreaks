using asd;
using CoreTweet;

namespace DiscoFreaks
{
    public class TweetLayer : Layer2D
    {
        // リザルトシーンのモード
        public enum Mode
        {
            Share,
            Authorize,
            Finished,
        }

        // 現在のモード
        public Mode CurrentMode;
        private bool IsTweet;

        // フレームカウント
        private int Frame;

        // 曲名・得点
        private string Title;
        private int Score;

        //
        public new ResultScene Scene
        {
            get => (ResultScene)base.Scene;
        }

        // 
        private Makinas Announce;
        private Makinas Yes;
        private Makinas No;
        private Makinas Finished;

        private PINObject PINObject;

        public TweetLayer(Score score, Result result)
        {
            // 曲名・得点の取得
            Title = score.Title;
            Score = result.Score;

            // コンポーネントを作成・追加
            AddComponent(new UIComponent(), "UI");

            var center = new Vector2DF(0.5f, 0.5f);
            Announce = new Makinas(48, 4, center)
            {
                Text = "プレイ結果をシェアしますか?",
                Position = new Vector2DF(1440, 250)
            };
            Yes = new Makinas(210, 4, center)
            {
                Text = "YES",
                Position = new Vector2DF(1200, 450)
            };
            No = new Makinas(210, 4, center)
            {
                Text = "NO",
                Position = new Vector2DF(1680, 450)
            };
            Finished = new Makinas(48, 4, center)
            {
                Text = "シェアが完了しました。\n詳細はTwitterを\nご確認ください。",
                Color = new Color(255, 255, 255, 0),
                Position = new Vector2DF(1440, 400)
            };

            PINObject = new PINObject();
        }

        protected override void OnAdded()
        {
            Announce.AddComponent(new FadeOutComponent(30, 255), "FadeOut");
            Yes.AddComponent(new FadeOutComponent(30, Yes.Color.A), "FadeOut");
            No.AddComponent(new FadeOutComponent(30, No.Color.A), "FadeOut");
            Finished.AddComponent(new FadeInComponent(30, 255), "FadeIn");

            AddObject(Announce);
            AddObject(Yes);
            AddObject(No);
            AddObject(Finished);

            AddObject(PINObject);
        }

        protected override void OnUpdated()
        {
            if (Scene.CurrentMode == ResultScene.Mode.Tweet)
            {
                switch (CurrentMode)
                {
                    case Mode.Share: OnShare(); break;
                    case Mode.Authorize: OnAuthorize(); break;
                    case Mode.Finished: OnFinished(); break;
                }
            }
        }

        private void OnShare()
        {
            if (IsTweet)
            {
                Yes.Color = new Color(255, 255, 255, 255);
                No.Color = new Color(255, 255, 255, 63);

                if (Input.KeyPush(Keys.Enter))
                {
                    if (TweetManager.IsTokensNull)
                    {
                        ((UIComponent)GetComponent("UI")).MoveLeft();
                        TweetManager.Authorize();
                        CurrentMode = Mode.Authorize;
                    }
                    else
                    {
                        Input.AcceptInput = false;
                        CurrentMode = Mode.Finished;
                    }
                }
            }
            else
            {
                Yes.Color = new Color(255, 255, 255, 63);
                No.Color = new Color(255, 255, 255, 255);

                if (Input.KeyPush(Keys.Enter))
                {
                    Engine.ChangeSceneWithTransition(new SelectScene(), new TransitionFade(1, 1));
                }
            }

            if (Input.KeyPush(Keys.Right) || Input.KeyPush(Keys.Left)) IsTweet = !IsTweet;
        }

        private void OnAuthorize()
        {
            if (Input.KeyPush(Keys.Enter))
            {
                try
                {
                    TweetManager.CreateToken(PINObject.PINCode);
                    TweetManager.TweetResult(Title, Score);
                    ((UIComponent)GetComponent("UI")).MoveRight();
                    CurrentMode = Mode.Finished;
                }
                catch(TwitterException)
                {
                    System.Console.WriteLine("ERROR");
                    PINObject.ShowError();
                }
            }

            if (Input.KeyPush(Keys.Backspace))
            {
                ((UIComponent)GetComponent("UI")).MoveRight();
                CurrentMode = Mode.Share;
            }
        }

        private void OnFinished()
        {
            System.IO.File.Delete("Result.png");

            if (Frame == 0)
            {
                ((ITextComponent)Announce.GetComponent("FadeOut")).Trigger();
                ((ITextComponent)Yes.GetComponent("FadeOut")).Trigger();
                ((ITextComponent)No.GetComponent("FadeOut")).Trigger();
            }
            if (Frame == 30)
            {
                ((ITextComponent)Finished.GetComponent("FadeIn")).Trigger();
            }

            if(Frame == 60) Input.AcceptInput = true;

            if (Input.KeyPush(Keys.Enter))
                Engine.ChangeSceneWithTransition(new SelectScene(), new TransitionFade(1, 1));

            Frame++;
        }
    }
}
