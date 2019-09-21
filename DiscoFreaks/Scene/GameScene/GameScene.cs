using asd;

namespace DiscoFreaks
{
    public class GameScene : Scene
    {
        // ゲームの設定
        public readonly Score Score;
        public readonly Difficulty Difficulty;
        public readonly Configuration Configuration;

        // ゲームの結果
        // private Result Result;

        // レイヤー
        private readonly Layer2D BackLayer;
        private readonly GroundLayer GroundLayer;
        private readonly NoteLayer NoteLayer;
        private readonly InfoLayer InfoLayer;
        private readonly Layer2D EffectLayer;

        public GameScene(Score Score, Difficulty Difficulty, Configuration Configuration)
        {
            this.Configuration = Configuration;
            this.Difficulty = Difficulty;
            this.Score = Score;

            BackLayer = new Layer2D();
            GroundLayer = new GroundLayer();
            NoteLayer = new NoteLayer();
            InfoLayer = new InfoLayer();
            EffectLayer = new Layer2D();
        }

        protected override void OnRegistered()
        {
            var back = new GameBackGround("Shader/OpenGL/Game.glsl");
            back.Luminance = Configuration.Luminance;
            BackLayer.AddPostEffect(back);

            for(int i = 0; i < 23; ++i)
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
                        Position = new Vector2DF(120 + 30 * i, 600)
                    }
                );
            }

            AddLayer(BackLayer);
            AddLayer(GroundLayer);
            AddLayer(NoteLayer);
            AddLayer(InfoLayer);
            AddLayer(EffectLayer);

            // Result = new Result();

            Note.HighSpeed = Configuration.HighSpeed;
            Note.Ofset = Configuration.Ofset;
            Note.NoteTimer.Ofset = Score[Difficulty].Ofset;
        }

        protected override void OnTransitionFinished()
        {
            Sound.Play(Sound.CreateBGM(Score.SoundPath));
            Note.NoteTimer.Start();
        }

        public void AddEffect(Vector2DF Position)
        {
            HitEffect effect = null;
            float scale = Configuration.EffectSize * 4;
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
