using asd;

namespace DiscoFreaks
{
    public class AudioSettingLayer : Layer2D
    {
        // レイヤーが登録されているシーン
        private new OptionScene Scene
        {
            get => (OptionScene)base.Scene;
        }

        // コンポーネント
        public readonly UIComponent UIComponent;
        public readonly LoopingUIComponent LoopingUIComponent;

        private readonly GridGazer AudioSetting;
        private readonly GridGazer BGMVolume;
        private readonly GridGazer SEVolume;

        public AudioSettingLayer()
        {
            UIComponent = new UIComponent();
            LoopingUIComponent = new LoopingUIComponent(3);

            Vector2DF center = new Vector2DF(0.5f, 0.0f);
            AudioSetting = new GridGazer(48, 4, center)
            {
                Text = "Audio Setting",
                Position = new Vector2DF(-480, 150)
            };
            BGMVolume = new GridGazer(36, 4, center) { Position = new Vector2DF(-480, 250) };
            SEVolume = new GridGazer(36, 4, center) { Position = new Vector2DF(-480, 300) };
        }

        protected override void OnAdded()
        {
            // コンポーネントを追加
            AddComponent(UIComponent, "UI");
            AddComponent(LoopingUIComponent, "LoopingUI");

            AddObject(AudioSetting);
            AddObject(BGMVolume);
            AddObject(SEVolume);
        }

        protected override void OnUpdated()
        {
            var config = Scene.Configuration;

            // View
            //--------------------------------------------------

            BGMVolume.Text = string.Format("BGM Volume : {0}", config.BGMVolume);
            SEVolume.Text = string.Format("SE Volume : {0}", config.SEVolume);

            BGMVolume.Color =
                SEVolume.Color = new Color(255, 255, 255, 63);

            switch (Scene.SettingItem)
            {
                case OptionScene.MenuItem.BGMVolume:
                    BGMVolume.Color = new Color(255, 255, Scene.SettingSwitch ? 0 : 255, 255);
                    Scene.ItemDescription.Text = "BGMの音量を設定することができます。";
                    break;
                case OptionScene.MenuItem.SEVolume:
                    SEVolume.Color = new Color(255, 255, Scene.SettingSwitch ? 0 : 255, 255);
                    Scene.ItemDescription.Text = "効果音の音量を設定することができます。";
                    break;
            }

            //--------------------------------------------------



            // Controll
            //--------------------------------------------------
            if (Scene.CurrentMode == OptionScene.Mode.AudioSetting)
            {
                if (!Scene.SettingSwitch)
                {
                    if (Input.KeyPush(Keys.Up))
                        Scene.SettingItem = (OptionScene.MenuItem)(Math.Mod((int)Scene.SettingItem - 8, 2) + 7);

                    if (Input.KeyPush(Keys.Down))
                        Scene.SettingItem = (OptionScene.MenuItem)(Math.Mod((int)Scene.SettingItem - 6, 2) + 7);
                }

                else if (Scene.SettingItem == OptionScene.MenuItem.BGMVolume)
                {
                    config.BGMVolume +=
                        Input.KeyPush(Keys.Right) ? 5 :
                        Input.KeyPush(Keys.Left) ? -5 : 0;

                    config.BGMVolume =
                        (int)Math.Clamp(config.BGMVolume, 0, 100);
                }

                else if (Scene.SettingItem == OptionScene.MenuItem.SEVolume)
                {
                    config.SEVolume +=
                        Input.KeyPush(Keys.Right) ? 5 :
                        Input.KeyPush(Keys.Left) ? -5 : 0;

                    config.SEVolume =
                        (int)Math.Clamp(config.SEVolume, 0, 100);
                }
            }
            //--------------------------------------------------

            Scene.Configuration = config;
        }
    }
}
