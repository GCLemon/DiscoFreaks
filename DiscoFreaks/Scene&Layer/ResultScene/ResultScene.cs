using System.Collections.Generic;
using CoreTweet;
using asd;

namespace DiscoFreaks
{
    public class ResultScene : Scene
    {
        private bool AllowTweet;
        private Score SelectedScore;
        private Result Result;

        // レイヤー
        private readonly ResultLayer ResultLayer;
        private readonly TweetLayer TweetLayer;

        // シーンの状態遷移に用いるコルーチン
        IEnumerator<object> Coroutine;

        public ResultScene(Score score, Difficulty difficulty, Result result, bool allow_tweet)
        {
            AllowTweet = allow_tweet;
            SelectedScore = score;
            Result = result;

            // 自己ベストのロード・変更・セーブ
            var high_score = HighScore.Load(score.Title);
            if (high_score[difficulty].score < result.Score)
                high_score[difficulty] = (result.Score, result.ClearJudgement);
            HighScore.Save(high_score, score.Title);

            // コンポーネントを追加
            AddComponent(new BackgroundComponent("Shader/OpenGL/Result.glsl"), "Background");
            AddComponent(new InputManageComponent(), "Input");
            AddComponent(new FixedUIComponent("Result"), "FixedUI");

            // インスタンスを代入
            ResultLayer = new ResultLayer(score, difficulty, result) { DrawingPriority = 1 };
            TweetLayer = new TweetLayer { DrawingPriority = 1 };

            // コルーチンを取得
            Coroutine = GetCoroutine();
        }

        protected override void OnRegistered()
        {
            // レイヤーの追加
            AddLayer(ResultLayer);
            AddLayer(TweetLayer);
        }

        protected override void OnUpdated()
        {
            Coroutine?.MoveNext();
        }

        protected override void OnTransitionBegin()
        {
            var config = Configuration.Load();
            config.AutoMode = false;
            Configuration.Save(config);
        }

        private IEnumerator<object> GetCoroutine()
        {
            // リザルトが全て表示されるまで待機
            while (!ResultLayer.IsShownAll)
            {
                // キー押下でリザルトの全表示
                foreach (var k in Enum.GetValues<Keys>())
                    if (Input.KeyPush((Keys)k)) ResultLayer.ShowAll();

                yield return null;
            }

            // 右シフトを押すまで待機
            while (!AllowTweet || !Input.KeyPush(Keys.RightShift))
            {
                // エンターで強制的に次の曲へ
                if (Input.KeyPush(Keys.Enter)) goto next_scene;

                yield return null;
            }

            Engine.TakeScreenshot("Result.png");

            ((UIComponent)ResultLayer.GetComponent("UI")).MoveLeft();
            ((UIComponent)TweetLayer.GetComponent("UI")).MoveLeft();

            // ツイッター
            while (true)
            {
                TweetLayer.Yes.Color = new Color(255, 255, 255, TweetLayer.IsTweet ? 255 : 63);
                TweetLayer.No.Color = new Color(255, 255, 255, TweetLayer.IsTweet ? 63 : 255);

                // ツイートするか否かを切替
                if (Input.KeyPush(Keys.Right) || Input.KeyPush(Keys.Left))
                    TweetLayer.IsTweet = !TweetLayer.IsTweet;

                // エンター押下で処理
                if (Input.KeyPush(Keys.Enter))
                {
                    // ツイートするか?
                    if (TweetLayer.IsTweet)
                    {
                        // トークンはないか?
                        if (TweetManager.IsTokensNull)
                        {
                            // Twitterの認証
                            ((UIComponent)TweetLayer.GetComponent("UI")).MoveLeft();
                            TweetManager.Authorize();

                            while (true)
                            {
                                for(int i = 0; i < 10; ++i)
                                {
                                    Keys key = (Keys)(i + 7);
                                    if (Input.KeyPush(key))
                                    {
                                        var pin_code = TweetLayer.PINObject.PINCode.ToCharArray();
                                        pin_code[TweetLayer.PINObject.SettingDigit] = i.ToString()[0];
                                        TweetLayer.PINObject.PINCode = new string(pin_code);
                                        TweetLayer.PINObject.SettingDigit = Math.Mod(++TweetLayer.PINObject.SettingDigit, 7);
                                    }
                                }

                                if (Input.KeyPush(Keys.Up) || Input.KeyPush(Keys.Down))
                                {
                                    var pin_code = TweetLayer.PINObject.PINCode.ToCharArray();
                                    var digit = int.Parse(pin_code[TweetLayer.PINObject.SettingDigit].ToString());
                                    digit = Math.Mod(digit + (Input.KeyPush(Keys.Up) ? 1 : -1), 10);
                                    pin_code[TweetLayer.PINObject.SettingDigit] = digit.ToString()[0];
                                    TweetLayer.PINObject.PINCode = new string(pin_code);
                                }

                                if (Input.KeyPush(Keys.Left) || Input.KeyPush(Keys.Right))
                                {
                                    TweetLayer.PINObject.SettingDigit += Input.KeyPush(Keys.Right) ? 1 : -1;
                                    TweetLayer.PINObject.SettingDigit = Math.Mod(TweetLayer.PINObject.SettingDigit, 7);
                                }

                                if (Input.KeyPush(Keys.Enter))
                                {
                                    try
                                    {
                                        TweetManager.CreateToken(TweetLayer.PINObject.PINCode);
                                        TweetManager.TweetResult(SelectedScore.Title, Result.Score);
                                        ((UIComponent)TweetLayer.GetComponent("UI")).MoveRight();
                                        Input.AcceptInput = false;
                                        goto finish;
                                    }
                                    catch (TwitterException)
                                    {
                                        System.Console.WriteLine("ERROR");
                                        TweetLayer.PINObject.ShowError();
                                    }
                                }

                                if (Input.KeyPush(Keys.Backspace))
                                {
                                    ((UIComponent)TweetLayer.GetComponent("UI")).MoveRight();
                                    break;
                                }

                                yield return null;
                            }
                        }
                    }

                    else goto next_scene;
                }

                yield return null;
            }

            finish:
            System.IO.File.Delete("Result.png");
            ((ITextComponent)TweetLayer.Announce.GetComponent("FadeOut")).Trigger();
            ((ITextComponent)TweetLayer.Yes.GetComponent("FadeOut")).Trigger();
            ((ITextComponent)TweetLayer.No.GetComponent("FadeOut")).Trigger();
            for (int i = 0; i < 30; ++i) yield return null;
            ((ITextComponent)TweetLayer.Finished.GetComponent("FadeIn")).Trigger();
            for (int i = 0; i < 30; ++i) yield return null;
            Input.AcceptInput = true;
            while (true)
            {
                // エンターで次の曲へ
                if (Input.KeyPush(Keys.Enter)) break;
                yield return null;
            }

            next_scene:
            Engine.ChangeSceneWithTransition(new SelectScene(SelectedScore), new TransitionFade(1, 1));
        }
    }
}
