﻿using System.Collections.Generic;
using System.Linq;
using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// 曲選択シーン
    /// </summary>
    public class SelectScene : Scene
    {
        // 曲選択シーンのモード
        public enum Mode
        {
            Music,
            Difficulty
        }

        // 現在のモード
        private Mode CurrentMode;

        // 読み込んだ譜面
        private List<Score> Scores = Score.CreateList();

        // 現在選択している譜面のID
        private int ScoreID;

        // 現在選択している難易度
        private Difficulty Difficulty;

        // Option シーンで設定する項目
        public Configuration Configuration;

        // レイヤー
        private readonly Layer2D BackLayer = new Layer2D();
        private readonly Layer2D TextLayer = new Layer2D();
        private readonly Layer2D UILayer = new Layer2D();

        // シーンのタイトル
        private readonly HeadUpDaisy SceneTitle =
            new HeadUpDaisy(72, 4, new Vector2DF(0.5f, 0))
            {
                Text = "Music Select",
                Position = new Vector2DF(480, 10)
            };

        #region 楽曲選択

        // ジャケット画像
        private TextureObject2D Jacket =
            new TextureObject2D
            {
                Scale = new Vector2DF(0.25f, 0.25f),
                CenterPosition = new Vector2DF(0, 256),
                Position = new Vector2DF(30, 370)
            };

        // 曲名
        private readonly Makinas MusicTitle =
            new Makinas(36, 4)
            { Position = new Vector2DF(150, -70) };

        // サブタイトル
        private readonly Makinas Subtitle =
            new Makinas(18, 4)
            { Position = new Vector2DF(150, -30) };

        // 難易度・レベル
        private readonly HeadUpDaisy Casual =
            new HeadUpDaisy(24, new Color(166, 226, 46), 4, new Color())
            { Position = new Vector2DF(150, 0) };

        private readonly HeadUpDaisy Stylish =
            new HeadUpDaisy(24, new Color(253, 151, 31), 4, new Color())
            { Position = new Vector2DF(150, 30) };

        private readonly HeadUpDaisy Freeky =
            new HeadUpDaisy(24, new Color(249, 38, 114), 4, new Color())
            { Position = new Vector2DF(150, 60) };

        private readonly HeadUpDaisy Psychic =
            new HeadUpDaisy(24, new Color(174, 129, 255), 4, new Color())
            { Position = new Vector2DF(150, 90) };

        // 選択中ではないが表示する曲名
        private readonly List<Makinas> AppearingScores =
            new List<Makinas>
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

        #endregion

        #region 難易度選択

        private readonly Makinas SelectDifficultyAnnounce =
            new Makinas(48, 4, new Vector2DF(0.5f, 0.5f))
            {
                Text = "難易度を選択してください。",
                Position = new Vector2DF(1440, 200)
            };

        private readonly Makinas DifficultyDescription =
            new Makinas(36, 4, new Vector2DF(0.5f, 0.5f))
            { Position = new Vector2DF(1440, 600) };

        private readonly ScoreDozer Casual_value =
            new ScoreDozer(120, new Color(166, 226, 46), 4, new Color(), new Vector2DF(0.5f, 1.0f))
            { Position = new Vector2DF(1080, 400) };

        private readonly ScoreDozer Stylish_value =
            new ScoreDozer(120, new Color(253, 151, 31), 4, new Color(), new Vector2DF(0.5f, 1.0f))
            { Position = new Vector2DF(1320, 400) };

        private readonly ScoreDozer Freeky_value =
            new ScoreDozer(120, new Color(249, 38, 114), 4, new Color(), new Vector2DF(0.5f, 1.0f))
            { Position = new Vector2DF(1560, 400) };

        private readonly ScoreDozer Psychic_value =
            new ScoreDozer(120, new Color(174, 129, 255), 4, new Color(), new Vector2DF(0.5f, 1.0f))
            { Position = new Vector2DF(1800, 400) };

        private readonly ScoreDozer Casual_label =
            new ScoreDozer(36, new Color(166, 226, 46), 4, new Color(), new Vector2DF(0.5f, 0.0f))
            { Text = "CASUAL" };

        private readonly ScoreDozer Stylish_label =
            new ScoreDozer(36, new Color(253, 151, 31), 4, new Color(), new Vector2DF(0.5f, 0.0f))
            { Text = "STYLISH" };

        private readonly ScoreDozer Freeky_label =
            new ScoreDozer(36, new Color(249, 38, 114), 4, new Color(), new Vector2DF(0.5f, 0.0f))
            { Text = "FREEKY" };

        private readonly ScoreDozer Psychic_label =
            new ScoreDozer(36, new Color(174, 129, 255), 4, new Color(), new Vector2DF(0.5f, 0.0f))
            { Text = "PSYCHIC" };

        #endregion

        private bool IsUsed;
        private int SoundID;
        private int Phase;
        private int Phase_tune;

        protected override void OnRegistered()
        {
            if (!IsUsed)
            {
                // 背景の設定
                BackLayer.AddPostEffect(new BackGround("Shader/OpenGL/Select.glsl"));

                // シーンのタイトルを設定
                TextLayer.AddObject(SceneTitle);

                // UI レイヤーに諸々のオブジェクトを追加
                var m = ChildManagementMode.RegistrationToLayer;
                var t = ChildTransformingMode.Position;
                var d = ChildDrawingMode.Color;
                Jacket.AddDrawnChild(MusicTitle, m, t, d);
                Jacket.AddDrawnChild(Subtitle, m, t, d);
                Jacket.AddDrawnChild(Casual, m, t, d);
                Jacket.AddDrawnChild(Stylish, m, t, d);
                Jacket.AddDrawnChild(Freeky, m, t, d);
                Jacket.AddDrawnChild(Psychic, m, t, d);
                UILayer.AddObject(Jacket);
                foreach (var text_obj in AppearingScores)
                    UILayer.AddObject(text_obj);

                UILayer.AddObject(SelectDifficultyAnnounce);
                UILayer.AddObject(DifficultyDescription);
                Casual_value.AddDrawnChild(Casual_label, m, t, d);
                Stylish_value.AddDrawnChild(Stylish_label, m, t, d);
                Freeky_value.AddDrawnChild(Freeky_label, m, t, d);
                Psychic_value.AddDrawnChild(Psychic_label, m, t, d);
                UILayer.AddObject(Casual_value);
                UILayer.AddObject(Stylish_value);
                UILayer.AddObject(Freeky_value);
                UILayer.AddObject(Psychic_value);

                // レイヤーの追加
                AddLayer(BackLayer);
                AddLayer(UILayer);
                AddLayer(TextLayer);
            }
        }

        protected override void OnStartUpdating()
        {
            // BGMを再生する
            if (!IsUsed) PlayBGM();
        }

        protected override void OnUpdated()
        {
            // 次回以降は OnRegistered 関数の処理を省略する
            IsUsed = true;

            // Phase の値によってオブジェクトの描画位置を変更する
            if (Phase != 0)
            {
                Input.AcceptInput = false;
                foreach (var obj in UILayer.Objects.Where(x => x.Parent == null))
                    obj.Position += new Vector2DF(-8 * Phase, 0);
                Phase += Phase > 0 ? -1 : 1;
            }
            else Input.AcceptInput = true;

            if (Phase_tune != 0) --Phase_tune;

            Draw();
            Cntroll();
        }

        // 音を変更する
        private void PlayBGM()
        {
            Sound.Stop(SoundID);
            string score = Scores[ScoreID].SoundPath;
            var source = Sound.CreateBGM(score);
            source.IsLoopingMode = true;
            source.LoopStartingPoint = 0;
            source.LoopEndPoint = source.Length;
            SoundID = Sound.Play(source);

        }

        // 描画関係の処理を記述する
        private void Draw()
        {
            var score = Scores[ScoreID];

            // 選択中の譜面の情報を変更する
            Jacket.Texture = Graphics.CreateTexture(score.JacketPath);
            MusicTitle.Text = score.Title;
            Subtitle.Text = score.Subtitle;
            Casual.Text = score[Difficulty.Casual] != null ?
                "CASUAL  [ Lv." + string.Format("{0,2}", score[Difficulty.Casual].Level) + " ]" : "";
            Stylish.Text = score[Difficulty.Stylish] != null ?
                "STYLISH [ Lv." + string.Format("{0,2}", score[Difficulty.Stylish].Level) + " ]" : "";
            Freeky.Text = score[Difficulty.Freeky] != null ?
                "FREEKY  [ Lv." + string.Format("{0,2}", score[Difficulty.Freeky].Level) + " ]" : "";
            Psychic.Text = score[Difficulty.Psychic] != null ?
                "PSYCHIC [ Lv." + string.Format("{0,2}", score[Difficulty.Psychic].Level) + " ]" : "";

            // オブジェクトの描画情報を変更する
            MusicTitle.Position = new Vector2DF(150 + (float)Math.Pow(Phase_tune, 2) * 0.5f, -70);
            Subtitle.Position = new Vector2DF(150 + (float)Math.Pow(Phase_tune, 2) * 0.5f, -30);
            MusicTitle.Color = new Color(255, 255, 255, 255 - Phase_tune * 17);
            Subtitle.Color = new Color(255, 255, 255, 255 - Phase_tune * 17);

            // 画面に表示する曲名の変更
            for (int i = 0; i < AppearingScores.Count; ++i)
            {
                int id = ScoreID + i - (i >= 4 ? 3 : 4);
                id = Math.Mod(id, Scores.Count);
                AppearingScores[i].Text = Scores[id].Title;
            }

            // 難易度の表示を変更する
            Casual_value.Text =
                score[Difficulty.Casual] != null ? string.Format("{0,2}", score[Difficulty.Casual].Level) : "";
            Stylish_value.Text =
                score[Difficulty.Stylish] != null ? string.Format("{0,2}", score[Difficulty.Stylish].Level) : "";
            Freeky_value.Text =
                score[Difficulty.Freeky] != null ? string.Format("{0,2}", score[Difficulty.Freeky].Level) : "";
            Psychic_value.Text =
                score[Difficulty.Psychic] != null ? string.Format("{0,2}", score[Difficulty.Psychic].Level) : "";
            Casual_label.Text =
                score[Difficulty.Casual] != null ? "CASUAL" : "";
            Stylish_label.Text =
                score[Difficulty.Stylish] != null ? "STYLISH" : "";
            Freeky_label.Text =
                score[Difficulty.Freeky] != null ? "FREEKY" : "";
            Psychic_label.Text =
                score[Difficulty.Psychic] != null ? "PSYCHIC" : "";

            Casual_value.Color = new Color(255, 255, 255, 63);
            Stylish_value.Color = new Color(255, 255, 255, 63);
            Freeky_value.Color = new Color(255, 255, 255, 63);
            Psychic_value.Color = new Color(255, 255, 255, 63);
            switch (Difficulty)
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
        }

        // 入力関係の処理を記述する
        private void Cntroll()
        {
            // Optionシーンへ移動
            if(Input.KeyPush(Keys.RightShift))
                Engine.ChangeSceneWithTransition(
                    new OptionScene(this),
                    new TransitionFade(1, 1),
                    false
                );

            switch (CurrentMode)
            {
                case Mode.Music:
                    OnSelectingScore();
                    break;
                case Mode.Difficulty:
                    OnSelectingDifficulty();
                    break;
            }

            void OnSelectingScore()
            {
                // 楽曲の変更
                if (Input.KeyPush(Keys.Up))
                {
                    ScoreID = Math.Mod(ScoreID - 1, Scores.Count);
                    Phase_tune = 15;
                    PlayBGM();
                }

                if (Input.KeyPush(Keys.Down))
                {
                    ScoreID = Math.Mod(ScoreID + 1, Scores.Count);
                    Phase_tune = 15;
                    PlayBGM();
                }

                // 前のシーンに移行
                if (Input.KeyPush(Keys.Backspace))
                {
                    Sound.Stop(SoundID);
                    Engine.ChangeSceneWithTransition(
                        new TitleScene(),
                        new TransitionFade(1, 1)
                    );
                }

                // モードの変更
                if (Input.KeyPush(Keys.Enter))
                {
                    ++CurrentMode;
                    foreach (var d in Enum.GetValues<Difficulty>())
                        if (Scores[ScoreID][(Difficulty)d] != null)
                        {
                            Difficulty = (Difficulty)d;
                            break;
                        }
                    Phase = 15;
                }
            }

            void OnSelectingDifficulty()
            {
                // 難易度の変更
                if (Input.KeyPush(Keys.Right))
                {
                    int d = (int)Difficulty;
                    for (int i = d + 1; 0 <= i && i < 4; ++i)
                    {
                        if (Scores[ScoreID][(Difficulty)i] != null)
                        {
                            Difficulty = (Difficulty)i;
                            break;
                        }
                    }
                }

                if (Input.KeyPush(Keys.Left))
                {
                    int d = (int)Difficulty;
                    for (int i = d - 1; 0 <= i && i < 4; --i)
                    {
                        if (Scores[ScoreID][(Difficulty)i] != null)
                        {
                            Difficulty = (Difficulty)i;
                            break;
                        }
                    }
                }

                // モードの変更
                if (Input.KeyPush(Keys.Backspace))
                {
                    --CurrentMode;
                    Phase = -15;
                }

                // シーンの変更
                if (Input.KeyPush(Keys.Enter))
                {
                    Sound.Stop(SoundID);
                    Engine.ChangeSceneWithTransition(
                        new GameScene(),
                        new TransitionFade(1, 1)
                    );
                }
            }
        }
    }
}
