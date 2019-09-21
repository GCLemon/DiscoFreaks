using asd;

namespace DiscoFreaks
{
    public class GroundLayer : Layer2D
    {
        // レイヤーが登録されているシーン
        private new GameScene Scene
        {
            get => (GameScene)base.Scene;
        }

        // コンボ数の表示
        private readonly ScoreDozer ComboValue;
        private readonly ScoreDozer ComboLavel;

        public GroundLayer()
        {
            var center = new Vector2DF(0.5f, 0.5f);
            ComboValue = new ScoreDozer(240, 0, center)
            {
                Color = new Color(255, 255, 255, 63),
                Position = new Vector2DF(480, 300)
            };
            ComboLavel = new ScoreDozer(60, 0, center)
            {
                Text = "COMBO",
                Color = new Color(255, 255, 255, 63),
                Position = new Vector2DF(480, 420)
            };
        }

        protected override void OnAdded()
        {
            // コンポーネントを追加
            ComboValue.AddComponent(new ShrinkComponent(1.1f), "Shrink");

            // 判定ライン
            AddObject(new GeometryObject2D
            {
                Shape = new LineShape
                {
                    StartingPosition = new Vector2DF(0, 0),
                    EndingPosition = new Vector2DF(960, 0),
                    Thickness = 4
                },
                Position = new Vector2DF(0, 600)
            });

            // コンボ数の表示
            ComboValue.Text = 0.ToString();
            AddObject(ComboValue);
            AddObject(ComboLavel);
        }

        protected override void OnUpdated()
        {
            if (Input.KeyPush(Keys.Enter))
                ((ITextComponent)ComboValue.GetComponent("Shrink")).Trigger();
        }
    }
}
