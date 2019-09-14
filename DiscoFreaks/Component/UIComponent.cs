using System;
using System.Linq;
using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// UIに用いるレイヤーに紐付けるコンポーネント
    /// </summary>
    public class UIComponent : Layer2DComponent
    {
        private int Phase;

        protected override void OnLayerUpdated()
        {
            Input.AcceptInput = Phase == 0;
            var objs = Owner.Objects.Where(x => x.Parent == null);
            foreach(var obj in objs)
                obj.Position += new Vector2DF(-8 * Phase, 0);
            Phase -= Math.Sign(Phase);
        }

        public void MoveRight() { if (Phase == 0) Phase = -15; }

        public void MoveLeft() { if (Phase == 0) Phase = 15; }
    }

    /// <summary>
    /// ループするUIを実現するコンポーネント
    /// </summary>
    public class LoopingUIComponent : Layer2DComponent
    {
        private int Pages;

        public LoopingUIComponent(int Pages)
        {
            if (Pages < 2)
            {
                Console.WriteLine("ERROR : Pages must be larger than 1.");
                Engine.Close();
            }
            this.Pages = Pages;
        }

        protected override void OnLayerUpdated()
        {
            var objs = Owner.Objects.Where(x => x.Parent == null);
            foreach (var obj in objs)
            {
                if (obj.Position.X > 960 * Pages - 480)
                    obj.Position -= new Vector2DF(960 * Pages, 0);

                if (obj.Position.X < 1440 - 960 * Pages)
                    obj.Position += new Vector2DF(960 * Pages, 0);
            }
        }
    }
}
