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
            set => DiffLayer.SelectedDifficulty = value;
        }

        // レイヤー
        private readonly TuneLayer TuneLayer;
        private readonly DifficultyLayer DiffLayer;

        // 再生されている音声のID
        private int SoundID;

        public SelectScene(Score init_score = null, int sound_id = 0)
        {
            // コンポーネントを追加
            AddComponent(new BackgroundComponent("Shader/OpenGL/Select.glsl"), "Background");
            AddComponent(new InputManageComponent(), "Input");
            AddComponent(new FixedUIComponent("Tune Select"), "FixedUI");

            // インスタンスを代入
            TuneLayer = new TuneLayer(init_score) { DrawingPriority = 1, IsDrawn = false };
            DiffLayer = new DifficultyLayer { DrawingPriority = 1, IsDrawn = false };

            // SoundIDの設定
            SoundID = sound_id;
        }

        protected override void OnRegistered()
        {
            // レイヤーの追加
            if (!TuneLayer.IsDrawn)
            {
                AddLayer(TuneLayer);
                TuneLayer.IsDrawn = true;
            }
            if (!DiffLayer.IsDrawn)
            {
                AddLayer(DiffLayer);
                DiffLayer.IsDrawn = true;
            }
        }

        protected override void OnStartUpdating()
        {
            // BGMを再生する
            if (!Sound.GetIsPlaying(SoundID)) PlayBGM();
        }
        
        protected override void OnUpdated()
        {
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
                        Engine.ChangeSceneWithTransition(new GamePlayScene(Score, Difficulty), new TransitionFade(1, 1));
                        break;
                }
            }

            else if (Input.KeyPush(Keys.RightShift))
                Engine.ChangeSceneWithTransition(new OptionScene(Score, SoundID), new TransitionFade(1, 1));
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
            Sound.SetVolume(SoundID, 70);
        }

        public void StopBGM()
        {
            Sound.Stop(SoundID);
        }
    }
}
