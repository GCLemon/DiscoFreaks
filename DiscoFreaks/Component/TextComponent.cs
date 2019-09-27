using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// インターフェース
    /// </summary>
    public interface ITextComponent
    {
        void Trigger();
    }

    /// <summary>
    /// 横からフェードインするテキストのコンポーネント
    /// </summary>
    public class FadeInComponent : Object2DComponent, ITextComponent
    {
        private float PositionX;
        private int Phase;

        protected override void OnObjectAdded()
        {
            PositionX = Owner.Position.X;
        }

        protected override void OnUpdate()
        {
            Owner.Position -= new Vector2DF(Phase, 0);
            ((TextObject2D)Owner).Color = new Color(255, 255, 255, 255 - Phase * 17);
            if (Phase != 0) --Phase;
        }

        public void Trigger()
        {
            Phase = 15;
            Owner.Position = new Vector2DF(PositionX + 120, Owner.Position.Y);
            ((TextObject2D)Owner).Color = new Color(255, 255, 255, 0);
        }
    }

    /// <summary>
    /// 文字が縮むコンポーネント
    /// </summary>
    public class ShrinkComponent : Object2DComponent, ITextComponent
    {
        private int Phase;
        private float Scale;

        public ShrinkComponent(float scale)
        {
            Scale = scale;
        }

        protected override void OnUpdate()
        {
            if (Phase >= 0)
            {
                var scale = 1 + Phase * Phase * (Scale - 1) * 0.01f;
                Owner.Scale = new Vector2DF(scale, scale);
                --Phase;
            }
        }

        public void Trigger()
        {
            Phase = 10;
        }
    }

    /// <summary>
    /// 文字が拡大するコンポーネント
    /// </summary>
    public class SwellComponent : Object2DComponent, ITextComponent
    {
        private int Phase;
        private float Scale;

        public SwellComponent(float scale)
        {
            Scale = scale;
        }

        protected override void OnUpdate()
        {
            if (Phase >= 0)
            {
                var scale = Scale - Phase * Phase * (Scale - 1) * 0.0025f;
                Owner.Scale = new Vector2DF(scale, scale);
                --Phase;
            }
        }

        public void Trigger()
        {
            Phase = 20;
        }
    }

    /// <summary>
    /// 文字に色をつけるコンポーネント
    /// </summary>
    public class ColorComponent : Object2DComponent, ITextComponent
    {
        private int MaxPhase;
        private int Phase;
        private Color Color;

        public ColorComponent(int MaxPhase)
        {
            this.MaxPhase = MaxPhase;
        }

        protected override void OnUpdate()
        {
            if (Phase >= 0)
            {
                Color.A = (byte)(127 * (double)Phase / MaxPhase);
                ((TextObject2D)Owner).Color = Color;
                --Phase;
            }
        }

        public void Trigger()
        {
            Color = new Color(255, 255, 255);
            Phase = MaxPhase;
        }

        public void Trigger(Color color)
        {
            Color = color;
            Phase = MaxPhase;
        }
    }
}
