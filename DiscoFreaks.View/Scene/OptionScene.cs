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

        private Mode CurrentMode;
        private MenuItem SettingItem;
        private Configuration Configuration;

        // シーン切り替え前に使われていたシーン
        private readonly Scene UsedScene;

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

        public OptionScene(Scene used_scene)
        {
            UsedScene = used_scene;
        }

        protected override void OnRegistered()
        {
            // 背景の設定
            BackLayer.AddPostEffect(new BackGround("Shader/OpenGL/Option.glsl"));

            // シーンのタイトルを設定
            TextLayer.AddObject(SceneTitle);

            // レイヤーの追加
            AddLayer(BackLayer);
            AddLayer(TextLayer);
            AddLayer(UILayer);
        }

        protected override void OnUpdated()
        {
            #region 入力受付部分

            if(Input.KeyPush(Keys.RightShift))
            {
                Engine.ChangeScene(UsedScene);
            }

            #endregion
        }
    }
}
