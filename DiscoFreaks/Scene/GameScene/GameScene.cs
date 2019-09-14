using asd;

namespace DiscoFreaks
{
    public class GameScene : Scene
    {
        /// <summary>
        /// ゲームの情報
        /// </summary>
        private readonly GameInfo GameInfo;

        // レイヤー
        private readonly Layer2D BackLayer = new Layer2D();
        private readonly Layer2D LaneLayer = new Layer2D();
        private readonly Layer2D NoteLayer = new Layer2D();
        private readonly Layer2D UILayer = new Layer2D();

        public GameScene(GameInfo GameInfo) => this.GameInfo = GameInfo;

        protected override void OnRegistered()
        {
            var back = new GameBackGround("Shader/OpenGL/Game.glsl");
            back.Luminance = GameInfo.Configuration.Luminance;
            BackLayer.AddPostEffect(back);

            AddLayer(BackLayer);
            AddLayer(LaneLayer);
            AddLayer(NoteLayer);
            AddLayer(UILayer);
        }
    }
}
