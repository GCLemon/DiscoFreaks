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
                GraphicsDevice = GraphicsDeviceType.OpenGL,
#if DEBUG
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
            /*
            var info = InitInfo.Create("Score/Rosetta_Reflection/Casual.frk");
            var score = new Score(info);
            var result = new Result(score[Difficulty.Casual]);
            foreach(var i in info.DetailInfo.Notes)
            {
                switch (i.Type)
                {
                    case NoteType.TapNote:
                        result.ChangePointByTapNote(Judgement.Just);
                        break;
                    case NoteType.HoldNote:
                        result.ChangePointByHoldNote(Judgement.Just);
                        break;
                    case NoteType.SlideNote:
                        result.ChangePointBySlideNote(Judgement.Just);
                        break;
                }
            }
            Engine.ChangeScene(new ResultScene(score, Difficulty.Casual, result, true));
            */
            Engine.ChangeSceneWithTransition(new TitleScene(), new TransitionFade(0, 1));
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
