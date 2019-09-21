﻿using asd;

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
        }
    }
}
