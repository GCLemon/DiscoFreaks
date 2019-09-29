using System.Collections.Generic;
using asd;

namespace DiscoFreaks
{
    public class PauseLayer : Layer2D
    {
        public enum Item
        {
            Resume,
            Retry,
            Return
        }
        public Item SelectingItem;

        private readonly GeometryObject2D Mask;
        private readonly ScanLine Pause;
        private readonly Dictionary<Item, ScanLine> Items;

        public PauseLayer()
        {
            IsDrawn = false;
            IsUpdated = false;

            var center = new Vector2DF(0.5f, 0.5f);
            Pause = new ScanLine(120, 4, center)
            {
                Text = "PAUSE",
                Position = new Vector2DF(480, 150)
            };
            Items = new Dictionary<Item, ScanLine>
            {
                { Item.Resume,  new ScanLine(60, 4, center) { Text = "RESUME", Position = new Vector2DF(480, 300) } },
                { Item.Retry,  new ScanLine(60, 4, center) { Text = "RETRY", Position = new Vector2DF(480, 400) } },
                { Item.Return,  new ScanLine(60, 4, center) { Text = "TUNE SELECT", Position = new Vector2DF(480, 500) } },
            };
            Mask = new GeometryObject2D
            {
                Shape = new RectangleShape { DrawingArea = new RectF(0, 0, 960, 720) },
                Color = new Color(0, 0, 0, 127)
            };
        }

        protected override void OnAdded()
        {
            AddObject(Mask);
            AddObject(Pause);
            foreach (var kv in Items) AddObject(kv.Value);
        }

        protected override void OnUpdated()
        {
            foreach (var kv in Items) kv.Value.Color = new Color(255, 255, 255, 63);
            Items[SelectingItem].Color = new Color(255, 255, 255);
        }
    }
}
