using asd;

namespace DiscoFreaks
{
    public class MenuLayer : Layer2D
    {
        // タイトルシーンでのメニューの項目
        public enum MenuItem
        {
            StartGame,
            Tutorial,
            Credits,
            QuitGame
        }

        // 現在選択している項目
        public MenuItem SelectingItem;

        // メニューアイテム
        private readonly GridGazer[] MenuItems;

        // コンポーネント
        public UIComponent UIComponent
        {
            get => (UIComponent)GetComponent("UI");
        }

        public MenuLayer()
        {
            MenuItems = new GridGazer[]
            {
                new GridGazer(48, 4, new Vector2DF(0.5f, 0.5f))
                {
                    Text = "Start Game",
                    Position = new Vector2DF(480, 500)
                },
                new GridGazer(48, 4, new Vector2DF(0.5f, 0.5f))
                {
                    Text = "Tutorial",
                    Position = new Vector2DF(480, 560)
                },
                new GridGazer(48, 4, new Vector2DF(0.5f, 0.5f))
                {
                    Text = "Credits",
                    Position = new Vector2DF(480, 620)
                },
                new GridGazer(48, 4, new Vector2DF(0.5f, 0.5f))
                {
                    Text = "Quit Game",
                    Position = new Vector2DF(480, 680)
                }
            };
        }

        protected override void OnAdded()
        {
            // シーンのタイトル
            ScoreDozer title = new ScoreDozer(96, 0, new Vector2DF(0.5f, 0.5f))
            {
                Text = "Disco Freaks",
                Position = new Vector2DF(480, 120)
            };
            AddObject(title);

            // コンポーネントを作成・追加
            AddComponent(new UIComponent(), "UI");

            // メニュー項目の追加
            foreach (var item in MenuItems) AddObject(item);
        }

        protected override void OnUpdated()
        {
            for (int i = 0; i < 4; ++i)
            {
                var alpha = (SelectingItem == (MenuItem)i) ? 255 : 63;
                MenuItems[i].Color = new Color(255, 255, 255, alpha);
            }
        }
    }
}
