using System.Collections.Generic;
using asd;

namespace DiscoFreaks
{
    public class DifficultyLayer : Layer2D
    {
        // レイヤーが登録されているシーン
        private new SelectScene Scene => (SelectScene)base.Scene;

        // コンポーネント
        public UIComponent UIComponent
        {
            get => (UIComponent)GetComponent("UI");
        }

        // テキストオブジェクト
        private readonly Makinas SelectDifficultyAnnounce;
        private readonly Makinas DifficultyDescription;
        private readonly Dictionary<Difficulty, (ScoreDozer label, ScoreDozer value)> Level;

        public DifficultyLayer()
        {
            // コンポーネントを作成・追加
            AddComponent(new UIComponent(), "UI");

            SelectDifficultyAnnounce = new Makinas(48, 4, new Vector2DF(0.5f, 0.5f))
            {
                Text = "難易度を選択してください。",
                Position = new Vector2DF(1440, 200)
            };

            DifficultyDescription = new Makinas(36, 4, new Vector2DF(0.5f, 0.5f)) { Position = new Vector2DF(1440, 600) };

            Level = new Dictionary<Difficulty, (ScoreDozer label, ScoreDozer value)>();
            Color[] color =
            {
                new Color(166, 226, 46),
                new Color(253, 151, 31),
                new Color(249, 38, 114),
                new Color(174, 129, 255)
            };
            for (int i = 0; i < 4; ++i)
            {
                var v = new ScoreDozer(120, color[i], 4, new Color(), new Vector2DF(0.5f, 1.0f))
                {
                    Position = new Vector2DF(1080 + 240 * i, 400)
                };
                var l = new ScoreDozer(36, color[i], 4, new Color(), new Vector2DF(0.5f, 0.0f));
                Level.Add((Difficulty)i, (l, v));
            }
        }

        protected override void OnAdded()
        {
            // レイヤーに諸々のオブジェクトを追加
            var m = ChildManagementMode.RegistrationToLayer;
            var t = ChildTransformingMode.All;
            var d = ChildDrawingMode.Color;
            AddObject(SelectDifficultyAnnounce);
            AddObject(DifficultyDescription);

            foreach (var difficulty in Enum.GetValues<Difficulty>())
            {
                var diff = (Difficulty)difficulty;
                var value = Level[diff].value;
                var label = Level[diff].label;
                value.AddDrawnChild(label, m, t, d);
                AddObject(value);
            }

            ChangeInfoOfDifficultyShowingObjects();
        }

        protected override void OnUpdated()
        {
            if (Scene.CurrentMode == SelectScene.Mode.Difficulty)
            {
                void SetInfo(int move)
                {
                    int d = (int)Scene.Difficulty;

                    for (int i = d + move; 0 <= i && i < 4; i += move)
                        if (Scene.Score[(Difficulty)i] != null)
                        {
                            Scene.Difficulty = (Difficulty)i;
                            break;
                        }
                }

                ChangeInfoOfDifficultyShowingObjects();

                // 難易度の変更
                if (Input.KeyPush(Keys.Right)) SetInfo(1);
                if (Input.KeyPush(Keys.Left)) SetInfo(-1);
            }
        }

        private void ChangeInfoOfDifficultyShowingObjects()
        {

            foreach (var difficulty in Enum.GetValues<Difficulty>())
            {
                var diff = (Difficulty)difficulty;
                var value = Level[diff].value;
                var label = Level[diff].label;

                // テキストの変更
                value.Text =
                    Scene.Score[diff] != null ?
                    string.Format("{0,2}", Scene.Score[diff].Level) :
                    "";
                label.Text = Scene.Score[diff] != null ? diff.ToString() : "";

                // 色の変更
                value.Color =
                    Scene.Difficulty == diff ?
                    new Color(255, 255, 255) :
                    new Color(255, 255, 255, 63);
            }

            switch (Scene.Difficulty)
            {
                case Difficulty.Casual:
                    DifficultyDescription.Text =
                        "音ゲー初心者向けの難易度です。\n珈琲を片手にごゆっくりどうぞ。";
                    break;
                case Difficulty.Stylish:
                    DifficultyDescription.Text =
                        "音ゲーにある程度慣れた人向けの難易度です。\n一人で盛り上がりたいときにおすすめ。";
                    break;
                case Difficulty.Freeky:
                    DifficultyDescription.Text =
                        "音ゲーを極めた人向けの難易度です。\nお子様の手の届かないところで遊びましょう。";
                    break;
                case Difficulty.Psychic:
                    DifficultyDescription.Text =
                        "廃人向けの難易度です。\n遊ぶな危険。";
                    break;
            }
        }
    }
}
