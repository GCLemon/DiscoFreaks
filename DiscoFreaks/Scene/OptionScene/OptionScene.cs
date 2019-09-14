using System.Linq;
using asd;

namespace DiscoFreaks
{
    public class OptionScene : Scene
    {
        // Option シーンでのモード
        public enum Mode
        {
            PlaySetting,
            VisualSetting,
            AudioSetting
        }

        // 設定項目
        public enum MenuItem
        {
            HighSpeed,
            Ofset,
            EffectType,
            EffectSize,
            Luminance,
            LaneBorder,
            BeatBorder,
            BGMVolume,
            SEVolume
        }

        public bool SettingSwitch;
        public Mode CurrentMode;
        public MenuItem SettingItem;
        public Configuration Configuration;

        // シーン切り替え前に使われていたシーン
        private readonly SelectScene UsedScene;

        // レイヤー
        private readonly Layer2D BackLayer = new Layer2D();
        private readonly Layer2D TextLayer = new Layer2D();
        private readonly PlaySettingLayer PSetLayer = new PlaySettingLayer();
        private readonly VisualSettingLayer VSetLayer = new VisualSettingLayer();
        private readonly AudioSettingLayer ASetLayer = new AudioSettingLayer();

        // シーンのタイトル
        private readonly HeadUpDaisy SceneTitle =
            new HeadUpDaisy(72, 4, new Vector2DF(0.5f, 0))
            {
                Text = "Play Option",
                Position = new Vector2DF(480, 10)
            };

        // メニュー項目の説明
        public readonly Makinas ItemDescription =
            new Makinas(32, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(480, 550) };

        // 左右にあるメニュー項目
        private readonly HeadUpDaisy LeftMode =
            new HeadUpDaisy(24, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(100, 40) };
        private readonly HeadUpDaisy RightMode =
            new HeadUpDaisy(24, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(860, 40) };


        public OptionScene(SelectScene used_scene)
        {
            // シーンの情報を保存
            UsedScene = used_scene;

            if(System.IO.File.Exists("PlaySetting.config"))
            {
                Configuration = Configuration.Load();
            }
            else
            {
                Configuration = Configuration.Init();
                Configuration.Save(Configuration);
            }
        }

        protected override void OnRegistered()
        {
            // 背景の設定
            BackLayer.AddPostEffect(new BackGround("Shader/OpenGL/Option.glsl"));

            // シーンのタイトルを設定
            TextLayer.AddObject(SceneTitle);
            TextLayer.AddObject(ItemDescription);
            TextLayer.AddObject(LeftMode);
            TextLayer.AddObject(RightMode);

            // レイヤーの追加
            AddLayer(BackLayer);
            AddLayer(TextLayer);
            AddLayer(PSetLayer);
            AddLayer(VSetLayer);
            AddLayer(ASetLayer);
        }

        protected override void OnUpdated()
        {
            Draw();
            Controll();
        }

        private void Draw()
        {
            switch (CurrentMode)
            {
                case Mode.PlaySetting:
                    LeftMode.Text = "← Audio\nSetting";
                    RightMode.Text = "Visual →\nSetting";
                    break;
                case Mode.VisualSetting:
                    LeftMode.Text = "← Play\nSetting";
                    RightMode.Text = "Audio →\nSetting";
                    break;
                case Mode.AudioSetting:
                    LeftMode.Text = "← Visual\nSetting";
                    RightMode.Text = "Play →\nSetting";
                    break;
            }
        }

        private void Controll()
        {
            if (!SettingSwitch)
            {
                if (Input.KeyPush(Keys.Right))
                {
                    switch (CurrentMode)
                    {
                        case Mode.PlaySetting:
                            CurrentMode = Mode.VisualSetting;
                            SettingItem = MenuItem.EffectType;
                            break;
                        case Mode.VisualSetting:
                            CurrentMode = Mode.AudioSetting;
                            SettingItem = MenuItem.BGMVolume;
                            break;
                        case Mode.AudioSetting:
                            CurrentMode = Mode.PlaySetting;
                            SettingItem = MenuItem.HighSpeed;
                            break;
                    }
                    PSetLayer.UIComponent.MoveLeft();
                    VSetLayer.UIComponent.MoveLeft();
                    ASetLayer.UIComponent.MoveLeft();
                }

                if (Input.KeyPush(Keys.Left))
                {
                    switch (CurrentMode)
                    {
                        case Mode.PlaySetting:
                            CurrentMode = Mode.AudioSetting;
                            SettingItem = MenuItem.BGMVolume;
                            break;
                        case Mode.VisualSetting:
                            CurrentMode = Mode.PlaySetting;
                            SettingItem = MenuItem.HighSpeed;
                            break;
                        case Mode.AudioSetting:
                            CurrentMode = Mode.VisualSetting;
                            SettingItem = MenuItem.EffectType;
                            break;
                    }
                    PSetLayer.UIComponent.MoveRight();
                    VSetLayer.UIComponent.MoveRight();
                    ASetLayer.UIComponent.MoveRight();
                }
            }

            if (Input.KeyPush(Keys.Enter))
                SettingSwitch = !SettingSwitch;

            if (Input.KeyPush(Keys.Backspace) && !SettingSwitch)
            {
                if(SettingSwitch)
                {
                    SettingSwitch = !SettingSwitch;
                }
                else
                {
                    Configuration.Save(Configuration);
                    Engine.ChangeSceneWithTransition(UsedScene, new TransitionFade(1, 1));
                }
            }
        }
    }
}
