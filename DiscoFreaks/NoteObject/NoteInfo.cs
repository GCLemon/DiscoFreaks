namespace DiscoFreaks
{
    /// <summary>
    /// ノートの種類
    /// </summary>
    public enum NoteType
    {
        TapNote,
        HoldNote,
        SlideNote
    }

    /// <summary>
    /// ノートの情報を格納する構造体
    /// </summary>
    public struct NoteInfo
    {
        public NoteType Type;

        public int LeftLane;
        public int RightLane;
        public long VisualTiming;
        public long AudioTiming;
        public long VisualLength;
        public long AudioLength;

        public Note Instanciate()
        {
            return Type switch
            {
                NoteType.TapNote => new TapNote(LeftLane, RightLane, VisualTiming, AudioTiming),
                NoteType.HoldNote => new HoldNote(LeftLane, RightLane, VisualTiming, AudioTiming, VisualLength, AudioLength),
                NoteType.SlideNote => new SlideNote(LeftLane, RightLane, VisualTiming, AudioTiming),
                _ => null
            };
        }
    }

    /// <summary>
    /// ソフランの情報を格納する構造体
    /// </summary>
    public struct SofLanInfo
    {
        public long Timing;
        public double AfterSpeed;

        public Note.SofLan Instanciate()
        {
            return new Note.SofLan(Timing, AfterSpeed);
        }
    }
}
