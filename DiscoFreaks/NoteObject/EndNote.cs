using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// ホールドノートの末端
    /// </summary>
    public class EndNote : Note
    {
        public EndNote(NoteInfo NoteInfo) : base(NoteInfo)
        {
            AddComponent(
                new TapNoteComponent
                {
                    TexturePath = "Image/HoldNote.png",
                    RightLane = NoteInfo.RightLane,
                    LeftLane = NoteInfo.LeftLane
                },
                "TapNote"
            );
            AddComponent(new EffectEmitComponent(), "Effect");
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (Judge() == Judgement.Just) { Dispose(); }
        }
    }
}