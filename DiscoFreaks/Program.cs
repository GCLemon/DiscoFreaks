using System.IO;
using asd;

namespace DiscoFreaks
{
    class MainClass
    {
        public static void Main()
        {
            // 必要なディレクトリを用意
            if(!Directory.Exists("HighScore"))
                Directory.CreateDirectory("HighScore");

            // ビルドモードによって初期化条件を切り替える
            EngineOption option = new EngineOption
            {
                ColorSpace = ColorSpaceType.LinearSpace,
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
#if DEBUG
            var IsFirstResult = false;
            if(IsFirstResult)
            {
                var score = Score.CreateList()[5];
                var diff = Difficulty.Casual;
                var result = new Result(score[diff]);
                foreach(var n in score[diff].Notes)
                {
                    if (n is TapNote) result.ChangePointByTapNote(Judgement.Just);
                    if (n is HoldNote) result.ChangePointByHoldNote(Judgement.Just);
                    if (n is SlideNote) result.ChangePointBySlideNote(Judgement.Just);
                }
                Engine.ChangeScene(new ResultScene(score, diff, result));
            }
            else
            {
                Engine.ChangeSceneWithTransition(new TitleScene(), new TransitionFade(0, 1));
            }
#else
            Engine.ChangeSceneWithTransition(new TitleScene(), new TransitionFade(0, 1));
#endif

            // エンジンの更新処理
            while (Engine.DoEvents())
            {
                Engine.Update();

                if(Input.KeyPush(Keys.Escape)) break;
            }

            // エンジンの終了宣言
            Engine.Terminate();
        }
    }
}
