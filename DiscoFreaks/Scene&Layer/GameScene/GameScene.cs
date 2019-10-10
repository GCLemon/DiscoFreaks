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
        private readonly Layer2D MaskLayer;
        private readonly GroundLayer GroundLayer;
        private readonly NoteLayer NoteLayer;
        private readonly InfoLayer InfoLayer;
        private readonly Layer2D EffectLayer;
        private readonly PauseLayer PauseLayer;

        // ゲーム開始時に再生するエフェクト
        private readonly ReadyGoEffect ReadyGo;

        public GameScene(Score score, Difficulty difficulty)
        {
            // ユーザー設定の読み込み
            Configuration = Configuration.Load();

            // 入力情報の受け取り
            Difficulty = difficulty;
            Score = score;

            // コンポーネントを追加
            AddComponent(new BackgroundComponent("Shader/OpenGL/Game.glsl"), "Background");
            AddComponent(new InputManageComponent(), "Input");

            // インスタンスを代入
            MaskLayer = new Layer2D { DrawingPriority = 1 };
            GroundLayer = new GroundLayer { DrawingPriority = 2 };
            NoteLayer = new NoteLayer(score[difficulty]) { DrawingPriority = 3 };
            EffectLayer = new Layer2D { DrawingPriority = 4 };
            InfoLayer = new InfoLayer(score, difficulty) { DrawingPriority = 5 };
            PauseLayer = new PauseLayer { DrawingPriority = 6 };

            ReadyGo = new ReadyGoEffect();
        }

        protected override void OnRegistered()
        {
            MaskLayer.AddObject(new GeometryObject2D
            {
                Shape = new RectangleShape { DrawingArea = new RectF(0, 0, 960, 720) },
                Color = new Color(0, 0, 0, (int)((100 - Configuration.Luminance) * 2.55) )
            });

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

            AddLayer(MaskLayer);
            AddLayer(GroundLayer);
            AddLayer(NoteLayer);
            AddLayer(InfoLayer);
            AddLayer(EffectLayer);
            AddLayer(PauseLayer);

            Result = new Result(Score[Difficulty]);

            Note.HighSpeed = Configuration.HighSpeed;
            Note.Ofset = Configuration.Ofset;
            Note.IsAutoPlaying = Configuration.AutoMode;
            Note.NoteTimer.Ofset = Score[Difficulty].Ofset;

            // ノートタイマーの初期化
            Note.NoteTimer.Stop();
            Note.NoteTimer.Reset();
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
                EffectLayer.AddObject(ReadyGo);

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

            // バックスペースが押されたらポーズ画面へ
            if (Input.KeyPush(Keys.Backspace))
            {
                Sound.Pause(SoundID);
                Note.NoteTimer.Stop();
                PauseLayer.IsDrawn = true;
                PauseLayer.IsUpdated = true;
                CurrentState = GameState.Pausing;
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
            if (Input.KeyPush(Keys.Up))
            {
                int x = Math.Mod((int)PauseLayer.SelectingItem - 1, 3);
                PauseLayer.SelectingItem = (PauseLayer.Item)x;
            }

            if (Input.KeyPush(Keys.Down))
            {
                int x = Math.Mod((int)PauseLayer.SelectingItem + 1, 3);
                PauseLayer.SelectingItem = (PauseLayer.Item)x;
            }

            if (Input.KeyPush(Keys.Enter))
            {
                switch (PauseLayer.SelectingItem)
                {
                    case PauseLayer.Item.Resume:
                        Sound.Resume(SoundID);
                        Note.NoteTimer.Start();
                        PauseLayer.IsDrawn = false;
                        PauseLayer.IsUpdated = false;
                        CurrentState = GameState.Playing;
                        break;
                    case PauseLayer.Item.Retry:
                        var new_scene = new GameScene(Score, Difficulty);
                        Engine.ChangeSceneWithTransition(new_scene, new TransitionFade(1, 1));
                        break;
                    case PauseLayer.Item.Return:
                        Engine.ChangeSceneWithTransition(new SelectScene(Score), new TransitionFade(1, 1));
                        break;
                }
            }
        }

        // ゲーム終了後の処理
        private void OnFinished()
        {
            if(!IsGameFinished)
            {
                var new_scene = new ResultScene(Score, Difficulty, Result, !Note.IsAutoPlaying);
                Engine.ChangeSceneWithTransition(new_scene, new TransitionFade(1, 1));
                IsGameFinished = true;
            }
        }

        public void AddEffect(Vector2DF position)
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
            effect.Position = position;
            EffectLayer.AddObject(effect);
        }
    }
}
