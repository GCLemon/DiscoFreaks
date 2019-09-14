using System.Diagnostics;
using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// 背景に使用するポストエフェクト
    /// </summary>
    public class BackGround : PostEffect
    {
        private readonly Stopwatch Stopwatch;
        protected readonly Material2D Material;

        public BackGround(string path)
        {
            // マテリアルの生成
            Material = Graphics.CreateMaterial(path);

            // ストップウォッチの生成・開始
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        protected override void OnDraw(RenderTexture2D dst, RenderTexture2D src)
        {
            // 変数の設定
            Material.SetVector2DF("resolution", Engine.WindowSize.To2DF());
            Material.SetFloat("time", Stopwatch.ElapsedMilliseconds * 0.001f);

            // マテリアルを用いて描画
            DrawOnTexture2DWithMaterial(dst, Material);
        }
    }

    /// <summary>
    /// 実際にゲームを行うシーンで使用するポストエフェクト
    /// </summary>
    public class GameBackGround : BackGround
    {
        /// <summary>
        /// 背景の明るさを設定する
        /// </summary>
        public float Luminance { set => Material.SetFloat("luminance", value); }

        public GameBackGround(string path) : base(path) { }
    }
}
