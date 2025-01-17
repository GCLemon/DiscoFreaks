﻿using asd;

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
            AutoMode,
            EffectType,
            EffectSize,
            Luminance,
            BGMVolume,
            SEVolume
        }

        public bool SettingSwitch;
        public Mode CurrentMode;
        public MenuItem SettingItem;
        public Configuration Configuration;

        // シーン切り替え前に選択されていた譜面
        private readonly Score SelectedScore;
        private readonly int PlayingSoundID;

        // レイヤー
        private readonly PlaySettingLayer PSetLayer;
        private readonly VisualSettingLayer VSetLayer;
        private readonly AudioSettingLayer ASetLayer;

        // メニュー項目の説明
        public readonly Makinas ItemDescription;

        // 左右にあるメニュー項目
        private readonly HeadUpDaisy LeftMode;
        private readonly HeadUpDaisy RightMode;


        public OptionScene(Score selected_score, int sound_id)
        {
            // コンポーネントを追加
            AddComponent(new BackgroundComponent("Shader/OpenGL/Option.glsl"), "Background");
            AddComponent(new InputManageComponent(), "Input");
            AddComponent(new FixedUIComponent("Play Option"), "FixedUI");

            // 譜面の情報を保存
            SelectedScore = selected_score;

            PSetLayer = new PlaySettingLayer { DrawingPriority = 1 };
            VSetLayer = new VisualSettingLayer { DrawingPriority = 1 };
            ASetLayer = new AudioSettingLayer { DrawingPriority = 1 };

            var center = new Vector2DF(0.5f, 0.0f);
            ItemDescription = new Makinas(32, 4, center) { Position = new Vector2DF(480, 550) };
            LeftMode = new HeadUpDaisy(24, 4, center) { Position = new Vector2DF(100, 40) };
            RightMode = new HeadUpDaisy(24, 4, center) { Position = new Vector2DF(860, 40) };

            Configuration = Configuration.Load();

            // BGMの情報を保存
            PlayingSoundID = sound_id;
        }

        protected override void OnRegistered()
        {
            // シーンのタイトルを設定
            var fixed_ui = (FixedUIComponent)GetComponent("FixedUI");
            fixed_ui.AddObject(ItemDescription);
            fixed_ui.AddObject(LeftMode);
            fixed_ui.AddObject(RightMode);

            // レイヤーの追加
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
                    Engine.ChangeSceneWithTransition(new SelectScene(SelectedScore, PlayingSoundID), new TransitionFade(1, 1));
                }
            }
        }
    }
}
