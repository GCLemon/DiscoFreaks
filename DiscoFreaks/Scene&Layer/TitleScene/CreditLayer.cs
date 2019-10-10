using asd;

namespace DiscoFreaks
{
    public class CreditObject : Makinas
    {
        private Makinas Author;

        public CreditObject(string item, string author) : base(24, 4)
        {
            Text = item;

            Author = new Makinas(18, 4)
            {
                Text = author,
                Position = new Vector2DF(15, 30)
            };
        }

        protected override void OnAdded()
        {
            var m = ChildManagementMode.RegistrationToLayer;
            var t = ChildTransformingMode.Position;
            AddChild(Author, m, t);
        }
    }

    public class CreditLayer : Layer2D
    {
        private CreditObject Altseed;
        private CreditObject CoreTweet;
        private CreditObject x0y0pxFreeFont;
        private CreditObject Makinas;
        private CreditObject Musics;
        private CreditObject Others;
        private CreditObject SpecialThanks;
        private CreditObject AndYou;

        // コンポーネント
        public UIComponent UIComponent
        {
            get => (UIComponent)GetComponent("UI");
        }

        public CreditLayer()
        {
            Altseed = new CreditObject("ゲームエンジン  \"Altseed\"", "Altseed : http://altseed.github.io/")
            {
                Position = new Vector2DF(990, 50)
            };
            CoreTweet = new CreditObject("Twitter ライブラリ \"Core Tweet\"", "Core Tweet : https://coretweet.github.io")
            {
                Position = new Vector2DF(990, 130)
            };
            x0y0pxFreeFont = new CreditObject("フォント　\"x0y0pxFreeFont シリーズ\"", "患者長ひっく様 : http://www17.plala.or.jp/xxxxxxx/00ff/")
            {
                Position = new Vector2DF(990, 210)
            };
            Makinas = new CreditObject("フォント　\"マキナス Scrap 5\"", "もじワク研究様 : https://moji-waku.com/makinas/")
            {
                Position = new Vector2DF(990, 290)
            };
            Musics = new CreditObject("収録楽曲", "Tsukuba DTM Lab. の皆様 : https://tsukubadtm.bandcamp.com/")
            {
                Position = new Vector2DF(990, 370)
            };
            Others = new CreditObject("その他プログラム・画像・スクリプト等々", "檸檬茶 : @Hi_Coders")
            {
                Position = new Vector2DF(990, 450)
            };
            SpecialThanks = new CreditObject("テストプレイ・制作に関するアドバイス等々", "サークルメンバー・および友人の方々")
            {
                Position = new Vector2DF(990, 530)
            };
            AndYou = new CreditObject("...そして、このゲームをプレイしてくださっているあなた", "この度は Disco Freaks をご遊戯いただき、誠にありがとうございます。")
            {
                Position = new Vector2DF(990, 610)
            };
        }

        protected override void OnAdded()
        {
            // コンポーネントを作成・追加
            AddComponent(new UIComponent(), "UI");

            // 表示する項目を追加
            AddObject(Altseed);
            AddObject(CoreTweet);
            AddObject(x0y0pxFreeFont);
            AddObject(Makinas);
            AddObject(Musics);
            AddObject(Others);
            AddObject(SpecialThanks);
            AddObject(AndYou);
        }
    }
}
