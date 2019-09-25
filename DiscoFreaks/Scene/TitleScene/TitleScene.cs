using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// タイトルシーン
    /// </summary>
    public class TitleScene : Scene
    {
        // タイトルシーンでのメニューの項目
        private enum MenuItem
        {
            StartGame,
            Tutorial,
            Credits,
            QuitGame
        }

        // 現在選択している項目
        private MenuItem SelectingItem;

        // レイヤー
        private readonly Layer2D TextLayer;

        // メニューアイテム
        private readonly GridGazer[] MenuItems;

        public TitleScene()
        {
            // コンポーネントを追加
            AddComponent(new BackgroundComponent("Shader/OpenGL/Title.glsl", 100), "Background");
            AddComponent(new InputManageComponent(), "Input");

            // インスタンスを代入
            TextLayer = new Layer2D { DrawingPriority = 1 };
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

        protected override void OnRegistered()
        {
            // 中心座標
            Vector2DF center = new Vector2DF(0.5f, 0.5f);

            // シーンのタイトル
            ScoreDozer title = new ScoreDozer(96, 0, center)
            {
                Text = "Disco Freaks",
                Position = new Vector2DF(480, 120)
            };
            TextLayer.AddObject(title);

            // メニューアイテム
            foreach (var item in MenuItems)
                TextLayer.AddObject(item);

            // レイヤーの追加
            AddLayer(TextLayer);
        }

        protected override void OnUpdated()
        {
            // メニュー項目の色を設定する
            for(int i = 0; i < 4; ++i)
            {
                var alpha = (SelectingItem == (MenuItem)i) ? 255 : 63;
                MenuItems[i].Color = new Color(255, 255, 255, alpha);
            }

            // ユーザー操作を受付
            if (Input.KeyPush(Keys.Up)) --SelectingItem;
            if (Input.KeyPush(Keys.Down)) ++SelectingItem;
            var id = Math.Mod((int)SelectingItem, 4);
            SelectingItem = (MenuItem)id;

            if (Input.KeyPush(Keys.Enter))
                switch (SelectingItem)
                {
                    case MenuItem.StartGame:
                        Engine.ChangeSceneWithTransition(new SelectScene(), new TransitionFade(1, 1));
                        break;
                    case MenuItem.Tutorial:
                        break;
                    case MenuItem.Credits:
                        break;
                    case MenuItem.QuitGame:
                        Engine.Close();
                        break;
                }
        }
    }
}
