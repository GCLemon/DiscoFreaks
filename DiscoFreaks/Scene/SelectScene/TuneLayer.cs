using System.Collections.Generic;
using asd;

namespace DiscoFreaks
{
    public class MusicLayer : Layer2D
    {
        // レイヤーが登録されているシーン
        private new SelectScene Scene
        {
            get => (SelectScene)base.Scene;
        }

        // コンポーネント
        public readonly UIComponent UIComponent;

        // ジャケット画像
        private readonly TextureObject2D Jacket;

        // 曲名・サブタイトル
        private readonly Makinas MusicTitle;
        private readonly Makinas Subtitle;

        // 難易度・レベル
        private readonly HeadUpDaisy Casual;
        private readonly HeadUpDaisy Stylish;
        private readonly HeadUpDaisy Freeky;
        private readonly HeadUpDaisy Psychic;

        // 選択中ではないが表示する曲名
        private readonly List<Makinas> AppearingScores;

        // 読み込んだ譜面
        private List<Score> Scores = Score.CreateList();

        // 現在選択している譜面のID
        private int ScoreID;

        // 選択された譜面
        public Score SelectedScore
        {
            get => Scores[ScoreID];
        }

        // コンストラクタ
        public MusicLayer()
        {
            // コンポーネント
            UIComponent = new UIComponent();

            // オブジェクト
            Jacket = new TextureObject2D
            {
                Scale = new Vector2DF(0.25f, 0.25f),
                CenterPosition = new Vector2DF(0, 256),
                Position = new Vector2DF(30, 370)
            };

            MusicTitle = new Makinas(36, 4) { Position = new Vector2DF(150, -70) };
            Subtitle = new Makinas(18, 4) { Position = new Vector2DF(150, -30) };

            Casual = new HeadUpDaisy(24, new Color(166, 226, 46), 4, new Color()) { Position = new Vector2DF(150, 0) };
            Stylish = new HeadUpDaisy(24, new Color(253, 151, 31), 4, new Color()) { Position = new Vector2DF(150, 30) };
            Freeky = new HeadUpDaisy(24, new Color(249, 38, 114), 4, new Color()) { Position = new Vector2DF(150, 60) };
            Psychic = new HeadUpDaisy(24, new Color(174, 129, 255), 4, new Color()) { Position = new Vector2DF(150, 90) };

            AppearingScores = new List<Makinas>
            {
                new Makinas(32, 4) { Position = new Vector2DF(30, 100), Color = new Color(255, 255, 255,  15) },
                new Makinas(32, 4) { Position = new Vector2DF(30, 150), Color = new Color(255, 255, 255,  31) },
                new Makinas(32, 4) { Position = new Vector2DF(30, 200), Color = new Color(255, 255, 255,  63) },
                new Makinas(32, 4) { Position = new Vector2DF(30, 250), Color = new Color(255, 255, 255, 127) },
                new Makinas(32, 4) { Position = new Vector2DF(30, 500), Color = new Color(255, 255, 255, 127) },
                new Makinas(32, 4) { Position = new Vector2DF(30, 550), Color = new Color(255, 255, 255,  63) },
                new Makinas(32, 4) { Position = new Vector2DF(30, 600), Color = new Color(255, 255, 255,  31) },
                new Makinas(32, 4) { Position = new Vector2DF(30, 650), Color = new Color(255, 255, 255,  15) }
            };
        }

        protected override void OnAdded()
        {
            // コンポーネントを追加
            AddComponent(UIComponent, "UI");
            MusicTitle.AddComponent(new FadeInComponent(), "FadeIn");
            Subtitle.AddComponent(new FadeInComponent(), "FadeIn");

            // レイヤーに諸々のオブジェクトを追加
            var m = ChildManagementMode.RegistrationToLayer;
            var t = ChildTransformingMode.Position;
            var d = ChildDrawingMode.Color;
            Jacket.AddDrawnChild(MusicTitle, m, t, d);
            Jacket.AddDrawnChild(Subtitle, m, t, d);
            Jacket.AddDrawnChild(Casual, m, t, d);
            Jacket.AddDrawnChild(Stylish, m, t, d);
            Jacket.AddDrawnChild(Freeky, m, t, d);
            Jacket.AddDrawnChild(Psychic, m, t, d);
            AddObject(Jacket);
            foreach (var text_obj in AppearingScores)
                AddObject(text_obj);
        }

        protected override void OnUpdated()
        {

            // View
            //--------------------------------------------------
            // 選択中の譜面の情報を変更する
            Jacket.Texture = Graphics.CreateTexture(SelectedScore.JacketPath);
            MusicTitle.Text = SelectedScore.Title;
            Subtitle.Text = SelectedScore.Subtitle;
            Casual.Text = SelectedScore[Difficulty.Casual] != null ?
                "CASUAL  [ Lv." + string.Format("{0,2}", SelectedScore[Difficulty.Casual].Level) + " ]" : "";
            Stylish.Text = SelectedScore[Difficulty.Stylish] != null ?
                "STYLISH [ Lv." + string.Format("{0,2}", SelectedScore[Difficulty.Stylish].Level) + " ]" : "";
            Freeky.Text = SelectedScore[Difficulty.Freeky] != null ?
                "FREEKY  [ Lv." + string.Format("{0,2}", SelectedScore[Difficulty.Freeky].Level) + " ]" : "";
            Psychic.Text = SelectedScore[Difficulty.Psychic] != null ?
                "PSYCHIC [ Lv." + string.Format("{0,2}", SelectedScore[Difficulty.Psychic].Level) + " ]" : "";

            // 画面に表示する曲名の変更
            for (int i = 0; i < AppearingScores.Count; ++i)
            {
                int id = ScoreID + i - (i >= 4 ? 3 : 4);
                id = Math.Mod(id, Scores.Count);
                AppearingScores[i].Text = Scores[id].Title;
            }
            //--------------------------------------------------



            // Controll
            //--------------------------------------------------
            if (Scene.CurrentMode == SelectScene.Mode.Music)
            {
                if (Input.KeyPush(Keys.Up))
                {
                    ScoreID = Math.Mod(ScoreID - 1, Scores.Count);
                    Change();
                }

                if (Input.KeyPush(Keys.Down))
                {
                    ScoreID = Math.Mod(ScoreID + 1, Scores.Count);
                    Change();
                }

                void Change()
                {
                    Scene.PlayBGM();
                    ((ITextComponent)MusicTitle.GetComponent("FadeIn")).Trigger();
                    ((ITextComponent)Subtitle.GetComponent("FadeIn")).Trigger();
                }

                // 選択中の難易度の変更
                foreach (var difficulty in Enum.GetValues<Difficulty>())
                {
                    var diff = (Difficulty)difficulty;
                    if (Scores[ScoreID][diff] != null)
                    {
                        Scene.Difficulty = diff;
                        break;
                    }
                }
            }
            //--------------------------------------------------
        }
    }
}
