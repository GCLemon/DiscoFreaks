using System.Diagnostics;
using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// ホールドノート
    /// </summary>
    public class HoldNote : Note
    {
        private readonly GeometryObject2D LinkPart;
        private readonly EndNote EndNote;

        private bool IsMoving = true;

        public HoldNote(NoteInfo NoteInfo, NoteInfo EndNoteInfo) : base(NoteInfo)
        {
            DrawingPriority = 1;

            AddComponent(
                new TapNoteComponent
                {
                    TexturePath = "Image/HoldNote.png",
                    RightLane = NoteInfo.RightLane,
                    LeftLane = NoteInfo.LeftLane
                },
                "TapNote"
            );

            LinkPart = new GeometryObject2D
            {
                Shape = new RectangleShape(),
                Color = new Color(230, 219, 116, 127),
                DrawingPriority = 0
            };

            Debug.Assert(NoteInfo.RightLane == EndNoteInfo.RightLane);
            Debug.Assert(NoteInfo.LeftLane == EndNoteInfo.LeftLane);
            EndNote = new EndNote(EndNoteInfo);
            EndNote.DrawingPriority = 1;
        }

        protected override void OnAdded()
        {
            var m = ChildManagementMode.RegistrationToLayer;
            var t = ChildTransformingMode.Nothing;
            AddChild(LinkPart, m, t);
            AddChild(EndNote, m, t);
        }

        protected override void OnUpdate()
        {
            if(IsMoving) base.OnUpdate();

            LinkPart.Position = Position + new Vector2DF(12, 0);
            var size_x = (NoteInfo.RightLane - NoteInfo.LeftLane + 1) * 30 - 24;
            var size_y = EndNote.GetGlobalPosition().Y - LinkPart.GetGlobalPosition().Y;
            var area = new RectF(0, 0, size_x, size_y);
            ((RectangleShape)LinkPart.Shape).DrawingArea = area;

            foreach (var key in JudgeKeys)
                if (Input.KeyPush(key))
                {
                    switch (Judge())
                    {
                        case Judgement.None:
                            break;
                        case Judgement.Just:
                            Position = new Vector2DF(Position.X, 600);
                            IsMoving = false;
                            break;
                        case Judgement.Cool:
                            Position = new Vector2DF(Position.X, 600);
                            IsMoving = false;
                            break;
                        case Judgement.Good:
                            Position = new Vector2DF(Position.X, 600);
                            IsMoving = false;
                            break;
                        case Judgement.Near:
                            Position = new Vector2DF(Position.X, 600);
                            IsMoving = false;
                            break;
                        case Judgement.Miss:
                            Position = new Vector2DF(Position.X, 600);
                            IsMoving = false;
                            break;
                    }
                }

            if (!EndNote.IsAlive)
            {
                Dispose();
            }
        }
    }
}
