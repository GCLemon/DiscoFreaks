namespace DiscoFreaks
{
    /// <summary>
    /// ホールドノートの末端
    /// </summary>
    public class EndNote : Note
    {
        public EndNote(int left_lane, int right_lane, long visual_timing, long audio_timing)
            : base(left_lane, right_lane, visual_timing, audio_timing)
        {
            AddComponent(
                new TapNoteComponent("Image/HoldNote.png", RightLane, LeftLane),
                "TapNote"
            );
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (NoteTimer.AudioTime - AudioTiming > 0) { Dispose(); }
        }
    }
}