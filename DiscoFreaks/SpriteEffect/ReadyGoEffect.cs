using asd;

namespace DiscoFreaks
{
    public class ReadyGoEffect : TextureObject2D
    {
        /// <summary>
        /// フレーム数
        /// </summary>
        private int Count;

        public ReadyGoEffect()
        {
            // 初期化
            Texture = Graphics.CreateTexture("Effect/ReadyGo/ReadyGo.0.png");
        }

        protected override void OnUpdate()
        {
            // 描画範囲の設定
            Texture = Graphics.CreateTexture("Effect/ReadyGo/ReadyGo." + Count + ".png");

            // エフェクトの再生が終わったらDispose
            if (++Count >= 240) Dispose();
        }
    }
}
