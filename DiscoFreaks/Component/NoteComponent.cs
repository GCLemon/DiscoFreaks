using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// ボタン押下に反応するノートに紐付けるコンポーネント
    /// </summary>
    public class TapNoteComponent : Object2DComponent
    {
        private new TextureObject2D Owner
        {
            get => (TextureObject2D)base.Owner;
        }

        public string TexturePath;
        public int RightLane;
        public int LeftLane;

        TextureObject2D Center;
        TextureObject2D Right;

        protected override void OnObjectAdded()
        {
            Center = new TextureObject2D();
            Right = new TextureObject2D();

            var texture = Graphics.CreateTexture(TexturePath);
            var size = RightLane - LeftLane;

            // Left
            //--------------------------------------------------
            Owner.Texture = texture;
            Owner.Src = new RectF(0, 0, 12, 16);
            Owner.CenterPosition = new Vector2DF(0, 8);
            //--------------------------------------------------

            // Center
            //--------------------------------------------------
            Center.Texture = texture;
            Center.Src = new RectF(12, 0, 16, 16);
            Center.Scale = new Vector2DF((size * 30 + 6) / 16.0f, 1);
            Center.CenterPosition = new Vector2DF(0, 8);
            Center.Position = new Vector2DF(12, 0);
            //--------------------------------------------------

            // Right
            //--------------------------------------------------
            Right.Texture = texture;
            Right.Src = new RectF(28, 0, 12, 16);
            Right.CenterPosition = new Vector2DF(12, 8);
            Right.Position = new Vector2DF(size * 30 + 30, 0);
            //--------------------------------------------------

            var m = ChildManagementMode.RegistrationToLayer;
            var t = ChildTransformingMode.Position;
            var d = ChildDrawingMode.Color;
            Owner.AddDrawnChild(Center, m, t, d);
            Owner.AddDrawnChild(Right, m, t, d);
        }

        protected override void OnObjectDisposed()
        {
            Center.Dispose();
            Right.Dispose();
        }
    }

    /// <summary>
    /// ボタン長押しに反応するノートに紐付けるコンポーネント
    /// </summary>
    public class SlideNoteComponent : Object2DComponent
    {
        private new TextureObject2D Owner
        {
            get => (TextureObject2D)base.Owner;
        }

        public string TexturePath;
        public int RightLane;
        public int LeftLane;

        TextureObject2D Center;
        TextureObject2D Right;

        protected override void OnObjectAdded()
        {
            Center = new TextureObject2D();
            Right = new TextureObject2D();

            var texture = Graphics.CreateTexture(TexturePath);
            var size = RightLane - LeftLane;

            // Left
            //--------------------------------------------------
            Owner.Texture = texture;
            Owner.Src = new RectF(0, 0, 8, 16);
            Owner.Scale = new Vector2DF((size * 15 + 8) / 8.0f, 1);
            Owner.CenterPosition = new Vector2DF(0, 8);
            //--------------------------------------------------

            // Center
            //--------------------------------------------------
            Center.Texture = texture;
            Center.Src = new RectF(8, 0, 14, 16);
            Center.CenterPosition = new Vector2DF(0, 8);
            Center.Position = new Vector2DF(size * 15 + 8, 0);
            //--------------------------------------------------

            // Right
            //--------------------------------------------------
            Right.Texture = texture;
            Right.Src = new RectF(22, 0, 8, 16);
            Right.CenterPosition = new Vector2DF(8, 8);
            Right.Scale = new Vector2DF((size * 15 + 8) / 8.0f, 1);
            Right.Position = new Vector2DF(size * 30 + 30, 0);
            //--------------------------------------------------

            var m = ChildManagementMode.RegistrationToLayer;
            var t = ChildTransformingMode.Position;
            var d = ChildDrawingMode.Color;
            Owner.AddDrawnChild(Center, m, t, d);
            Owner.AddDrawnChild(Right, m, t, d);
        }

        protected override void OnObjectDisposed()
        {
            Center.Dispose();
            Right.Dispose();
        }
    }

    /// <summary>
    /// エフェクトが発生するノートに紐づけるコンポーネント
    /// </summary>
    public class EffectEmitComponent : Object2DComponent
    {
        private new Note Owner
        {
            get => (Note)base.Owner;
        }

        protected override void OnObjectDisposed()
        {
            var size = Owner.NoteInfo.RightLane - Owner.NoteInfo.LeftLane + 1;
            Owner.Scene.AddEffect(Owner.Position + new Vector2DF(size * 15, 0));
        }
    }
}
