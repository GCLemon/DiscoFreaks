using asd;

namespace DiscoFreaks
{
    public class ResultScene : Scene
    {
        // レイヤー
        private readonly ResultLayer ResultLayer;

        public ResultScene(Score score, Difficulty difficulty, Result result)
        {
            // 自己ベストのロード・変更・セーブ
            var high_score = HighScore.Load(score.Title);
            if (high_score[difficulty].Score < result.Score)
                high_score[difficulty] = (result.Score, result.ClearJudgement);
            HighScore.Save(high_score, score.Title);

            // コンポーネントを追加
            AddComponent(new BackgroundComponent("Shader/OpenGL/Result.glsl", 100), "Background");
            AddComponent(new InputManageComponent(), "Input");
            AddComponent(new FixedUIComponent("Result"), "FixedUI");

            // インスタンスを代入
            ResultLayer = new ResultLayer(score, difficulty, result) { DrawingPriority = 1 };
        }

        protected override void OnRegistered()
        {
            // レイヤーの追加
            AddLayer(ResultLayer);
        }

        protected override void OnStartUpdating()
        {
            // ノートタイマーの停止
            Note.NoteTimer.Stop();
            Note.NoteTimer.Reset();
        }

        protected override void OnUpdated()
        {
            if(Input.KeyPush(Keys.Enter))
            {
                if (ResultLayer.IsShownAll)
                    Engine.ChangeSceneWithTransition(new SelectScene(), new TransitionFade(1, 1));

                else ResultLayer.IsShownAll = true;
            }
        }
    }
}
