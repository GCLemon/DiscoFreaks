using System.Linq;

namespace DiscoFreaks
{
    /// <summary>
    /// タップノート
    /// </summary>
    public class TapNote : Note
    {
        public TapNote(int left_lane, int right_lane, long visual_timing, long audio_timing)
            :　base(left_lane, right_lane, visual_timing, audio_timing)
        {
            AddComponent(
                new TapNoteComponent("Image/TapNote.png", RightLane, LeftLane),
                "TapNote"
            );
            AddComponent(new EffectEmitComponent(), "Effect");
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            // キーの押下によって判定
            if (!Layer.Objects.Where(x => x is Note).Any(x => IsOverlapped((Note)x)))
            {
                var judgement = Judge();

                bool is_pressed = JudgeKeys.Any(x => Input.KeyPush(x));
                bool is_judgable = judgement != Judgement.None;
                if (is_pressed && is_judgable)
                {
                    Scene.Result.ChangePointByTapNote(judgement);
                    if(judgement == Judgement.Near) RemoveComponent("Effect");
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
