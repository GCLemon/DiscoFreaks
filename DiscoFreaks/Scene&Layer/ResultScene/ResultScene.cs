using asd;

namespace DiscoFreaks
{
    public class ResultScene : Scene
    {
        // リザルトシーンのモード
        public enum Mode
        {
            Result,
            Tweet
        }

        // 現在のモード
        public Mode CurrentMode { get; private set; }

        private Score SelectedScore;

        // レイヤー
        private readonly ResultLayer ResultLayer;
        private readonly TweetLayer TweetLayer;

        public ResultScene(Score score, Difficulty difficulty, Result result)
        {
            SelectedScore = score;

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
            TweetLayer = new TweetLayer(score, result) { DrawingPriority = 1 };
        }

        protected override void OnRegistered()
        {
            // レイヤーの追加
            AddLayer(ResultLayer);
            AddLayer(TweetLayer);
        }

        protected override void OnUpdated()
        {
            if (CurrentMode == Mode.Result)
            {
                if (!ResultLayer.IsShownAll)
                {
                    // キー押下でリザルトの全表示
                    foreach (var k in Enum.GetValues<Keys>())
                        if (Input.KeyPush((Keys)k))
                            ResultLayer.ShowAll();
                }
                else
                {
                    // エンターで次の曲へ
                    if (Input.KeyPush(Keys.Enter))
                        Engine.ChangeSceneWithTransition(new SelectScene(SelectedScore), new TransitionFade(1, 1));

                    // 左シフトでツイートへ
                    if (Input.KeyPush(Keys.RightShift))
                    {
                        Engine.TakeScreenshot("Result.png");

                        ((UIComponent)ResultLayer.GetComponent("UI")).MoveLeft();
                        ((UIComponent)TweetLayer.GetComponent("UI")).MoveLeft();
                        CurrentMode = Mode.Tweet;
                    }
                }
            }
        }
    }
}
