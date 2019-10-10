using asd;

namespace DiscoFreaks
{
    public class PlaySettingLayer : Layer2D
    {
        // レイヤーが登録されているシーン
        private new OptionScene Scene
        {
            get => (OptionScene)base.Scene;
        }

        // コンポーネント
        public UIComponent UIComponent
        {
            get => (UIComponent)GetComponent("UI");
        }
        public LoopingUIComponent LoopingUIComponent
        {
            get => (LoopingUIComponent)GetComponent("LoopingUI");
        }

        private readonly GridGazer PlaySetting;
        private readonly GridGazer HighSpeed;
        private readonly GridGazer Ofset;
        private readonly GridGazer AutoMode;

        public PlaySettingLayer()
        {
            // コンポーネントを作成・追加
            AddComponent(new UIComponent(), "UI");
            AddComponent(new LoopingUIComponent(3), "LoopingUI");

            Vector2DF center = new Vector2DF(0.5f, 0.0f);
            PlaySetting = new GridGazer(48, 4, center)
            {
                Text = "Play Setting",
                Position = new Vector2DF(480, 150)
            };
            HighSpeed = new GridGazer(36, 4, center) { Position = new Vector2DF(480, 250) };
            Ofset = new GridGazer(36, 4, center) { Position = new Vector2DF(480, 300) };
            AutoMode = new GridGazer(36, 4, center) { Position = new Vector2DF(480, 350) };
        }

        protected override void OnAdded()
        {
            AddObject(PlaySetting);
            AddObject(HighSpeed);
            AddObject(Ofset);
            AddObject(AutoMode);
        }

        protected override void OnUpdated()
        {
            var config = Scene.Configuration;

            // View
            //--------------------------------------------------

            HighSpeed.Text = string.Format("High Speed : x{0}", config.HighSpeed.ToString("0.0"));
            Ofset.Text = string.Format("Ofset : {0}ms", config.Ofset.ToString("+0;-0;±0"));
            AutoMode.Text = string.Format("Auto Play : " + (config.AutoMode ? "ON" : "OFF"));

            HighSpeed.Color =
                Ofset.Color =
             AutoMode.Color = new Color(255, 255, 255, 63);
            switch (Scene.SettingItem)
            {
                case OptionScene.MenuItem.HighSpeed:
                    HighSpeed.Color = new Color(255, 255, Scene.SettingSwitch ? 0 : 255, 255);
                    Scene.ItemDescription.Text = "ノーツが落下する速度を設定することができます。";
                    break;
                case OptionScene.MenuItem.Ofset:
                    Ofset.Color = new Color(255, 255, Scene.SettingSwitch ? 0 : 255, 255);
                    Scene.ItemDescription.Text = "判定のタイミングを設定することができます。\nタイミングが早いと思った場合はプラス、\n遅いと思った場合はマイナスに設定してください。";
                    break;
                case OptionScene.MenuItem.AutoMode:
                    AutoMode.Color = new Color(255, 255, Scene.SettingSwitch ? 0 : 255, 255);
                    Scene.ItemDescription.Text = "自動演奏を有効にするかを設定することが\nできます。";
                    break;
            }
            //--------------------------------------------------



            // Controll
            //--------------------------------------------------
            if (Scene.CurrentMode == OptionScene.Mode.PlaySetting)
            {
                if (!Scene.SettingSwitch)
                {
                    if (Input.KeyPush(Keys.Up))
                        Scene.SettingItem = (OptionScene.MenuItem)Math.Mod((int)Scene.SettingItem - 1, 3);

                    if (Input.KeyPush(Keys.Down))
                        Scene.SettingItem = (OptionScene.MenuItem)Math.Mod((int)Scene.SettingItem + 1, 3);
                }

                else if (Scene.SettingItem == OptionScene.MenuItem.HighSpeed)
                {
                    config.HighSpeed +=
                        Input.KeyPush(Keys.Right) ? 0.5 :
                        Input.KeyPush(Keys.Left) ? -0.5 : 0;

                    config.HighSpeed =
                        Math.Clamp(config.HighSpeed, 1, 20);
                }

                else if (Scene.SettingItem == OptionScene.MenuItem.Ofset)
                {
                    config.Ofset +=
                        Input.KeyPush(Keys.Right) ? 10 :
                        Input.KeyPush(Keys.Left) ? -10 : 0;
                }

                else if (Scene.SettingItem == OptionScene.MenuItem.AutoMode)
                {
                    if(Input.KeyPush(Keys.Right) || Input.KeyPush(Keys.Left))
                        config.AutoMode = !config.AutoMode;
                }
            }
            //--------------------------------------------------

            Scene.Configuration = config;
        }
    }
}
