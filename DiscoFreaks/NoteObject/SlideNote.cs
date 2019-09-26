using System.Linq;

namespace DiscoFreaks
{
    /// <summary>
    /// スライドノート
    /// </summary>
    public class SlideNote : Note
    {
        public SlideNote(NoteInfo NoteInfo)　: base(NoteInfo)
        {
            AddComponent(
                new SlideNoteComponent
                {
                    TexturePath = "Image/SlideNote.png",
                    RightLane = NoteInfo.RightLane,
                    LeftLane = NoteInfo.LeftLane
                },
                "SlideNote"
            );
            AddComponent(new EffectEmitComponent(), "Effect");
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            // キーがホールドされているかを判定
            bool is_holding = false;
            foreach (var key in JudgeKeys)
                is_holding |= Input.KeyHold(key);

            // ホールドされていた場合
            if (is_holding)
            {
                if (Judge() == Judgement.Just)
                {
                    Scene.Result.ChangePointBySlideNote(Judgement.Just);
                    Dispose();
                }
            }

            // ホールドされていない場合
            else
            {
                bool is_pressed = JudgeKeys.Any(x => Input.KeyPush(x));
                bool is_judgable = Judge() != Judgement.None;
                if (is_pressed && is_judgable)
                {
                    var error = NoteTimer.VisualTime - NoteInfo.VisualTiming;
                    Scene.Result.ChangePointBySlideNote(error > 0 ? Judge() : Judgement.Just);
                    Dispose();
                }
            }

            // Miss判定の場合は強制的に判定する
            if (Judge() == Judgement.Miss)
            {
                Scene.Result.ChangePointByTapNote(Judge());
                RemoveComponent("Effect");
                Dispose();
            }
        }
    }
}
