using System.Collections.Generic;
using asd;

namespace DiscoFreaks
{
    public abstract class GameScene : Scene
    {
        // ゲームの設定
        public readonly Score Score;
        public readonly Difficulty Difficulty;
        public readonly Configuration Configuration;

        // ゲームの結果
        public Result Result;

        // 再生されている音源のID
        protected int SoundID;

        // レイヤー
        private readonly Layer2D MaskLayer;
        private readonly GroundLayer GroundLayer;
        private readonly NoteLayer NoteLayer;
        private readonly InfoLayer InfoLayer;
        protected readonly Layer2D EffectLayer;
        protected readonly PauseLayer PauseLayer;

        // ゲームの状態遷移に用いるコルーチン
        IEnumerator<object> Coroutine;

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
            GroundLayer = new GroundLayer { DrawingPriority = 1 };
            NoteLayer = new NoteLayer(score[difficulty]) { DrawingPriority = 2 };
            EffectLayer = new Layer2D { DrawingPriority = 3 };
            InfoLayer = new InfoLayer(score, difficulty) { DrawingPriority = 4 };
            MaskLayer = new Layer2D { DrawingPriority = 5 };
            PauseLayer = new PauseLayer { DrawingPriority = 6 };
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
                    Keys.Q, Keys.A, Keys.W, Keys.S, Keys.E, Keys.D, Keys.R, Keys.F, Keys.T, Keys.G,
                    Keys.Y, Keys.H, Keys.U, Keys.J, Keys.I, Keys.K, Keys.O, Keys.L, Keys.P,
                    Keys.Semicolon, Keys.LeftBracket, Keys.Apostrophe, Keys.RightBracket, Keys.Backslash
                };

                EffectLayer.AddObject(new PressEffect(keys[i]) { Position = new Vector2DF(135 + 30 * i, 600) } );
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

            //コルーチンを用意
            Coroutine = GetCoroutine();
        }

        // 更新処理
        protected override void OnUpdated() => Coroutine?.MoveNext();

        // コルーチン 
        protected abstract IEnumerator<object> GetCoroutine();

        // エフェクトの追加
        public void AddEffect(Vector2DF position)
        {
            float scale = Configuration.EffectSize * 2;
            HitEffect effect = Configuration.EffectType switch
            {
                EffectType.Simple      => new Simple(scale),
                EffectType.Explosion   => new Explosion(scale),
                EffectType.BlueInk     => new BlueInk(scale),
                EffectType.Gust        => new Gust(scale),
                EffectType.MagicCircle => new MagicCircle(scale),
                EffectType.Stardust    => new StarDust(scale),
                _ => null
            };
            effect.Position = position;
            EffectLayer.AddObject(effect);
        }
    }
}
