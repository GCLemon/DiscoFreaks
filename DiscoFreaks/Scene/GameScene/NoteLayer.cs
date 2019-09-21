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

        protected override void OnAdded()
        {
            var detail = Scene.Score[Scene.Difficulty];

            // ノートを追加
            while (detail.Notes.Peek().NoteInfo.VisualTiming < 10000)
                AddObject(detail.Notes.Dequeue());

            // ソフランオブジェクトを追加
            while (detail.SofLans.Count != 0)
                AddObject(detail.SofLans.Dequeue());
        }

        protected override void OnUpdated()
        {
            var detail = Scene.Score[Scene.Difficulty];

            while (detail.Notes.Count != 0)
            {
                // ノートを追加
                AddObject(detail.Notes.Dequeue());

                // キューが空になったらループ終了
                if (detail.Notes.Count == 0) break;

                // 必要以上の追加を行わないように
                // 追加を制限する
                var timing = detail.Notes.Peek().NoteInfo.VisualTiming;
                var border = Note.NoteTimer.VisualTime + 10000;
                if (timing > border) break;
            }
        }
    }
}
