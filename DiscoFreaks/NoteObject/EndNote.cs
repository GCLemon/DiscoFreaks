namespace DiscoFreaks
{
    /// <summary>
    /// ホールドノートの末端
    /// </summary>
    public class EndNote : Note
    {
        public EndNote(NoteInfo note_info) : base(note_info)
        {
            AddComponent(
                new TapNoteComponent("Image/HoldNote.png", NoteInfo.RightLane, NoteInfo.LeftLane),
                "TapNote"
            );
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (NoteTimer.AudioTime - NoteInfo.AudioTiming > 0) { Dispose(); }
        }
    }
}