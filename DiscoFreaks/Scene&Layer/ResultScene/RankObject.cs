using asd;

namespace DiscoFreaks
{
    public class RankObject : ScoreDozer
    {
        ScoreDozer Label;
        ScoreDozer Judge;

        ScoreDozer ImpactLabel;
        ScoreDozer ImpactValue;
        ScoreDozer ImpactJudge;

        public RankObject(Rank rank) : base(new Vector2DF(0.5f, 0.5f))
        {
            Color color = new Color();
            Color o_color = new Color();

            switch (rank)
            {
                case Rank.F:
                    o_color = new Color(255, 0, 0);
                    break;
                case Rank.E:
                    color = new Color(127, 0, 255);
                    break;
                case Rank.D:
                    color = new Color(0, 0, 255);
                    break;
                case Rank.C:
                    color = new Color(0, 255, 255);
                    break;
                case Rank.B:
                    color = new Color(0, 255, 0);
                    break;
                case Rank.A:
                    color = new Color(192, 255, 0);
                    break;
                case Rank.S:
                    color = new Color(255, 255, 0);
                    break;
                case Rank.SS:
                    color = new Color(255, 128, 0);
                    break;
                case Rank.SSS:
                    color = new Color(255, 0, 0);
                    break;
                case Rank.EXC:
                    color = new Color(255, 255, 255);
                    break;
            }

            ResetFont(150, color, 4, o_color);
            Text = rank.ToString();
            Position = new Vector2DF(765, 540);
            IsDrawn = false;

            var center = new Vector2DF(0.5f, 0.5f);
            Label = new ScoreDozer(48, color, 4, o_color, center)
            {
                Text = "RANK",
                Position = new Vector2DF(0, -90),
                IsDrawn = false
            };

            Judge = new ScoreDozer(36, color, 4, o_color, center)
            {
                Text = ((int)rank) >= 3 ? "SUCCESS" : "FAILURE",
                Position = new Vector2DF(0, 75),
                IsDrawn = false
            };

            ImpactValue = new ScoreDozer(150, color, 4, o_color, center)
            {
                Text = rank.ToString(),
                Position = new Vector2DF(765, 540),
                IsDrawn = false
            };

            ImpactLabel = new ScoreDozer(48, color, 4, o_color, center)
            {
                Text = "RANK",
                Position = new Vector2DF(0, -90),
                IsDrawn = false
            };

            ImpactJudge = new ScoreDozer(36, color, 4, o_color, center)
            {
                Text = ((int)rank) >= 3 ? "SUCCESS" : "FAILURE",
                Position = new Vector2DF(0, 75),
                IsDrawn = false
            };
        }

        protected override void OnAdded()
        {
            // 子オブジェクトの描画方法を設定
            var m = ChildManagementMode.RegistrationToLayer;
            var t = ChildTransformingMode.All;
            var d = ChildDrawingMode.Color;

            // こオブジェクトの追加
            AddDrawnChild(Label, m, t, d);
            AddDrawnChild(Judge, m, t, d);

            // インパクトの設定
            ImpactValue.AddDrawnChild(ImpactLabel, m, t, d);
            ImpactValue.AddDrawnChild(ImpactJudge, m, t, d);
            ImpactValue.AddComponent(new SwellComponent(1.2f), "Swell");
            ImpactValue.AddComponent(new ColorComponent(20), "Color");
            Layer.AddObject(ImpactValue);
        }

        public void Impact()
        {
            IsDrawn = true;
            Label.IsDrawn = true;
            Judge.IsDrawn = true;
            ImpactValue.IsDrawn = true;
            ImpactLabel.IsDrawn = true;
            ImpactJudge.IsDrawn = true;
            ((ITextComponent)ImpactValue.GetComponent("Swell")).Trigger();
            ((ITextComponent)ImpactValue.GetComponent("Color")).Trigger();
        }

        public void Interrupt()
        {
            IsDrawn = true;
            Label.IsDrawn = true;
            Judge.IsDrawn = true;
            ImpactValue.IsDrawn = false;
            ImpactLabel.IsDrawn = false;
            ImpactJudge.IsDrawn = false;
        }
    }
}
