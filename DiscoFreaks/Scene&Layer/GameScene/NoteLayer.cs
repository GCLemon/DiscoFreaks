using System.Collections.Generic;
using asd;

namespace DiscoFreaks
{
    public class NoteLayer : Layer2D
    {
        private readonly Queue<Note> Notes;
        private readonly Queue<Note.SofLan> SofLans;

        public NoteLayer(Score.Detail detail)
        {
            Notes = new Queue<Note>();
            foreach (var note_info in detail.Notes)
                Notes.Enqueue(note_info.Instanciate());

            SofLans = new Queue<Note.SofLan>();
            foreach (var note_info in detail.SofLans)
                SofLans.Enqueue(note_info.Instanciate());
        }

        protected override void OnAdded()
        {
            // ノートを追加
            while (Notes.Peek().VisualTiming < 10000)
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
                var timing = Notes.Peek().VisualTiming;
                var border = Note.NoteTimer.VisualTime + 10000;
                if (timing > border) break;

                // ノートを追加
                AddObject(Notes.Dequeue());
            }
        }
    }
}
