using System.Collections.Generic;

namespace DiscoFreaks
{
    /// <summary>
    /// 難易度ごとに設ける譜面の詳細部分
    /// </summary>
    public class Detail
    {
        public int Level { get; }
        public int Ofset { get; }
        public double InitialBPM { get; }

        public Queue<Note> Notes { get; }
        public Queue<Note.SofLan> SofLans { get; }

        internal Detail (InitInfo.Detail info)
        {
            info.Notes.Sort((x, y) => (int)(x.NoteInfo.AudioTiming - y.NoteInfo.AudioTiming));
            Level = info.Level;
            Ofset = info.Ofset;
            InitialBPM = info.InitialBPM;
            Notes = new Queue<Note>(info.Notes);
            SofLans = new Queue<Note.SofLan>(info.SofLans);
        }
    }
}
