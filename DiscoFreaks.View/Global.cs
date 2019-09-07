using System.Text;
using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// キーの押下情報を取得する
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// 入力を受け付けるか
        /// </summary>
        public static bool AcceptInput = true;

        /// <summary>
        /// キーをこのフレームで押したか
        /// </summary>
        public static bool KeyPush(Keys keys) =>
            Engine.Keyboard.GetKeyState(keys) == ButtonState.Push && AcceptInput;

        /// <summary>
        /// キーを押し続けているか
        /// </summary>
        public static bool KeyHold(Keys keys) =>
            Engine.Keyboard.GetKeyState(keys) == ButtonState.Hold && AcceptInput;

        /// <summary>
        /// キーをこのフレームで離したか
        /// </summary>
        public static bool KeyRelease(Keys keys) =>
            Engine.Keyboard.GetKeyState(keys) == ButtonState.Release && AcceptInput;

        /// <summary>
        /// キーを離し続けているか
        /// </summary>
        public static bool KeyFree(Keys keys) =>
            Engine.Keyboard.GetKeyState(keys) == ButtonState.Free || !AcceptInput;
    }

    /// <summary>
    /// 音関係の操作
    /// </summary>
    public static class Sound
    {
        /// <summary>
        /// BGM を生成する
        /// </summary>
        public static SoundSource CreateBGM(string path) =>
            Engine.Sound.CreateSoundSource(path, false);

        /// <summary>
        /// 効果音を生成する
        /// </summary>
        public static SoundSource CreateSE(string path) =>
            Engine.Sound.CreateSoundSource(path, true);

        /// <summary>
        /// 音を再生する
        /// </summary>
        public static int Play(SoundSource source) =>
            Engine.Sound.Play(source);

        /// <summary>
        /// 音を停止する
        /// </summary>
        public static void Stop(int id) =>
            Engine.Sound.Stop(id);

        /// <summary>
        /// 音を一時停止する
        /// </summary>
        public static void Pause(int id) =>
            Engine.Sound.Pause(id);

        /// <summary>
        /// 一時停止を解除する
        /// </summary>
        public static void Resume(int id) =>
            Engine.Sound.Resume(id);

        /// <summary>
        /// 一時停止を解除する
        /// </summary>
        public static void StopAll() =>
            Engine.Sound.StopAll();
    }

    /// <summary>
    /// 描画関係の操作
    /// </summary>
    public static class Graphics
    {
        /// <summary>
        /// 動的フォントを生成する
        /// </summary>
        public static Font CreateDFont(string path, int size, Color color, int o_size, Color o_color) =>
            Engine.Graphics.CreateDynamicFont(path, size, color, o_size, o_color);

        /// <summary>
        /// テクスチャを生成する
        /// </summary>
        public static Texture2D CreateTexture(string path) =>
            Engine.Graphics.CreateTexture2D(path);

        /// <summary>
        /// シェーダファイルからマテリアルを生成する
        /// </summary>
        public static Material2D CreateMaterial(string path) =>
            Engine.Graphics.CreateMaterial2D(
                Engine.Graphics.CreateShader2D(
                    new UTF8Encoding().GetString(
                        Engine.File.CreateStaticFile(path).Buffer
                    )
                )
            );

        /// <summary>
        /// エフェクトを生成する
        /// </summary>
        public static Effect CreateEffect(string path) =>
            Engine.Graphics.CreateEffect(path);
    }

    /// <summary>
    /// 列挙型関係の操作
    /// </summary>
    public static class Enum
    {
        /// <summary>
        /// 文字列から列挙型に変換する
        /// </summary>
        public static T Parse<T>(string value) =>
            (T)System.Enum.Parse(typeof(T), value);

        /// <summary>
        /// 列挙型に含まれる値を全て列挙する
        /// </summary>
        public static System.Array GetValues<T>() =>
            System.Enum.GetValues(typeof(T));
    }

    /// <summary>
    /// 計算関係の操作
    /// </summary>
    public static class Math
    {
        /// <summary>
        /// 最小非負剰余を求める
        /// </summary>
        public static int Mod(int x, int y)
        {
            return ((x % y) + y) % y;
        }

        /// <summary>
        /// 累乗を求める
        /// </summary>
        public static double Pow(double x, double y)
        {
            return System.Math.Pow(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        public static double Clamp(double x, double min, double max)
        {
            if (x < min) return min;
            if (x > max) return max;
            return x;
        }
    }
}
