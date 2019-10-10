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

        public List<NoteInfo> Notes { get; }
        public List<SofLanInfo> SofLans { get; }

        internal Detail (InitInfo.Detail info)
        {
            info.Notes.Sort((x, y) => (int)(x.AudioTiming - y.AudioTiming));
            Level = info.Level;
            Ofset = info.Ofset;
            InitialBPM = info.InitialBPM;
            Notes = new List<NoteInfo>(info.Notes);
            SofLans = new List<SofLanInfo>(info.SofLans);
        }
    }
}
