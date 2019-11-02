using System.Collections.Generic;
using asd;

namespace DiscoFreaks
{
    public class ResultLayer : Layer2D
    {
        // フレームカウント
        private int Frame;

        // 全て表示し終わったか
        public bool IsShownAll;
        public bool IsResultTaken;

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

        private readonly TextureObject2D RankObject;
        private readonly TextureObject2D RankImpact;

        public ResultLayer(Score score, Difficulty difficulty, Result result)
        {
            // コンポーネントを作成・追加
            AddComponent(new UIComponent(), "UI");

            switch (difficulty)
            {
                case Difficulty.Casual: DiffColor = new Color(166, 226, 46); break;
                case Difficulty.Stylish: DiffColor = new Color(253, 151, 31); break;
                case Difficulty.Freaky: DiffColor = new Color(249, 38, 114); break;
                case Difficulty.Psychic: DiffColor = new Color(174, 129, 255); break;
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
            Title = new Makinas(48, 4, center)
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

            RankObject = new TextureObject2D
            {
                Texture = Graphics.CreateTexture("Image/Rank_" + result.Rank + ".png"),
                CenterPosition = new Vector2DF(165, 105),
                Position = new Vector2DF(765, 540),
                IsDrawn = false
            };
            RankImpact = new TextureObject2D
            {
                Texture = Graphics.CreateTexture("Image/Rank_" + result.Rank + ".png"),
                CenterPosition = new Vector2DF(165, 105),
                Position = new Vector2DF(765, 540),
                IsDrawn = false
            };
            //--------------------------------------------------
        }

        protected override void OnAdded()
        {
            // オブジェクトの追加
            // 曲名・サブタイトル・ジャケット・難易度
            //--------------------------------------------------
            base.AddObject(Jacket);
            base.AddObject(Title);
            base.AddObject(Subtitle);
            base.AddObject(LevelLabel);
            base.AddObject(LevelValue);
            //--------------------------------------------------

            // 得点その他詳細
            //--------------------------------------------------
            void AddObject(ScoreDozer obj)
            {
                obj.AddComponent(new SlideComponent(), "Slide");
                obj.AddComponent(new FadeInComponent(15, 225), "FadeIn");
                base.AddObject(obj);
            }
            AddObject(Just);
            AddObject(Cool);
            AddObject(Good);
            AddObject(Near);
            AddObject(Miss);
            AddObject(Combo);
            AddObject(ScoreLabel);
            AddObject(ScoreValue);

            base.AddObject(RankObject);
            base.AddObject(RankImpact);
            //--------------------------------------------------
        }

        protected override void OnUpdated()
        {
            if (!IsShownAll)
            {
                void Trigger(TextObject2D text)
                {
                    ((ITextComponent)text.GetComponent("Slide")).Trigger();
                    ((ITextComponent)text.GetComponent("FadeIn")).Trigger();
                    text.IsDrawn = true;
                }

                if (Frame == 100)
                {
                    Trigger(ScoreLabel); ScoreLabel.IsDrawn = true;
                    Trigger(ScoreValue); ScoreValue.IsDrawn = true;
                }

                if (Frame == 140) { Trigger(Just); Just.IsDrawn = true; }
                if (Frame == 150) { Trigger(Cool); Cool.IsDrawn = true; }
                if (Frame == 160) { Trigger(Good); Good.IsDrawn = true; }
                if (Frame == 170) { Trigger(Near); Near.IsDrawn = true; }
                if (Frame == 180) { Trigger(Miss); Miss.IsDrawn = true; }
                if (Frame == 190) { Trigger(Combo); Combo.IsDrawn = true; }
                if (Frame == 230)
                {
                    RankObject.IsDrawn = true;
                    RankImpact.IsDrawn = true;
                }
                if (230 <= Frame && Frame < 250)
                {
                    var v = (Frame - 230) / 20.0f;
                    var s = 1.2 - (1.0 - v) * (1.0 - v) * (1.0 - v) * 0.2f;

                    RankImpact.Color = new Color(255, 255, 255, (int)(255 * (1.0 - v)));
                    RankImpact.Scale = new Vector2DF((float)s, (float)s);
                }

                if (Frame == 250) ShowAll();

                ++Frame;
            }
        }

        public void ShowAll()
        {
            // 全ての描画
            IsShownAll = true;
            foreach (var obj in Objects) obj.IsDrawn = true;

            void Reset(TextObject2D text, Vector2DF pos)
            {
                text.RemoveComponent("Slide");
                text.RemoveComponent("FadeIn");
                text.Color = new Color(255, 255, 255);
                text.Position = pos;
            }

            // 位置のリセット
            //--------------------------------------------------
            Reset(ScoreLabel, new Vector2DF(350, 260));
            Reset(ScoreValue, new Vector2DF(350, 300));
            Reset(Just, new Vector2DF(350, 420));
            Reset(Cool, new Vector2DF(350, 460));
            Reset(Good, new Vector2DF(350, 500));
            Reset(Near, new Vector2DF(350, 540));
            Reset(Miss, new Vector2DF(350, 580));
            Reset(Combo, new Vector2DF(350, 630));
            //--------------------------------------------------

            RankImpact.Dispose();
        }
    }
}
