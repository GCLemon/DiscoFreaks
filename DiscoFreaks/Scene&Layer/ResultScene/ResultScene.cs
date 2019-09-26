using asd;

namespace DiscoFreaks
{
    public class ResultScene : Scene
    {
        // レイヤー
        private readonly ResultLayer ResultLayer;

        public ResultScene(Score score, Difficulty difficulty, Result result)
        {
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
