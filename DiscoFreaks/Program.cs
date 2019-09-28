using asd;

namespace DiscoFreaks
{
    class MainClass
    {
        public static void Main()
        {
            // ビルドモードによって初期化条件を切り替える
            EngineOption option = new EngineOption
            {
#if DEBUG
                GraphicsDevice = GraphicsDeviceType.OpenGL,
                WindowPosition = WindowPositionType.Centering
#else
                IsFullScreen = true
#endif
            };

            // エンジンの初期化
            Engine.Initialize("Disco Freaks", 960, 720, option);

#if DEBUG
            // ルートディレクトリの登録
            Engine.File.AddRootDirectory("./Resource");
#else
            // リソースパックの読み込み
            Engine.File.AddRootPackageWithPassword("Resource.pack", "DISCO_FREAKS");
#endif

            // シーンチェンジ
            Engine.ChangeSceneWithTransition(new TitleScene(), new TransitionFade(0, 1));


            // エンジンの更新処理
            while (Engine.DoEvents())
            {
                Engine.Update();

                if(Input.KeyPush(Keys.Escape))
                {
                    break;
                }
            }

            // エンジンの終了宣言
            Engine.Terminate();
        }
    }
}
