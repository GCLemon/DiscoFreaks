using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// タップノート
    /// </summary>
    public class TapNote : Note
    {
        public TapNote(NoteInfo info) : base(info)
        {
            TextureObject2D center = new TextureObject2D();
            TextureObject2D right = new TextureObject2D();

            var texture = Graphics.CreateTexture("Image/TapNote.png");
            var size = info.RightLane - info.LeftLane;

            // Left
            //--------------------------------------------------
            Texture = texture;
            Src = new RectF(0, 0, 12, 16);
            CenterPosition = new Vector2DF(0, 8);
            //--------------------------------------------------

            // Center
            //--------------------------------------------------
            center.Texture = texture;
            center.Src = new RectF(12, 0, 16, 16);
            center.Scale = new Vector2DF((size * 30 + 6) / 16.0f, 1);
            center.CenterPosition = new Vector2DF(0, 8);
            center.Position = new Vector2DF(12, 0);
            //--------------------------------------------------

            // Right
            //--------------------------------------------------
            right.Texture = texture;
            right.Src = new RectF(28, 0, 12, 16);
            right.CenterPosition = new Vector2DF(12, 8);
            right.Position = new Vector2DF(size * 30 + 30, 0);
            //--------------------------------------------------

            var m = ChildManagementMode.RegistrationToLayer;
            var t = ChildTransformingMode.Position;
            AddChild(center, m, t);
            AddChild(right, m, t);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            foreach(var key in CorrespondingKeys)
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

        protected override void OnDispose()
        {
            foreach (var c in Children) c.Dispose();
        }
    }
}
