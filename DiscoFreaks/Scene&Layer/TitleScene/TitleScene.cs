using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// タイトルシーン
    /// </summary>
    public class TitleScene : Scene
    {
        // レイヤー
        private readonly MenuLayer MenuLayer;
        private readonly CreditLayer CreditLayer;

        private bool IsShowingCredits;

        public TitleScene()
        {
            // コンポーネントを追加
            AddComponent(new BackgroundComponent("Shader/OpenGL/Title.glsl"), "Background");
            AddComponent(new InputManageComponent(), "Input");

            // インスタンスを代入
            MenuLayer = new MenuLayer { DrawingPriority = 1 };
            CreditLayer = new CreditLayer { DrawingPriority = 2 };
        }

        protected override void OnRegistered()
        {
            // レイヤーの追加
            AddLayer(MenuLayer);
            AddLayer(CreditLayer);
        }

        protected override void OnUpdated()
        {
            // ユーザー操作を受付
            if (Input.KeyPush(Keys.Up)) --MenuLayer.SelectingItem;
            if (Input.KeyPush(Keys.Down)) ++MenuLayer.SelectingItem;
            var id = Math.Mod((int)MenuLayer.SelectingItem, 4);
            MenuLayer.SelectingItem = (MenuLayer.MenuItem)id;

            if (Input.KeyPush(Keys.Enter))
            {
                if(IsShowingCredits)
                {
                    IsShowingCredits = false;
                    MenuLayer.UIComponent.MoveRight();
                    CreditLayer.UIComponent.MoveRight();
                }
                else
                {
                    switch (MenuLayer.SelectingItem)
                    {
                        case MenuLayer.MenuItem.StartGame:
                            Engine.ChangeSceneWithTransition(new SelectScene(), new TransitionFade(1, 1));
                            break;
                        case MenuLayer.MenuItem.Tutorial:
                            break;
                        case MenuLayer.MenuItem.Credits:
                            IsShowingCredits = true;
                            MenuLayer.UIComponent.MoveLeft();
                            CreditLayer.UIComponent.MoveLeft();
                            break;
                        case MenuLayer.MenuItem.QuitGame:
                            Engine.Close();
                            break;
                    }
                }
            }
        }
    }
}
