using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// タップノート
    /// </summary>
    public class TapNote : Note
    {
        public TapNote(NoteInfo NoteInfo) : base(NoteInfo)
        {
            AddComponent(
                new TapNoteComponent
                {
                    TexturePath = "Image/TapNote.png",
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

            foreach(var key in JudgeKeys)
                if(Input.KeyPush(key))
                {
                    switch (Judge())
                    {
                        case Judgement.None:
                            break;
                        case Judgement.Just:
                            Dispose();
                            break;
                        case Judgement.Cool:
                            Dispose();
                            break;
                        case Judgement.Good:
                            Dispose();
                            break;
                        case Judgement.Near:
                            Dispose();
                            break;
                        case Judgement.Miss:
                            Dispose();
                            break;
                    }
                }
        }
    }
}
