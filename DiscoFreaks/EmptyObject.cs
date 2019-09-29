using asd;

namespace DiscoFreaks
{
    public abstract class EmptyObject2D
    {
        public delegate void Override();

        public class ObjectCore : GeometryObject2D
        {
            private readonly Override MAdded;
            private readonly Override MUpdate;
            private readonly Override MRemoved;
            private readonly Override MDispose;

            public ObjectCore(Override m_added,Override m_update,Override m_removed,Override m_dispose)
            {
                MAdded = m_added;
                MUpdate = m_update;
                MRemoved = m_removed;
                MDispose = m_dispose;
            }

            protected override void OnAdded() => MAdded();
            protected override void OnUpdate() => MUpdate();
            protected override void OnRemoved() => MRemoved();
            protected override void OnDispose() => MDispose();
        }

        public ObjectCore Core;
        
        public static implicit operator Object2D(EmptyObject2D obj)
        {
            return obj.Core;
        }

        public EmptyObject2D()
        {
            Core = new ObjectCore(OnAdded, OnUpdate, OnRemoved, OnDispose);
        }

        protected virtual void OnAdded() {}

        protected virtual void OnUpdate() {}
        
        protected virtual void OnRemoved() {}

        protected virtual void OnDispose() {}

        public void Dispose() => Core.Dispose();
    }
}
