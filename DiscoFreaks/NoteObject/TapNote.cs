using System.Linq;

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

            // キーの押下によって判定
            if (!Layer.Objects
                .Where(x => x is Note)
                .Any(x => IsOverlapped((Note)x))
            )
            {
                bool is_pressed = JudgeKeys.Any(x => Input.KeyPush(x));
                bool is_judgable = Judge() != Judgement.None;
                if (is_pressed && is_judgable)
                {
                        Scene.Result.ChangePointByTapNote(Judge());
                        Dispose();
                }
            }

            // Miss判定の場合は強制的に判定する
            if(Judge() == Judgement.Miss)
            {
                Scene.Result.ChangePointByTapNote(Judge());
                RemoveComponent("Effect");
                Dispose();
            }
        }
    }
}
