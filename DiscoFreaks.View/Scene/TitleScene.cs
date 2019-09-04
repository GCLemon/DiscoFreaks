using asd;
using DiscoFreaks.Core;

namespace DiscoFreaks.View
{
    /// <summary>
    /// タイトルシーン
    /// </summary>
    public class TitleScene : Scene
    {
        // モデル
        protected readonly GameModel GameModel;
        protected readonly TitleSceneModel SceneModel;

        // レイヤー
        private readonly Layer2D BackLayer = new Layer2D();
        private readonly Layer2D TextLayer = new Layer2D();

        // メニューアイテム
        private readonly GridGazer[] MenuItems =
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

        public TitleScene(GameModel game_model)
        {
            GameModel = game_model;
            SceneModel = new TitleSceneModel();
        }

        protected override void OnRegistered()
        {
            // 背景の設定
            BackLayer.AddPostEffect(new BackGround("Shader/OpenGL/Title.glsl"));

            // テキストの処理
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
            AddLayer(BackLayer);
            AddLayer(TextLayer);
        }

        protected override void OnUpdated()
        {
            // メニュー項目の色を設定する
            foreach (var item in MenuItems)
                item.Color = new Color(255, 255, 255, 63);
            var id = (int)SceneModel.SelectingItem;
            MenuItems[id].Color = new Color(255, 255, 255, 255);

            // ユーザー操作を受付
            if (Input.KeyPush(Keys.Up)) SceneModel.PrevItem();
            if (Input.KeyPush(Keys.Down)) SceneModel.NextItem();
            if (Input.KeyPush(Keys.Enter))
                switch (SceneModel.SelectingItem)
                {
                    case TitleSceneModel.MenuItem.StartGame:
                        Engine.ChangeScene(new SelectScene(GameModel));
                        break;
                    case TitleSceneModel.MenuItem.Tutorial:
                        break;
                    case TitleSceneModel.MenuItem.Credits:
                        break;
                    case TitleSceneModel.MenuItem.QuitGame:
                        Engine.Close();
                        break;
                }
        }
    }
}
