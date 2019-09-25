using asd;

namespace DiscoFreaks
{
    public class ResultLayer : Layer2D
    {
        // レイヤーが登録されているシーン
        private new ResultScene Scene
        {
            get => (ResultScene)base.Scene;
        }

        public ResultLayer()
        {
            // コンポーネントを作成・追加
            AddComponent(new UIComponent(), "UI");
        }
    }
}
