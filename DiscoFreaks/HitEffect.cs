using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// このゲームで用いるヒットエフェクト 
    /// </summary>
    public abstract class HitEffect : TextureObject2D
    {
        /// <summary>
        /// フレーム数
        /// </summary>
        private int Count;

        protected HitEffect(string path, float scale)
        {
            // 諸々の初期化
            Texture = Graphics.CreateTexture(path);
            CenterPosition = new Vector2DF(128, 128);
            Scale = new Vector2DF(scale / 256, scale / 256);
            AlphaBlend = AlphaBlendMode.Add;
            Src = new RectF(0, 0, 256, 256);
        }

        protected override void OnUpdate()
        {
            // 描画範囲の設定
            Vector2DF pos = new Vector2DF(Count % 4, Count / 4) * 256;
            Vector2DF size = new Vector2DF(256, 256);
            Src = new RectF(pos, size);

            // エフェクトの再生が終わったらDispose
            if(++Count >= 24) Dispose();
        }
    }

    /// <summary>
    /// シンプルな波紋
    /// </summary>
    public class Simple : HitEffect
    {
        public Simple(float scale) :
            base("Effect/Simple.png", scale) { }
    }

    /// <summary>
    /// 爆発
    /// </summary>
    public class Explosion : HitEffect
    {
        public Explosion(float scale) :
            base("Effect/Explosion.png", scale) { }
    }

    /// <summary>
    /// インク
    /// </summary>
    public class BlueInk : HitEffect
    {
        public BlueInk(float scale) :
            base("Effect/BlueInk.png", scale) { }
    }

    /// <summary>
    /// 突風
    /// </summary>
    public class Gust : HitEffect
    {
        public Gust(float scale) :
            base("Effect/Gust.png", scale) { }
    }

    /// <summary>
    /// 魔法陣
    /// </summary>
    public class MagicCircle : HitEffect
    {
        public MagicCircle(float scale) :
            base("Effect/MagicCircle.png", scale) { }
    }

    /// <summary>
    /// 星屑
    /// </summary>
    public class StarDust : HitEffect
    {
        public StarDust(float scale) :
            base("Effect/StarDust.png", scale) { }
    }

    /// <summary>
    /// ボタンが押された時に再生
    /// </summary>
    public class PressEffect : TextureObject2D
    {
        private int Count;

        private readonly Keys Key;

        public PressEffect(Keys Key)
        {
            // 諸々の初期化
            Texture = Graphics.CreateTexture("Effect/Press.png");
            CenterPosition = new Vector2DF(128, 128);
            Scale = new Vector2DF(0.375f, 0.375f);
            AlphaBlend = AlphaBlendMode.Add;
            Src = new RectF(0, 768, 256, 256);
            Count = 12;
            this.Key = Key;
        }

        protected override void OnUpdate()
        {
            if (Count++ <= 12)
            {
                // 描画範囲の設定
                Vector2DF pos = new Vector2DF(Count % 4, Count / 4) * 256;
                Vector2DF size = new Vector2DF(256, 256);
                Src = new RectF(pos, size);
            }

            if(Input.KeyPush(Key)) { Count = 0; }
        }
    }
}
