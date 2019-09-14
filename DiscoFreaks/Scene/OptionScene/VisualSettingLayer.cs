using asd;

namespace DiscoFreaks
{
    public class VisualSettingLayer : Layer2D
    {
        // レイヤーが登録されているシーン
        private new OptionScene Scene
        {
            get => (OptionScene)base.Scene;
        }

        // コンポーネント
        public readonly UIComponent UIComponent;
        public readonly LoopingUIComponent LoopingUIComponent;

        private readonly GridGazer VisualSetting;
        private readonly GridGazer EffectType;
        private readonly GridGazer EffectSize;
        private readonly GridGazer Luminance;
        private readonly GridGazer ShowLaneBorder;
        private readonly GridGazer ShowBeatBorder;

        public VisualSettingLayer()
        {
            UIComponent = new UIComponent();
            LoopingUIComponent = new LoopingUIComponent(3);

            Vector2DF center = new Vector2DF(0.5f, 0.0f);
            VisualSetting = new GridGazer(48, 4, center)
            {
                Text = "Visual Setting",
                Position = new Vector2DF(1440, 150)
            };
            EffectType = new GridGazer(36, 4, center) { Position = new Vector2DF(1440, 250) };
            EffectSize = new GridGazer(36, 4, center) { Position = new Vector2DF(1440, 300) };
            Luminance = new GridGazer(36, 4, center) { Position = new Vector2DF(1440, 350) };
            ShowLaneBorder = new GridGazer(36, 4, center) { Position = new Vector2DF(1440, 400) };
            ShowBeatBorder = new GridGazer(36, 4, center) { Position = new Vector2DF(1440, 450) };
        }

        protected override void OnAdded()
        {
            // コンポーネントを追加
            AddComponent(UIComponent, "UI");
            AddComponent(LoopingUIComponent, "LoopingUI");

            AddObject(VisualSetting);
            AddObject(EffectType);
            AddObject(EffectSize);
            AddObject(Luminance);
            AddObject(ShowLaneBorder);
            AddObject(ShowBeatBorder);
        }

        protected override void OnUpdated()
        {
            var config = Scene.Configuration;

            // View
            //--------------------------------------------------
            switch (config.EffectType)
            {
                case DiscoFreaks.EffectType.Simple:
                    EffectType.Text = "Effect Type : Simple";
                    break;
                case DiscoFreaks.EffectType.Explosion:
                    EffectType.Text = "Effect Type : Explosion";
                    break;
                case DiscoFreaks.EffectType.BlueInk:
                    EffectType.Text = "Effect Type : Blue Ink";
                    break;
                case DiscoFreaks.EffectType.Gust:
                    EffectType.Text = "Effect Type : Gust";
                    break;
                case DiscoFreaks.EffectType.MagicCircle:
                    EffectType.Text = "Effect Type : Magic Circle";
                    break;
                case DiscoFreaks.EffectType.Stardust:
                    EffectType.Text = "Effect Type : Stardust";
                    break;
            }
            EffectSize.Text = string.Format("Effect Size : {0}", config.EffectSize);
            Luminance.Text = string.Format("Background Luminance : {0}", config.Luminance);
            ShowLaneBorder.Text = "Lane Border : " + (config.ShowLaneBorder ? "ON" : "OFF");
            ShowBeatBorder.Text = "Beat Border : " + (config.ShowBeatBorder ? "ON" : "OFF");

                EffectType.Color =
                EffectSize.Color =
                 Luminance.Color =
            ShowLaneBorder.Color =
            ShowBeatBorder.Color = new Color(255, 255, 255, 63);

            switch(Scene.SettingItem)
            {
                case OptionScene.MenuItem.EffectType:
                    EffectType.Color = new Color(255, 255, Scene.SettingSwitch ? 0 : 255, 255);
                    Scene.ItemDescription.Text = "エフェクトの種類を設定することができます。";
                    break;
                case OptionScene.MenuItem.EffectSize:
                    EffectSize.Color = new Color(255, 255, Scene.SettingSwitch ? 0 : 255, 255);
                    Scene.ItemDescription.Text = "エフェクトの大きさを設定することができます。";
                    break;
                case OptionScene.MenuItem.Luminance:
                    Luminance.Color = new Color(255, 255, Scene.SettingSwitch ? 0 : 255, 255);
                    Scene.ItemDescription.Text = "背景の明るさを設定することができます。";
                    break;
                case OptionScene.MenuItem.LaneBorder:
                    ShowLaneBorder.Color = new Color(255, 255, Scene.SettingSwitch ? 0 : 255, 255);
                    Scene.ItemDescription.Text = "レーンの境目を表示するかを設定\nすることができます。";
                    break;
                case OptionScene.MenuItem.BeatBorder:
                    ShowBeatBorder.Color = new Color(255, 255, Scene.SettingSwitch ? 0 : 255, 255);
                    Scene.ItemDescription.Text = "小節線を表示するかを設定することができます。";
                    break;
            }
            //--------------------------------------------------



            // Controll
            //--------------------------------------------------
            if (Scene.CurrentMode == OptionScene.Mode.VisualSetting)
            {
                if (!Scene.SettingSwitch)
                {
                    if (Input.KeyPush(Keys.Up))
                        Scene.SettingItem = (OptionScene.MenuItem)(Math.Mod((int)Scene.SettingItem - 3, 5) + 2);

                    if (Input.KeyPush(Keys.Down))
                        Scene.SettingItem = (OptionScene.MenuItem)(Math.Mod((int)Scene.SettingItem - 1, 5) + 2);
                }

                else if (Scene.SettingItem == OptionScene.MenuItem.EffectType)
                {
                    if (Input.KeyPush(Keys.Right))
                        config.EffectType =
                            (EffectType)Math.Mod((int)config.EffectType + 1, 6);

                    if (Input.KeyPush(Keys.Left))
                        config.EffectType =
                            (EffectType)Math.Mod((int)config.EffectType - 1, 6);
                }

                else if (Scene.SettingItem == OptionScene.MenuItem.EffectSize)
                {
                    config.EffectSize +=
                        Input.KeyPush(Keys.Right) ? 5 :
                        Input.KeyPush(Keys.Left) ? -5 : 0;

                    config.EffectSize =
                        (int)Math.Clamp(config.EffectSize, 50, 150);
                }

                else if (Scene.SettingItem == OptionScene.MenuItem.Luminance)
                {
                    config.Luminance +=
                        Input.KeyPush(Keys.Right) ? 5 :
                        Input.KeyPush(Keys.Left) ? -5 : 0;

                    config.Luminance =
                        (int)Math.Clamp(config.Luminance, 0, 100);
                }

                else if (Scene.SettingItem == OptionScene.MenuItem.LaneBorder)
                {
                    if (Input.KeyPush(Keys.Right) || Input.KeyPush(Keys.Left))
                        config.ShowLaneBorder = !config.ShowLaneBorder;
                }

                else if (Scene.SettingItem == OptionScene.MenuItem.BeatBorder)
                {
                    if (Input.KeyPush(Keys.Right) || Input.KeyPush(Keys.Left))
                        config.ShowBeatBorder = !config.ShowBeatBorder;
                }
            }
            //--------------------------------------------------

            Scene.Configuration = config;
        }
    }
}
