using asd;

namespace DiscoFreaks
{
    public class ResultLayer : Layer2D
    {
        // レイヤーが登録されているシーン
        private new ResultScene Scene
        {
            get => (ResultScene)base.Scene;
        }

        // フレームカウント
        private int Frame;

        // 全て表示し終わったか
        public bool IsShownAll;

        // 難易度表示の色
        private readonly Color DiffColor;

        // ジャケット画像
        private readonly TextureObject2D Jacket;

        // 表示するテキスト
        private readonly Makinas Title;
        private readonly Makinas Subtitle;
        private readonly GridGazer LevelValue;
        private readonly GridGazer LevelLabel;
        private readonly ScoreDozer ScoreValue;
        private readonly ScoreDozer ScoreLabel;
        private readonly ScoreDozer Just;
        private readonly ScoreDozer Cool;
        private readonly ScoreDozer Good;
        private readonly ScoreDozer Near;
        private readonly ScoreDozer Miss;
        private readonly ScoreDozer Combo;

        public ResultLayer(Score score , Difficulty difficulty, Result result)
        {
            // コンポーネントを作成・追加
            AddComponent(new UIComponent(), "UI");

            switch (difficulty)
            {
                case Difficulty.Casual:
                    DiffColor = new Color(166, 226, 46);
                    break;
                case Difficulty.Stylish:
                    DiffColor = new Color(253, 151, 31);
                    break;
                case Difficulty.Freeky:
                    DiffColor = new Color(249, 38, 114);
                    break;
                case Difficulty.Psychic:
                    DiffColor = new Color(174, 129, 255);
                    break;
            }

            // 曲名・サブタイトル・ジャケット・難易度
            //--------------------------------------------------
            var center = new Vector2DF(0.5f, 0);
            Jacket = new TextureObject2D
            {
                Texture = Graphics.CreateTexture(score.JacketPath),
                Scale = new Vector2DF(0.5f, 0.5f),
                Position = new Vector2DF(50, 270)
            };
            Title = new  Makinas(48, 4, center)
            {
                Text = score.Title,
                Position = new Vector2DF(480, 130)
            };
            Subtitle = new Makinas(32, 4, center)
            {
                Text = score.Subtitle,
                Position = new Vector2DF(480, 190)
            };
            LevelValue = new GridGazer(72, DiffColor, 4, new Color(), center)
            {
                Text = "Lv." + score[difficulty].Level,
                Position = new Vector2DF(178, 550)
            };
            LevelLabel = new GridGazer(48, DiffColor, 4, new Color(), center)
            {
                Text = difficulty.ToString().ToUpper(),
                Position = new Vector2DF(178, 620)
            };
            //--------------------------------------------------

            // 得点その他詳細
            //--------------------------------------------------
            ScoreLabel = new ScoreDozer(36, 4)
            {
                Text = "SCORE",
                Position = new Vector2DF(350, 260),
                IsDrawn = false
            };
            ScoreValue = new ScoreDozer(100, 4)
            {
                Text = result.Score.ToString(),
                Position = new Vector2DF(350, 300),
                IsDrawn = false
            };
            Just = new ScoreDozer(28, new Color(230, 219, 116), 1, new Color(255, 255, 0))
            {
                Text = "JUST : " + string.Format("{0:D4}", result.Just),
                Position = new Vector2DF(350, 420),
                IsDrawn = false
            };
            Cool = new ScoreDozer(28, new Color(249, 38, 114), 1, new Color(255, 0, 255))
            {
                Text = "COOL : " + string.Format("{0:D4}", result.Cool),
                Position = new Vector2DF(350, 460),
                IsDrawn = false
            };
            Good = new ScoreDozer(28, new Color(166, 226, 46), 1, new Color(0, 255, 0))
            {
                Text = "GOOD : " + string.Format("{0:D4}", result.Good),
                Position = new Vector2DF(350, 500),
                IsDrawn = false
            };
            Near = new ScoreDozer(28, new Color(49, 137, 211), 1, new Color(0, 255, 255))
            {
                Text = "NEAR : " + string.Format("{0:D4}", result.Near),
                Position = new Vector2DF(350, 540),
                IsDrawn = false
            };
            Miss = new ScoreDozer(28, new Color(0, 0, 0), 1, new Color(255, 0, 0))
            {
                Text = "MISS : " + string.Format("{0:D4}", result.Miss),
                Position = new Vector2DF(350, 580),
                IsDrawn = false
            };
            Combo = new ScoreDozer(28, new Color(255, 255, 255), 1, new Color(0, 0, 0))
            {
                Text = "COMBO : " + string.Format("{0:D4}", result.BestCombo),
                Position = new Vector2DF(350, 630),
                IsDrawn = false
            };
            //--------------------------------------------------
        }

        protected override void OnAdded()
        {
            // コンポーネントの追加
            //--------------------------------------------------
            ScoreValue.AddComponent(new FadeInComponent(), "FadeIn");
            ScoreLabel.AddComponent(new FadeInComponent(), "FadeIn");
            Just.AddComponent(new FadeInComponent(), "FadeIn");
            Cool.AddComponent(new FadeInComponent(), "FadeIn");
            Good.AddComponent(new FadeInComponent(), "FadeIn");
            Just.AddComponent(new FadeInComponent(), "FadeIn");
            Near.AddComponent(new FadeInComponent(), "FadeIn");
            Miss.AddComponent(new FadeInComponent(), "FadeIn");
            Combo.AddComponent(new FadeInComponent(), "FadeIn");
            //--------------------------------------------------


            // オブジェクトの追加
            // 曲名・サブタイトル・ジャケット・難易度
            //--------------------------------------------------
            AddObject(Jacket);
            AddObject(Title);
            AddObject(Subtitle);
            AddObject(LevelLabel);
            AddObject(LevelValue);
            //--------------------------------------------------

            // 得点その他詳細
            //--------------------------------------------------
            AddObject(Just);
            AddObject(Cool);
            AddObject(Good);
            AddObject(Near);
            AddObject(Miss);
            AddObject(Combo);
            AddObject(ScoreLabel);
            AddObject(ScoreValue);
            //--------------------------------------------------
        }

        protected override void OnUpdated()
        {
            if (!IsShownAll)
            {
                ITextComponent FadeIn(TextObject2D text)
                    => (ITextComponent)text.GetComponent("FadeIn");

                if (Frame == 100)
                {
                    FadeIn(ScoreLabel).Trigger(); ScoreLabel.IsDrawn = true;
                    FadeIn(ScoreValue).Trigger(); ScoreValue.IsDrawn = true;
                }

                if (Frame == 140) { FadeIn(Just).Trigger(); Just.IsDrawn = true; }
                if (Frame == 150) { FadeIn(Cool).Trigger(); Cool.IsDrawn = true; }
                if (Frame == 160) { FadeIn(Good).Trigger(); Good.IsDrawn = true; }
                if (Frame == 170) { FadeIn(Near).Trigger(); Near.IsDrawn = true; }
                if (Frame == 180) { FadeIn(Miss).Trigger(); Miss.IsDrawn = true; }
                if (Frame == 190) { FadeIn(Combo).Trigger(); Combo.IsDrawn = true; }

                if (Frame == 210) IsShownAll = true;

                ++Frame;
            }
            else
            {
                foreach (var obj in Objects) obj.IsDrawn = true;

                // 色のリセット
                //--------------------------------------------------
                ScoreLabel.Color = new Color(255, 255, 255);
                ScoreValue.Color = new Color(255, 255, 255);
                Just.Color = new Color(255, 255, 255);
                Cool.Color = new Color(255, 255, 255);
                Good.Color = new Color(255, 255, 255);
                Near.Color = new Color(255, 255, 255);
                Miss.Color = new Color(255, 255, 255);
                Combo.Color = new Color(255, 255, 255);
                //--------------------------------------------------

                // 位置のリセット
                //--------------------------------------------------
                ScoreLabel.Position = new Vector2DF(350, 260);
                ScoreValue.Position = new Vector2DF(350, 300);
                Just.Position = new Vector2DF(350, 420);
                Cool.Position = new Vector2DF(350, 460);
                Good.Position = new Vector2DF(350, 500);
                Near.Position = new Vector2DF(350, 540);
                Miss.Position = new Vector2DF(350, 580);
                Combo.Position = new Vector2DF(350, 630);
                //--------------------------------------------------
            }
        }
    }
}
