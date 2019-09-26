using asd;

namespace DiscoFreaks
{
    public class GameScene : Scene
    {
        // 現在のゲームの状態
        private enum GameState
        {
            Ready,
            Playing,
            Pausing,
            Finished,
        }
        private GameState CurrentState;

        // ゲームの設定
        public readonly Score Score;
        public readonly Difficulty Difficulty;
        public readonly Configuration Configuration;

        // ゲームの結果
        public Result Result;

        // フレームカウンタ
        private int FrameCount;

        // 再生されている音源のID
        private int SoundID;

        // 音が再生されたか
        private bool IsSoundStarted;

        // ゲームが終わったか
        private bool IsGameFinished;

        // レイヤー
        private readonly GroundLayer GroundLayer;
        private readonly NoteLayer NoteLayer;
        private readonly InfoLayer InfoLayer;
        private readonly Layer2D EffectLayer;

        // ゲーム開始時に再生するエフェクト
        private readonly ReadyGoEffect ReadyGo;

        public GameScene(Score Score, Difficulty Difficulty, Configuration Configuration)
        {
            // 入力情報の受け取り
            this.Configuration = Configuration;
            this.Difficulty = Difficulty;
            this.Score = Score;

            // コンポーネントを追加
            var luminance = Configuration.Luminance;
            AddComponent(new BackgroundComponent("Shader/OpenGL/Game.glsl", luminance), "Background");
            AddComponent(new InputManageComponent(), "Input");

            // インスタンスを代入
            GroundLayer = new GroundLayer { DrawingPriority = 1 };
            NoteLayer = new NoteLayer(Score[Difficulty]) { DrawingPriority = 2 };
            EffectLayer = new Layer2D { DrawingPriority = 3 };
            InfoLayer = new InfoLayer(Score, Difficulty) { DrawingPriority = 4 };

            ReadyGo = new ReadyGoEffect();
        }

        protected override void OnRegistered()
        {
            for(int i = 0; i < 24; ++i)
            {
                Keys[] keys =
                {
                    Keys.Q, Keys.A, Keys.W, Keys.S, Keys.E,
                    Keys.D, Keys.R, Keys.F, Keys.T, Keys.G,
                    Keys.Y, Keys.H, Keys.U, Keys.J, Keys.I,
                    Keys.K, Keys.O, Keys.L, Keys.P,
                    Keys.Semicolon, Keys.LeftBracket,
                    Keys.Apostrophe, Keys.RightBracket,
                    Keys.Backslash
                };

                EffectLayer.AddObject(
                    new PressEffect(keys[i])
                    {
                        Position = new Vector2DF(135 + 30 * i, 600)
                    }
                );
            }

            AddLayer(GroundLayer);
            AddLayer(NoteLayer);
            AddLayer(InfoLayer);
            AddLayer(EffectLayer);

            Result = new Result(Score[Difficulty]);

            Note.HighSpeed = Configuration.HighSpeed;
            Note.Ofset = Configuration.Ofset;
            Note.NoteTimer.Ofset = Score[Difficulty].Ofset;
        }

        protected override void OnUpdated()
        {
            switch (CurrentState)
            {
                case GameState.Ready: OnReady(); break;
                case GameState.Playing: OnPlaying(); break;
                case GameState.Pausing: OnPausing(); break;
                case GameState.Finished: OnFinished(); break;
            }

        }

        // ゲームが始まっていない場合の処理
        private void OnReady()
        {
            // エフェクトを再生する
            if (FrameCount == 180)
            {
                EffectLayer.AddObject(ReadyGo);
            }

            // エフェクトの再生が終わったらゲームを開始する
            if (!ReadyGo.IsAlive)
            {
                Note.NoteTimer.Start();
                CurrentState = GameState.Playing;
            }

            ++FrameCount;
        }

        // ゲーム中の処理
        private void OnPlaying()
        {
            // 音を再生する
            if (!IsSoundStarted && Note.NoteTimer.ElapsedMilliseconds >= 2000)
            {
                var source = Sound.CreateBGM(Score.SoundPath);
                source.IsLoopingMode = false;
                SoundID = Sound.Play(source);
                Sound.SetVolume(SoundID, Configuration.BGMVolume);
                IsSoundStarted = true;
            }

            // 音の再生が終わったらゲームを終了する
            if (IsSoundStarted && !Sound.GetIsPlaying(SoundID))
            {
                CurrentState = GameState.Finished;
            }
        }

        // ポーズ中の処理
        private void OnPausing()
        {

        }

        // ゲーム終了後の処理
        private void OnFinished()
        {
            if(!IsGameFinished)
            {
                Engine.ChangeSceneWithTransition(new ResultScene(Score, Difficulty, Result), new TransitionFade(1, 1));
                IsGameFinished = true;
            }
        }

        public void AddEffect(Vector2DF Position)
        {
            HitEffect effect = null;
            float scale = Configuration.EffectSize * 2;
            switch (Configuration.EffectType)
            {
                case EffectType.Simple:
                    effect = new Simple(scale);
                    break;
                case EffectType.Explosion:
                    effect = new Explosion(scale);
                    break;
                case EffectType.BlueInk:
                    effect = new BlueInk(scale);
                    break;
                case EffectType.Gust:
                    effect = new Gust(scale);
                    break;
                case EffectType.MagicCircle:
                    effect = new MagicCircle(scale);
                    break;
                case EffectType.Stardust:
                    effect = new StarDust(scale);
                    break;
            }
            effect.Position = Position;
            EffectLayer.AddObject(effect);
        }
    }
}
