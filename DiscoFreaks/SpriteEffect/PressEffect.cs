using asd;

namespace DiscoFreaks
{
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

            if (Input.KeyPush(Key)) { Count = 0; }
        }
    }
}
