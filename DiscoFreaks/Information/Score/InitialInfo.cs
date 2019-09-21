using System.Collections.Generic;

namespace DiscoFreaks
{
    /// <summary>
    /// 譜面オブジェクトの初期化に使う構造体
    /// </summary>
    public struct InitInfo
    {
        public struct Detail
        {
            public int Level;
            public int Ofset;
            public double InitialBPM;

            public List<Note> Notes;
            public List<Note.SofLan> SofLans;
        }

        public string Title;
        public string Subtitle;
        public string SoundPath;
        public string JacketPath;
        public Difficulty Difficulty;
        public Detail DetailInfo;
    }
}
