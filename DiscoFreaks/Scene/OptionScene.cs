using System.IO;
using System.Linq;
using asd;

namespace DiscoFreaks
{
    public class OptionScene : Scene
    {
        // Option シーンでのモード
        private enum Mode
        {
            PlaySetting,
            VisualSetting,
            AudioSetting
        }

        // 設定項目
        private enum MenuItem
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

        private bool SettingSwitch;
        private Mode CurrentMode;
        private MenuItem SettingItem;
        private Configuration Configuration;

        // シーン切り替え前に使われていたシーン
        private readonly SelectScene UsedScene;

        // レイヤー
        private readonly Layer2D BackLayer = new Layer2D();
        private readonly Layer2D TextLayer = new Layer2D();
        private readonly Layer2D UILayer = new Layer2D();

        // シーンのタイトル
        private readonly HeadUpDaisy SceneTitle =
            new HeadUpDaisy(72, 4, new Vector2DF(0.5f, 0))
            {
                Text = "Play Option",
                Position = new Vector2DF(480, 10)
            };

        // メニュー項目の説明
        private readonly Makinas ItemDescription =
            new Makinas(32, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(480, 550) };

        // 左右にあるメニュー項目
        private readonly HeadUpDaisy LeftMode =
            new HeadUpDaisy(24, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(100, 40) };
        private readonly HeadUpDaisy RightMode =
            new HeadUpDaisy(24, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(860, 40) };

        #region Play Setting

        private readonly GridGazer TextsPlay =
            new GridGazer(48, 4, new Vector2DF(0.5f, 0))
            { Text = "Play Configuration"};

        private readonly GridGazer HighSpeed =
            new GridGazer(36, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(0, 100) };

        private readonly GridGazer Ofset =
            new GridGazer(36, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(0, 150) };

        #endregion

        #region Visual Setting

        private readonly GridGazer TextsVisual =
            new GridGazer(48, 4, new Vector2DF(0.5f, 0))
            { Text = "Visual Configuration" };

        private readonly GridGazer EffectType =
            new GridGazer(36, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(0, 100) };

        private readonly GridGazer EffectSize =
            new GridGazer(36, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(0, 150) };

        private readonly GridGazer Luminance =
            new GridGazer(36, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(0, 200) };

        private readonly GridGazer ShowLaneBorder =
            new GridGazer(36, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(0, 250) };

        private readonly GridGazer ShowBeatBorder =
            new GridGazer(36, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(0, 300) };

        #endregion

        #region Audio Setting

        private readonly GridGazer TextsAudio =
            new GridGazer(48, 4, new Vector2DF(0.5f, 0))
            { Text = "Audio Configuration" };

        private readonly GridGazer BGMVolume =
            new GridGazer(36, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(0, 100) };

        private readonly GridGazer SEVolume =
            new GridGazer(36, 4, new Vector2DF(0.5f, 0))
            { Position = new Vector2DF(0, 150) };

        #endregion

        private int Phase;

        public OptionScene(SelectScene used_scene)
        {
            // シーンの情報を保存
            UsedScene = used_scene;

            if(System.IO.File.Exists("PlaySetting.config"))
            {
                ReadConfig();
            }
            else
            {
                InitConfig();
                WriteConfig();
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

            var m = ChildManagementMode.RegistrationToLayer;
            var t = ChildTransformingMode.Position;

            TextsPlay.AddChild(HighSpeed, m, t);
            TextsPlay.AddChild(Ofset, m, t);
            TextsVisual.AddChild(EffectType, m, t);
            TextsVisual.AddChild(EffectSize, m, t);
            TextsVisual.AddChild(Luminance, m, t);
            TextsVisual.AddChild(ShowLaneBorder, m, t);
            TextsVisual.AddChild(ShowBeatBorder, m, t);
            TextsAudio.AddChild(BGMVolume, m, t);
            TextsAudio.AddChild(SEVolume, m, t);

            UILayer.AddObject(TextsPlay);
            UILayer.AddObject(TextsVisual);
            UILayer.AddObject(TextsAudio);

            // レイヤーの追加
            AddLayer(BackLayer);
            AddLayer(TextLayer);
            AddLayer(UILayer);
        }

        protected override void OnUpdated()
        {
            // Phase の値によってオブジェクトの描画位置を変更する
            if (Phase != 0)
            {
                Input.AcceptInput = false;
                foreach (var obj in UILayer.Objects.Where(x => x.Parent == null))
                    obj.Position += new Vector2DF(-8 * Phase, 0);
                Phase += Phase > 0 ? -1 : 1;
            }
            else
            {
                Input.AcceptInput = true;
                switch (CurrentMode)
                {
                    case Mode.PlaySetting:
                        TextsPlay.Position = new Vector2DF(480, 150);
                        TextsVisual.Position = new Vector2DF(1440, 150);
                        TextsAudio.Position = new Vector2DF(-480, 150);
                        LeftMode.Text = "← Audio\nSetting";
                        RightMode.Text = "Visual →\nSetting";
                        break;
                    case Mode.VisualSetting:
                        TextsPlay.Position = new Vector2DF(-480, 150);
                        TextsVisual.Position = new Vector2DF(480, 150);
                        TextsAudio.Position = new Vector2DF(1440, 150);
                        LeftMode.Text = "← Play\nSetting";
                        RightMode.Text = "Audio →\nSetting";
                        break;
                    case Mode.AudioSetting:
                        TextsPlay.Position = new Vector2DF(1440, 150);
                        TextsVisual.Position = new Vector2DF(-480, 150);
                        TextsAudio.Position = new Vector2DF(480, 150);
                        LeftMode.Text = "← Visual\nSetting";
                        RightMode.Text = "Play →\nSetting";
                        break;
                }
            }

            Draw();
            Controll();
        }

        private void Draw()
        {
            HighSpeed.Text = string.Format("High Speed : x{0}", Configuration.HighSpeed.ToString("0.0"));
            Ofset.Text = string.Format("Ofset : {0}ms", Configuration.Ofset.ToString("+0;-0;±0"));
            EffectSize.Text = string.Format("Effect Size : {0}", Configuration.EffectSize);
            Luminance.Text = string.Format("Background Luminance : {0}", Configuration.Luminance);
            ShowLaneBorder.Text = "Lane Border : " + (Configuration.ShowLaneBorder ? "ON" : "OFF");
            ShowBeatBorder.Text = "Beat Border : " + (Configuration.ShowBeatBorder ? "ON" : "OFF");
            BGMVolume.Text = string.Format("BGM Volume : {0}", Configuration.BGMVolume);
            SEVolume.Text = string.Format("SE Volume : {0}", Configuration.SEVolume);
            switch (Configuration.EffectType)
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

            HighSpeed.Color =
            Ofset.Color =
            EffectType.Color =
            EffectSize.Color =
            Luminance.Color =
            ShowLaneBorder.Color =
            ShowBeatBorder.Color =
            BGMVolume.Color =
            SEVolume.Color =
            new Color(255, 255, 255, 63);

            switch (SettingItem)
            {
                case MenuItem.HighSpeed:
                    HighSpeed.Color = new Color(255, 255, SettingSwitch ? 0 : 255, 255);
                    ItemDescription.Text = "ノーツが落下する速度を設定することができます。";
                    break;
                case MenuItem.Ofset:
                    Ofset.Color = new Color(255, 255, SettingSwitch ? 0 : 255, 255);
                    ItemDescription.Text = "判定のタイミングを設定することができます。\nタイミングが早いと思った場合はプラス、\n遅いと思った場合はマイナスに設定してください。";
                    break;
                case MenuItem.EffectType:
                    EffectType.Color = new Color(255, 255, SettingSwitch ? 0 : 255, 255);
                    ItemDescription.Text = "エフェクトの種類を設定することができます。";
                    break;
                case MenuItem.EffectSize:
                    EffectSize.Color = new Color(255, 255, SettingSwitch ? 0 : 255, 255);
                    ItemDescription.Text = "エフェクトの大きさを設定することができます。";
                    break;
                case MenuItem.Luminance:
                    Luminance.Color = new Color(255, 255, SettingSwitch ? 0 : 255, 255);
                    ItemDescription.Text = "背景の明るさを設定することができます。";
                    break;
                case MenuItem.LaneBorder:
                    ShowLaneBorder.Color = new Color(255, 255, SettingSwitch ? 0 : 255, 255);
                    ItemDescription.Text = "レーンの境目を表示するかを設定\nすることができます。";
                    break;
                case MenuItem.BeatBorder:
                    ShowBeatBorder.Color = new Color(255, 255, SettingSwitch ? 0 : 255, 255);
                    ItemDescription.Text = "小節線を表示するかを設定することができます。";
                    break;
                case MenuItem.BGMVolume:
                    BGMVolume.Color = new Color(255, 255, SettingSwitch ? 0 : 255, 255);
                    ItemDescription.Text = "BGMの音量を設定することができます。";
                    break;
                case MenuItem.SEVolume:
                    SEVolume.Color = new Color(255, 255, SettingSwitch ? 0 : 255, 255);
                    ItemDescription.Text = "効果音の音量を設定することができます。";
                    break;
            }
        }

        private void Controll()
        {
            switch (CurrentMode)
            {
                case Mode.PlaySetting:
                    PlaySetting();
                    break;
                case Mode.VisualSetting:
                    VisualSetting();
                    break;
                case Mode.AudioSetting:
                    AudioSetting();
                    break;
            }

            if (Input.KeyPush(Keys.Enter) || Input.KeyPush(Keys.Backspace) && SettingSwitch)
                SettingSwitch = !SettingSwitch;

            else if (Input.KeyPush(Keys.Backspace) && !SettingSwitch)
            {
                // ファイルを読み込み
                using (var stream = new FileStream(
                    "PlaySetting.config",
                    FileMode.Open
                ))
                {
                    var writer = new BinaryWriter(stream);

                    // Play Configuration
                    //--------------------------------------------------
                    writer.Write(Configuration.HighSpeed);
                    writer.Write(Configuration.Ofset);
                    //--------------------------------------------------

                    // Visual Configuration
                    //--------------------------------------------------
                    writer.Write((int)Configuration.EffectType);
                    writer.Write(Configuration.EffectSize);
                    writer.Write(Configuration.Luminance);
                    writer.Write(Configuration.ShowLaneBorder);
                    writer.Write(Configuration.ShowBeatBorder);
                    //--------------------------------------------------

                    // Audio Configuration
                    //--------------------------------------------------
                    writer.Write(Configuration.BGMVolume);
                    writer.Write(Configuration.SEVolume);
                    //--------------------------------------------------

                    writer.Close();
                    writer.Dispose();
                }
                Engine.ChangeSceneWithTransition(UsedScene, new TransitionFade(1, 1));
            }
        }

        private void PlaySetting()
        {
            if(!SettingSwitch)
            {
                if (Input.KeyPush(Keys.Right))
                {
                    CurrentMode = Mode.VisualSetting;
                    SettingItem = MenuItem.EffectType;
                    Phase = 15;
                }

                if (Input.KeyPush(Keys.Left))
                {
                    CurrentMode = Mode.AudioSetting;
                    SettingItem = MenuItem.BGMVolume;
                    Phase = -15;
                }

                if (Input.KeyPush(Keys.Up))
                    SettingItem = (MenuItem)Math.Mod((int)SettingItem - 1, 2);

                if (Input.KeyPush(Keys.Down))
                    SettingItem = (MenuItem)Math.Mod((int)SettingItem + 1, 2);

            }

            else if (SettingItem == MenuItem.HighSpeed)
            {
                Configuration.HighSpeed += 
                    Input.KeyPush(Keys.Right) ? 0.5 :
                    Input.KeyPush(Keys.Left) ? -0.5 : 0;

                Configuration.HighSpeed =
                    Math.Clamp(Configuration.HighSpeed, 1, 20);
            }

            else if (SettingItem == MenuItem.Ofset)
            {
                Configuration.Ofset +=
                    Input.KeyPush(Keys.Right) ? 10 :
                    Input.KeyPush(Keys.Left) ? -10 : 0;
            }
        }

        private void VisualSetting()
        {

            if (!SettingSwitch)
            {
                if (Input.KeyPush(Keys.Right))
                {
                    CurrentMode = Mode.AudioSetting;
                    SettingItem = MenuItem.BGMVolume;
                    Phase = 15;
                }

                if (Input.KeyPush(Keys.Left))
                {
                    CurrentMode = Mode.PlaySetting;
                    SettingItem = MenuItem.HighSpeed;
                    Phase = -15;
                }

                if (Input.KeyPush(Keys.Up))
                    SettingItem = (MenuItem)(Math.Mod((int)SettingItem - 3, 5) + 2);

                if (Input.KeyPush(Keys.Down))
                    SettingItem = (MenuItem)(Math.Mod((int)SettingItem - 1, 5) + 2);
            }

            else if (SettingItem == MenuItem.EffectType)
            {
                if (Input.KeyPush(Keys.Right))
                    Configuration.EffectType =
                        (EffectType)Math.Mod((int)Configuration.EffectType + 1, 6);

                if (Input.KeyPush(Keys.Left))
                    Configuration.EffectType =
                        (EffectType)Math.Mod((int)Configuration.EffectType - 1, 6);
            }

            else if (SettingItem == MenuItem.EffectSize)
            {
                Configuration.EffectSize +=
                    Input.KeyPush(Keys.Right) ? 5 :
                    Input.KeyPush(Keys.Left) ? -5 : 0;

                Configuration.EffectSize =
                    (int)Math.Clamp(Configuration.EffectSize, 50, 150);
            }

            else if (SettingItem == MenuItem.Luminance)
            {
                Configuration.Luminance +=
                    Input.KeyPush(Keys.Right) ? 5 :
                    Input.KeyPush(Keys.Left) ? -5 : 0;

                Configuration.Luminance =
                    (int)Math.Clamp(Configuration.Luminance, 0, 100);
            }

            else if (SettingItem == MenuItem.LaneBorder)
            {
                if (Input.KeyPush(Keys.Right) || Input.KeyPush(Keys.Left))
                    Configuration.ShowLaneBorder = !Configuration.ShowLaneBorder;
            }

            else if (SettingItem == MenuItem.BeatBorder)
            {
                if (Input.KeyPush(Keys.Right) || Input.KeyPush(Keys.Left))
                    Configuration.ShowBeatBorder = !Configuration.ShowBeatBorder;
            }
        }

        private void AudioSetting()
        {

            if (!SettingSwitch)
            {
                if (Input.KeyPush(Keys.Right))
                {
                    CurrentMode = Mode.PlaySetting;
                    SettingItem = MenuItem.HighSpeed;
                    Phase = 15;
                }

                if (Input.KeyPush(Keys.Left))
                {
                    CurrentMode = Mode.VisualSetting;
                    SettingItem = MenuItem.EffectType;
                    Phase = -15;
                }

                if (Input.KeyPush(Keys.Up))
                    SettingItem = (MenuItem)(Math.Mod((int)SettingItem - 8, 2) + 7);

                if (Input.KeyPush(Keys.Down))
                    SettingItem = (MenuItem)(Math.Mod((int)SettingItem - 6, 2) + 7);
            }

            else if (SettingItem == MenuItem.BGMVolume)
            {
                Configuration.BGMVolume +=
                    Input.KeyPush(Keys.Right) ? 5 :
                    Input.KeyPush(Keys.Left) ? -5 : 0;

                Configuration.BGMVolume =
                    (int)Math.Clamp(Configuration.BGMVolume, 0, 100);
            }

            else if (SettingItem == MenuItem.SEVolume)
            {
                Configuration.SEVolume +=
                    Input.KeyPush(Keys.Right) ? 5 :
                    Input.KeyPush(Keys.Left) ? -5 : 0;

                Configuration.SEVolume =
                    (int)Math.Clamp(Configuration.SEVolume, 0, 100);
            }
        }

        private void InitConfig()
        {
            // Conriguration を初期化
            Configuration = new Configuration
            {
                // Play Configuration
                //--------------------------------------------------
                HighSpeed = 1,
                Ofset = 0,
                //--------------------------------------------------

                // Visual Configuration
                //--------------------------------------------------
                EffectType = 0,
                EffectSize = 100,
                Luminance = 100,
                ShowLaneBorder = true,
                ShowBeatBorder = true,
                //--------------------------------------------------

                // Audio Configuration
                //--------------------------------------------------
                BGMVolume = 100,
                SEVolume = 100
                //--------------------------------------------------
            };
        }

        private void ReadConfig()
        {
            // ファイルを読み込み
            using (var stream = new FileStream(
                "PlaySetting.config",
                FileMode.Open
            ))
            {
                // バイナリ読み込み用オブジェクト
                var reader = new BinaryReader(stream);

                // ファイル読み込み
                Configuration = new Configuration
                {
                    // Play Configuration
                    //--------------------------------------------------
                    HighSpeed = reader.ReadDouble(),
                    Ofset = reader.ReadInt32(),
                    //--------------------------------------------------

                    // Visual Configuration
                    //--------------------------------------------------
                    EffectType = (EffectType)reader.ReadInt32(),
                    EffectSize = reader.ReadInt32(),
                    Luminance = reader.ReadInt32(),
                    ShowLaneBorder = reader.ReadBoolean(),
                    ShowBeatBorder = reader.ReadBoolean(),
                    //--------------------------------------------------

                    // Audio Configuration
                    //--------------------------------------------------
                    BGMVolume = reader.ReadInt32(),
                    SEVolume = reader.ReadInt32()
                    //--------------------------------------------------
                };

                // ファイルを閉じる
                reader.Close();
                reader.Dispose();
            }
        }

        private void WriteConfig()
        {
            // ファイルを読み込み
            using (var stream = new FileStream(
                "PlaySetting.config",
                FileMode.Create
            ))
            {
                var writer = new BinaryWriter(stream);

                // Play Configuration
                //--------------------------------------------------
                writer.Write(Configuration.HighSpeed);
                writer.Write(Configuration.Ofset);
                //--------------------------------------------------

                // Visual Configuration
                //--------------------------------------------------
                writer.Write((int)Configuration.EffectType);
                writer.Write(Configuration.EffectSize);
                writer.Write(Configuration.Luminance);
                writer.Write(Configuration.ShowLaneBorder);
                writer.Write(Configuration.ShowBeatBorder);
                //--------------------------------------------------

                // Audio Configuration
                //--------------------------------------------------
                writer.Write(Configuration.BGMVolume);
                writer.Write(Configuration.SEVolume);
                //--------------------------------------------------

                writer.Close();
                writer.Dispose();
            }
        }
    }
}
