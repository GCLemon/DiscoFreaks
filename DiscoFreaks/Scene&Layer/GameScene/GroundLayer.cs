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
        private readonly ScoreDozer Judgement;

        // 前フレームにおけるリザルト
        private Result Result;

        public GroundLayer()
        {
            var center = new Vector2DF(0.5f, 0.5f);
            ComboValue = new ScoreDozer(240, 0, center)
            {
                Color = new Color(255, 255, 255, 63),
                Position = new Vector2DF(480, 250)
            };
            ComboLavel = new ScoreDozer(60, 0, center)
            {
                Text = "COMBO",
                Color = new Color(255, 255, 255, 63),
                Position = new Vector2DF(480, 370)
            };
            Judgement = new ScoreDozer(center)
            {
                Color = new Color(255, 255, 255, 0),
                Position = new Vector2DF(480, 480)
            };
        }

        protected override void OnAdded()
        {
            // コンポーネントを追加
            ComboValue.AddComponent(new ShrinkComponent(1.1f), "Shrink");
            Judgement.AddComponent(new ColorComponent(50), "Color");

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
            AddObject(Judgement);

            // 一応初期状態のResultを代入
            Result = Scene.Result;
        }

        

        protected override void OnUpdated()
        {
            ComboValue.IsDrawn = ComboLavel.IsDrawn = Scene.Result.Combo > 5;

            if(Scene.Result.Just > Result.Just)
            {
                Judgement.ResetFont(96, new Color(230, 219, 116), 4, new Color(255, 255, 0));
                Judgement.Text = "JUST";
                ComboValue.Text = Scene.Result.Combo.ToString();
                ((ITextComponent)Judgement.GetComponent("Color")).Trigger();
                ((ITextComponent)ComboValue.GetComponent("Shrink")).Trigger();
            }

            if (Scene.Result.Cool > Result.Cool)
            {
                Judgement.ResetFont(96, new Color(249, 38, 114), 4, new Color(255, 0, 255));
                Judgement.Text = "COOL";
                ComboValue.Text = Scene.Result.Combo.ToString();
                ((ITextComponent)Judgement.GetComponent("Color")).Trigger();
                ((ITextComponent)ComboValue.GetComponent("Shrink")).Trigger();
            }

            if (Scene.Result.Good > Result.Good)
            {
                Judgement.ResetFont(96, new Color(166, 226, 46), 4, new Color(0, 255, 0));
                Judgement.Text = "GOOD";
                ComboValue.Text = Scene.Result.Combo.ToString();
                ((ITextComponent)Judgement.GetComponent("Color")).Trigger();
                ((ITextComponent)ComboValue.GetComponent("Shrink")).Trigger();
            }

            if (Scene.Result.Near > Result.Near)
            {
                Judgement.ResetFont(96, new Color(49, 137, 211), 4, new Color(0, 255, 255));
                Judgement.Text = "NEAR";
                ComboValue.Text = Scene.Result.Combo.ToString();
                ((ITextComponent)Judgement.GetComponent("Color")).Trigger();
                ((ITextComponent)ComboValue.GetComponent("Shrink")).Trigger();
            }

            if (Scene.Result.Miss > Result.Miss)
            {
                Judgement.ResetFont(96, new Color(0, 0, 0), 4, new Color(255, 0, 0));
                Judgement.Text = "MISS";
                ComboValue.Text = Scene.Result.Combo.ToString();
                ((ITextComponent)Judgement.GetComponent("Color")).Trigger();
                ((ITextComponent)ComboValue.GetComponent("Shrink")).Trigger();
            }

            Result = Scene.Result;
        }
    }
}
