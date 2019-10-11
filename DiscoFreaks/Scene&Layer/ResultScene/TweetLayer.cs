using asd;
using CoreTweet;

namespace DiscoFreaks
{
    public class TweetLayer : Layer2D
    {
        // リザルトシーンのモード
        public enum Mode
        {
            Share,
            Authorize,
            Finished,
        }

        // 現在のモード
        public Mode CurrentMode;
        public bool IsTweet;

        //
        public new ResultScene Scene
        {
            get => (ResultScene)base.Scene;
        }

        // 
        public readonly Makinas Announce;
        public readonly Makinas Yes;
        public readonly Makinas No;
        public readonly Makinas Finished;

        public readonly PINObject PINObject;

        public TweetLayer()
        {
            // コンポーネントを作成・追加
            AddComponent(new UIComponent(), "UI");

            var center = new Vector2DF(0.5f, 0.5f);
            Announce = new Makinas(48, 4, center)
            {
                Text = "プレイ結果をシェアしますか?",
                Position = new Vector2DF(1440, 250)
            };
            Yes = new Makinas(210, 4, center)
            {
                Text = "YES",
                Position = new Vector2DF(1200, 450)
            };
            No = new Makinas(210, 4, center)
            {
                Text = "NO",
                Position = new Vector2DF(1680, 450)
            };
            Finished = new Makinas(48, 4, center)
            {
                Text = "シェアが完了しました。\n詳細はTwitterを\nご確認ください。",
                Color = new Color(255, 255, 255, 0),
                Position = new Vector2DF(1440, 400)
            };

            PINObject = new PINObject();
        }

        protected override void OnAdded()
        {
            Announce.AddComponent(new FadeOutComponent(30, 255), "FadeOut");
            Yes.AddComponent(new FadeOutComponent(30, Yes.Color.A), "FadeOut");
            No.AddComponent(new FadeOutComponent(30, No.Color.A), "FadeOut");
            Finished.AddComponent(new FadeInComponent(30, 255), "FadeIn");

            AddObject(Announce);
            AddObject(Yes);
            AddObject(No);
            AddObject(Finished);

            AddObject(PINObject);
        }
    }
}
