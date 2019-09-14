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
        public Mode CurrentMode { get; private set; }

        // 読み込んだ譜面
        public Score Score
        {
            get => TuneLayer.SelectedScore;
        }

        // 現在選択している難易度
        public Difficulty Difficulty
        {
            get => DiffLayer.SelectedDifficulty;
        }

        // レイヤー
        private readonly Layer2D BackLayer = new Layer2D();
        private readonly Layer2D TextLayer = new Layer2D();
        private readonly MusicLayer TuneLayer = new MusicLayer();
        private readonly DifficultyLayer DiffLayer = new DifficultyLayer();

        // シーンのタイトル
        private readonly HeadUpDaisy SceneTitle =
            new HeadUpDaisy(72, 4, new Vector2DF(0.5f, 0))
            {
                Text = "Music Select",
                Position = new Vector2DF(480, 10)
            };

        private bool IsUsed;
        private int SoundID;

        protected override void OnRegistered()
        {
            if (!IsUsed)
            {
                // 背景の設定
                BackLayer.AddPostEffect(new BackGround("Shader/OpenGL/Select.glsl"));

                // シーンのタイトルを設定
                TextLayer.AddObject(SceneTitle);

                // レイヤーの追加
                AddLayer(BackLayer);
                AddLayer(TuneLayer);
                AddLayer(DiffLayer);
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

            if (Input.KeyPush(Keys.Backspace))
            {
                switch (CurrentMode)
                {
                    case Mode.Music:
                        StopBGM();
                        Engine.ChangeSceneWithTransition(
                            new TitleScene(),
                            new TransitionFade(1, 1)
                        );
                        break;
                    case Mode.Difficulty:
                        CurrentMode = Mode.Music;
                        TuneLayer.UIComponent.MoveRight();
                        DiffLayer.UIComponent.MoveRight();
                        break;
                }
            }

            if (Input.KeyPush(Keys.Enter))
            {
                switch (CurrentMode)
                {
                    case Mode.Music:
                        CurrentMode = Mode.Difficulty;
                        TuneLayer.UIComponent.MoveLeft();
                        DiffLayer.UIComponent.MoveLeft();
                        break;
                    case Mode.Difficulty:
                        StopBGM();
                        Engine.ChangeSceneWithTransition(
                            new GameScene(new GameInfo
                            {
                                Score = Score,
                                Difficulty = Difficulty,
                                Configuration = Configuration.Load()
                            }),
                            new TransitionFade(1, 1)
                        );
                        break;
                }
            }

            if (Input.KeyPush(Keys.RightShift))
            {
                Engine.ChangeSceneWithTransition(
                    new OptionScene(this),
                    new TransitionFade(1, 1),
                    false
                );
            }
        }

        // 音を変更する
        public void PlayBGM()
        {
            Sound.Stop(SoundID);
            string score = Score.SoundPath;
            var source = Sound.CreateBGM(score);
            source.IsLoopingMode = true;
            source.LoopStartingPoint = 0;
            source.LoopEndPoint = source.Length;
            SoundID = Sound.Play(source);

        }

        public void StopBGM()
        {
            Sound.Stop(SoundID);
        }
    }
}
