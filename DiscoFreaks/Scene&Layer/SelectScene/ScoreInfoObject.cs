using System;
using System.Collections.Generic;
using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// ジャケットを含む譜面の情報
    /// </summary>
    public class ScoreInfoObject : TextureObject2D
    {
        // レイヤー
        private new TuneLayer Layer
        {
            get => (TuneLayer)base.Layer;
        }

        // 曲名・サブタイトル
        private readonly Makinas MusicTitle;
        private readonly Makinas Subtitle;

        // 難易度・レベル
        private struct LevelInfo
        {
            public GridGazer Label;
            public GridGazer Value;
            public GridGazer Score;
            public GridGazer Rank;
            public GridGazer Judge;
        }
        private Dictionary<Difficulty, LevelInfo> LevelInfos;

        public ScoreInfoObject()
        {
            // ジャケットの設定
            Scale = new Vector2DF(0.25f, 0.25f);
            CenterPosition = new Vector2DF(0, 256);
            Position = new Vector2DF(30, 370);

            // 曲名・サブタイトル
            MusicTitle = new Makinas(36, 4) { Position = new Vector2DF(150, -70) };
            Subtitle = new Makinas(18, 4) { Position = new Vector2DF(150, -30) };

            // レベル等の表示
            LevelInfo CreateSet(Color color, Vector2DF globalpos)
            {
                return new LevelInfo
                {
                    Label = new GridGazer(24, color, 2, new Color()) { Position = globalpos },
                    Value = new GridGazer(24, color, 2, new Color()) { Position = new Vector2DF(150, 0) },
                    Score = new GridGazer(24, 2) { Position = new Vector2DF(240, 0) },
                    Rank = new GridGazer(24, 2, new Vector2DF(0.5f, 0)) { Position = new Vector2DF(400, 0) },
                    Judge = new GridGazer(24, 2) { Position = new Vector2DF(450, 0) }
                };
            }
            LevelInfos = new Dictionary<Difficulty, LevelInfo>
            {
                { Difficulty.Casual, CreateSet(new Color(166, 226, 46), new Vector2DF(150, 0)) },
                { Difficulty.Stylish, CreateSet(new Color(253, 151, 31), new Vector2DF(150, 30)) },
                { Difficulty.Freeky, CreateSet(new Color(249, 38, 114), new Vector2DF(150, 60)) },
                { Difficulty.Psychic, CreateSet(new Color(174, 129, 255), new Vector2DF(150, 90)) }
            };
        }

        protected override void OnAdded()
        {
            // コンポーネントを追加
            MusicTitle.AddComponent(new SlideComponent(), "Slide");
            MusicTitle.AddComponent(new FadeInComponent(15, 255), "FadeIn");
            Subtitle.AddComponent(new SlideComponent(), "Slide");
            Subtitle.AddComponent(new FadeInComponent(15, 255), "FadeIn");

            // 子オブジェクトの描画方法を設定
            var m = ChildManagementMode.RegistrationToLayer;
            var t = ChildTransformingMode.Position;
            var d = ChildDrawingMode.Color;

            // 子オブジェクト・孫オブジェクトの追加
            AddDrawnChild(MusicTitle, m, t, d);
            AddDrawnChild(Subtitle, m, t, d);
            foreach (var diff in Enum.GetValues<Difficulty>())
            {
                var text = LevelInfos[(Difficulty)diff];
                text.Label.AddDrawnChild(text.Value, m, t, d);
                text.Label.AddDrawnChild(text.Score, m, t, d);
                text.Label.AddDrawnChild(text.Rank, m, t, d);
                text.Label.AddDrawnChild(text.Judge, m, t, d);
                AddDrawnChild(LevelInfos[(Difficulty)diff].Label, m, t, d);
            }
        }

        public void SetInfo()
        {
            // ジャケット・曲名・サブタイトル
            Texture = Graphics.CreateTexture(Layer.SelectedScore.JacketPath);
            MusicTitle.Text = Layer.SelectedScore.Title;
            Subtitle.Text = Layer.SelectedScore.Subtitle;

            // 難易度・レベル・自己ベスト・ランク
            foreach (var diff in Enum.GetValues<Difficulty>())
            {
                var difficulty = (Difficulty)diff;
                var score = Layer.SelectedScore[difficulty];
                var high_score_obj = HighScore.Load(Layer.SelectedScore.Title);
                var high_score = high_score_obj[difficulty].score;
                var judgement = high_score_obj[difficulty].judge;

                if (score != null)
                {
                    LevelInfos[difficulty].Label.Text = difficulty.ToString();
                    LevelInfos[difficulty].Value.Text = "Lv." + score.Level;
                    LevelInfos[difficulty].Score.Text = string.Format("{0:D6}", high_score);
                    LevelInfos[difficulty].Rank.Text = Result.GetRank(high_score).ToString();
                    LevelInfos[difficulty].Judge.Text = judgement.ToString();

                    switch (Result.GetRank(high_score))
                    {
                        case Rank.F:
                            LevelInfos[difficulty].Rank.ResetFont(24, new Color(), 2, new Color(255, 0, 0));
                            break;
                        case Rank.E:
                            LevelInfos[difficulty].Rank.ResetFont(24, new Color(127, 0, 255), 2, new Color());
                            break;
                        case Rank.D:
                            LevelInfos[difficulty].Rank.ResetFont(24, new Color(0, 0, 255), 2, new Color());
                            break;
                        case Rank.C:
                            LevelInfos[difficulty].Rank.ResetFont(24, new Color(0, 255, 255), 2, new Color());
                            break;
                        case Rank.B:
                            LevelInfos[difficulty].Rank.ResetFont(24, new Color(0, 255, 0), 2, new Color());
                            break;
                        case Rank.A:
                            LevelInfos[difficulty].Rank.ResetFont(24, new Color(192, 255, 0), 2, new Color());
                            break;
                        case Rank.S:
                            LevelInfos[difficulty].Rank.ResetFont(24, new Color(255, 255, 0), 2, new Color());
                            break;
                        case Rank.SS:
                            LevelInfos[difficulty].Rank.ResetFont(24, new Color(255, 128, 0), 2, new Color());
                            break;
                        case Rank.SSS:
                            LevelInfos[difficulty].Rank.ResetFont(24, new Color(255, 0, 0), 2, new Color());
                            break;
                        case Rank.EXC:
                            LevelInfos[difficulty].Rank.ResetFont(24, new Color(255, 255, 255), 2, new Color());
                            break;
                    }
                }
                else
                {
                    LevelInfos[difficulty].Label.Text = "";
                    LevelInfos[difficulty].Value.Text = "";
                    LevelInfos[difficulty].Score.Text = "";
                    LevelInfos[difficulty].Judge.Text = "";
                }
            }

            // コンポーネントの効果を発動する
            try
            {
                ((ITextComponent)MusicTitle.GetComponent("Slide")).Trigger();
                ((ITextComponent)MusicTitle.GetComponent("FadeIn")).Trigger();
                ((ITextComponent)Subtitle.GetComponent("Slide")).Trigger();
                ((ITextComponent)Subtitle.GetComponent("FadeIn")).Trigger();
            }
            catch(NullReferenceException)
            {

            }
        }
    }
}
