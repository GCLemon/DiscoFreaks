using System.Linq;
using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// 背景を追加するためのコンポーネント
    /// </summary>
    public class BackgroundComponent : SceneComponent
    {
        private string ShaderPath;

        public BackgroundComponent(string path)
        {
            ShaderPath = path;
        }

        protected override void OnSceneRegistered()
        {
            var back = new BackGround(ShaderPath);
            var back_layer = new Layer2D { DrawingPriority = 0};
            back_layer.AddPostEffect(back);
            Owner.AddLayer(back_layer);
        }
    }

    /// <summary>
    /// 入力の受付の是非を管理するコンポーネント
    /// </summary>
    public class InputManageComponent : SceneComponent
    {
        protected override void OnStartSceneUpdating() => Input.AcceptInput = true;

        protected override void OnSceneTransitionBegin() => Input.AcceptInput = false;
    }

    /// <summary>
    /// シーンのタイトルと、その他レイヤー全体の操作を
    /// 必要としないUIを追加するコンポーネント
    /// </summary>
    public class FixedUIComponent : SceneComponent
    {
        private readonly Layer2D UILayer;

        public FixedUIComponent(string scene_title)
        {
            UILayer = new Layer2D { DrawingPriority = 2 };
            UILayer.AddObject(new HeadUpDaisy(72, 4, new Vector2DF(0.5f, 0))
            {
                Text = scene_title,
                Position = new Vector2DF(480, 10)
            });
        }

        protected override void OnSceneRegistered()
        {
            if(!Owner.Layers.Contains(UILayer))
                Owner.AddLayer(UILayer);
        }

        public void AddObject(Object2D obj) => UILayer.AddObject(obj);
    }
}
