using asd;

namespace DiscoFreaks
{
    public class DifficultyLayer : Layer2D
    {
        // レイヤーが登録されているシーン
        private new SelectScene Scene => (SelectScene)base.Scene;

        // コンポーネント
        public readonly UIComponent UIComponent;

        private readonly Makinas SelectDifficultyAnnounce;
        private readonly Makinas DifficultyDescription;

        private readonly ScoreDozer Casual_value;
        private readonly ScoreDozer Stylish_value;
        private readonly ScoreDozer Freeky_value;
        private readonly ScoreDozer Psychic_value;

        private readonly ScoreDozer Casual_label;
        private readonly ScoreDozer Stylish_label;
        private readonly ScoreDozer Freeky_label;
        private readonly ScoreDozer Psychic_label;

        public Difficulty SelectedDifficulty { get; private set; }

        public DifficultyLayer()
        {
            UIComponent = new UIComponent();

            SelectDifficultyAnnounce = new Makinas(48, 4, new Vector2DF(0.5f, 0.5f))
            {
                Text = "難易度を選択してください。",
                Position = new Vector2DF(1440, 200)
            };

            DifficultyDescription = new Makinas(36, 4, new Vector2DF(0.5f, 0.5f)) { Position = new Vector2DF(1440, 600) };

            Vector2DF center = new Vector2DF(0.5f, 1.0f);
            Casual_value = new ScoreDozer(120, new Color(166, 226, 46), 4, new Color(), center) { Position = new Vector2DF(1080, 400) };
            Stylish_value = new ScoreDozer(120, new Color(253, 151, 31), 4, new Color(), center) { Position = new Vector2DF(1320, 400) };
            Freeky_value = new ScoreDozer(120, new Color(249, 38, 114), 4, new Color(), center) { Position = new Vector2DF(1560, 400) };
            Psychic_value = new ScoreDozer(120, new Color(174, 129, 255), 4, new Color(), center) { Position = new Vector2DF(1800, 400) };

            center = new Vector2DF(0.5f, 0.0f);
            Casual_label = new ScoreDozer(36, new Color(166, 226, 46), 4, new Color(), center) { Text = "CASUAL" };
            Stylish_label = new ScoreDozer(36, new Color(253, 151, 31), 4, new Color(), center) { Text = "STYLISH" };
            Freeky_label = new ScoreDozer(36, new Color(249, 38, 114), 4, new Color(), center){ Text = "FREEKY" };
            Psychic_label = new ScoreDozer(36, new Color(174, 129, 255), 4, new Color(), center) { Text = "PSYCHIC" };
        }

        protected override void OnAdded()
        {
            // コンポーネントを追加
            AddComponent(UIComponent, "UI");

            // レイヤーに諸々のオブジェクトを追加
            var m = ChildManagementMode.RegistrationToLayer;
            var t = ChildTransformingMode.Position;
            var d = ChildDrawingMode.Color;
            AddObject(SelectDifficultyAnnounce);
            AddObject(DifficultyDescription);
            Casual_value.AddDrawnChild(Casual_label, m, t, d);
            Stylish_value.AddDrawnChild(Stylish_label, m, t, d);
            Freeky_value.AddDrawnChild(Freeky_label, m, t, d);
            Psychic_value.AddDrawnChild(Psychic_label, m, t, d);
            AddObject(Casual_value);
            AddObject(Stylish_value);
            AddObject(Freeky_value);
            AddObject(Psychic_value);
        }

        protected override void OnUpdated()
        {
            // View
            //--------------------------------------------------

            // 難易度の表示を変更する
            Casual_value.Text =
                Scene.Score[Difficulty.Casual] != null ? string.Format("{0,2}", Scene.Score[Difficulty.Casual].Level) : "";
            Stylish_value.Text =
                Scene.Score[Difficulty.Stylish] != null ? string.Format("{0,2}", Scene.Score[Difficulty.Stylish].Level) : "";
            Freeky_value.Text =
                Scene.Score[Difficulty.Freeky] != null ? string.Format("{0,2}", Scene.Score[Difficulty.Freeky].Level) : "";
            Psychic_value.Text =
                Scene.Score[Difficulty.Psychic] != null ? string.Format("{0,2}", Scene.Score[Difficulty.Psychic].Level) : "";
            Casual_label.Text =
                Scene.Score[Difficulty.Casual] != null ? "CASUAL" : "";
            Stylish_label.Text =
                Scene.Score[Difficulty.Stylish] != null ? "STYLISH" : "";
            Freeky_label.Text =
                Scene.Score[Difficulty.Freeky] != null ? "FREEKY" : "";
            Psychic_label.Text =
                Scene.Score[Difficulty.Psychic] != null ? "PSYCHIC" : "";

            Casual_value.Color = new Color(255, 255, 255, 63);
            Stylish_value.Color = new Color(255, 255, 255, 63);
            Freeky_value.Color = new Color(255, 255, 255, 63);
            Psychic_value.Color = new Color(255, 255, 255, 63);
            switch (SelectedDifficulty)
            {
                case Difficulty.Casual:
                    Casual_value.Color = new Color(255, 255, 255);
                    DifficultyDescription.Text =
                        "音ゲー初心者向けの難易度です。\n珈琲を片手にごゆっくりどうぞ。";
                    break;
                case Difficulty.Stylish:
                    Stylish_value.Color = new Color(255, 255, 255);
                    DifficultyDescription.Text =
                        "音ゲーにある程度慣れた人向けの難易度です。\n一人で盛り上がりたいときにおすすめ。";
                    break;
                case Difficulty.Freeky:
                    Freeky_value.Color = new Color(255, 255, 255);
                    DifficultyDescription.Text =
                        "音ゲーを極めた人向けの難易度です。\nお子様の手の届かないところで遊びましょう。";
                    break;
                case Difficulty.Psychic:
                    Psychic_value.Color = new Color(255, 255, 255);
                    DifficultyDescription.Text =
                        "廃人向けの難易度です。\n遊ぶな危険。";
                    break;
            }

            //--------------------------------------------------



            // Controll
            //--------------------------------------------------


            if (Scene.CurrentMode == SelectScene.Mode.Difficulty)
            {
                // 難易度の変更
                if (Input.KeyPush(Keys.Right))
                {
                    int d = (int)SelectedDifficulty;
                    for (int i = d + 1; 0 <= i && i < 4; ++i)
                    {
                        if (Scene.Score[(Difficulty)i] != null)
                        {
                            SelectedDifficulty = (Difficulty)i;
                            break;
                        }
                    }
                }

                if (Input.KeyPush(Keys.Left))
                {
                    int d = (int)SelectedDifficulty;
                    for (int i = d - 1; 0 <= i && i < 4; --i)
                    {
                        if (Scene.Score[(Difficulty)i] != null)
                        {
                            SelectedDifficulty = (Difficulty)i;
                            break;
                        }
                    }
                }

                //--------------------------------------------------
            }
        }
    }
}
