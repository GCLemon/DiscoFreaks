using System.Collections.Generic;
using asd;

namespace DiscoFreaks
{
    public class NoteLayer : Layer2D
    {
        private readonly Queue<Note> Notes;
        private readonly Queue<Note.SofLan> SofLans;

        public NoteLayer(Detail detail)
        {
            Notes = new Queue<Note>(detail.Notes);
            SofLans = new Queue<Note.SofLan>(detail.SofLans);
        }

        protected override void OnAdded()
        {
            // ノートを追加
            while (Notes.Peek().NoteInfo.VisualTiming < 10000)
                AddObject(Notes.Dequeue());

            // ソフランオブジェクトを追加
            while (SofLans.Count != 0)
                AddObject(SofLans.Dequeue());
        }

        protected override void OnUpdated()
        {
            while (Notes.Count != 0)
            {
                // 必要以上の追加を行わないように
                // 追加を制限する
                var timing = Notes.Peek().NoteInfo.VisualTiming;
                var border = Note.NoteTimer.VisualTime + 10000;
                if (timing > border) break;

                // ノートを追加
                AddObject(Notes.Dequeue());
            }
        }
    }
}
