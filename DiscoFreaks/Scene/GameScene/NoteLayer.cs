using asd;

namespace DiscoFreaks
{
    public class NoteLayer : Layer2D
    {
        // レイヤーが登録されているシーン
        private new GameScene Scene
        {
            get => (GameScene)base.Scene;
        }

        public NoteLayer()
        {
        }
    }
}
