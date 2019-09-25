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
        private readonly Material2D Material;
        private readonly int Luminance;

        public BackGround(string path, int luminance)
        {
            // マテリアルの生成
            Material = Graphics.CreateMaterial(path);

            // ストップウォッチの生成・開始
            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            // 明るさの設定
            Luminance = luminance;
        }

        protected override void OnDraw(RenderTexture2D dst, RenderTexture2D src)
        {
            // 変数の設定
            Material.SetVector2DF("resolution", Engine.WindowSize.To2DF());
            Material.SetFloat("time", Stopwatch.ElapsedMilliseconds * 0.001f);
            Material.SetFloat("luminance", Luminance);

            // マテリアルを用いて描画
            DrawOnTexture2DWithMaterial(dst, Material);
        }
    }
}
