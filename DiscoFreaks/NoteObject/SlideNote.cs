using System.Linq;

namespace DiscoFreaks
{
    /// <summary>
    /// スライドノート
    /// </summary>
    public class SlideNote : Note
    {
        public SlideNote(NoteInfo note_info)　: base(note_info)
        {
            AddComponent(
                new SlideNoteComponent("Image/SlideNote.png", note_info.RightLane, note_info.LeftLane),
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
                var judgement = Judge();

                bool is_pressed = JudgeKeys.Any(x => Input.KeyPush(x));
                bool is_judgable = judgement != Judgement.None;
                if (is_pressed && is_judgable)
                {
                    var error = NoteTimer.VisualTime - NoteInfo.VisualTiming;
                    Scene.Result.ChangePointBySlideNote(error > 0 ? judgement : Judgement.Just);
                    if(judgement == Judgement.Near) RemoveComponent("Effect");
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
