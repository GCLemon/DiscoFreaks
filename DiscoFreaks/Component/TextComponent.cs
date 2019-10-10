using System.Collections.Generic;
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
    public class SlideComponent : Object2DComponent, ITextComponent
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
        private readonly int MaxPhase;
        private int Phase;
        private readonly float Scale;

        public ShrinkComponent(int max_phase, float scale)
        {
            MaxPhase = max_phase;
            Scale = scale;
            Phase = -1;
        }

        protected override void OnUpdate()
        {
            if (Phase >= 0)
            {
                var scale = 1 + (float)Math.Pow((double)Phase / MaxPhase, 2) * (Scale - 1);
                Owner.Scale = new Vector2DF(scale, scale);
                --Phase;
            }
        }

        public void Trigger()
        {
            Phase = MaxPhase;
        }
    }

    /// <summary>
    /// 文字が拡大するコンポーネント
    /// </summary>
    public class SwellComponent : Object2DComponent, ITextComponent
    {
        private readonly int MaxPhase;
        private int Phase;
        private readonly float Scale;

        public SwellComponent(int max_phase, float scale)
        {
            MaxPhase = max_phase;
            Scale = scale;
            Phase = -1;
        }

        protected override void OnUpdate()
        {
            if (Phase >= 0)
            {
                var scale = Scale - (float)Math.Pow((double)Phase / MaxPhase, 2) * (Scale - 1);
                Owner.Scale = new Vector2DF(scale, scale);
                --Phase;
            }
        }

        public void Trigger()
        {
            Phase = MaxPhase;
        }
    }

    /// <summary>
    /// 文字に色をつけるコンポーネント
    /// </summary>
    public class FadeOutComponent : Object2DComponent, ITextComponent
    {
        private readonly int MaxPhase;
        private readonly int MaxAlpha;
        private int Phase;
        private Color Color;

        public FadeOutComponent(int max_phase, int max_alpha)
        {
            MaxPhase = max_phase;
            MaxAlpha = max_alpha;
            Phase = -1;
        }

        protected override void OnUpdate()
        {
            if (Phase >= 0)
            {
                Color.A = (byte)(MaxAlpha * (double)Phase / MaxPhase);
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

    /// <summary>
    /// 文字に色をつけるコンポーネント
    /// </summary>
    public class FadeInComponent : Object2DComponent, ITextComponent
    {
        private readonly int MaxPhase;
        private readonly int MaxAlpha;
        private int Phase;
        private Color Color;

        public FadeInComponent(int max_phase, int max_alpha)
        {
            MaxPhase = max_phase;
            MaxAlpha = max_alpha;
            Phase = -1;
        }

        protected override void OnUpdate()
        {
            if (Phase >= 0)
            {
                Color.A = (byte)(MaxAlpha * (1 - (double)Phase / MaxPhase));
                ((TextObject2D)Owner).Color = Color;
                --Phase;
            }
        }

        public void Trigger()
        {
            Color = new Color(255, 255, 255, 0);
            Phase = MaxPhase;
        }

        public void Trigger(Color color)
        {
            Color = color;
            Phase = MaxPhase;
        }
    }

    /// <summary>
    /// 文字を1文字ずつ表示するコンポーネント
    /// </summary>
    public class TypingComponent : Object2DComponent, ITextComponent
    {
        private IEnumerator<object> Coroutine;

        private string Text;

        protected override void OnUpdate()
        {
            Coroutine?.MoveNext();
        }

        public void Trigger()
        {
            Text = ((TextObject2D)Owner).Text;
            Coroutine = GetCoroutine();
        }

        private IEnumerator<object> GetCoroutine()
        {
            ((TextObject2D)Owner).Text = "";
            foreach (char c in Text)
            {
                ((TextObject2D)Owner).Text += c;
                for (int i = 0; i < 5; ++i) yield return null;
            }
        }
    }
}
