using System.Diagnostics;

namespace DiscoFreaks
{
    /// <summary>
    /// ノートの位置を計算するためのタイマー
    /// </summary>
    public class NoteTimer : Stopwatch
    {
        private double LapsingSpeed = 1;
        private long Intercept;

        private new long ElapsedMilliseconds
        {
            get => base.ElapsedMilliseconds - Note.Ofset - Ofset - 2000;
        }

        public long Ofset;

        // 経過時間を取得[ms]
        public long AudioTime =>
            ElapsedMilliseconds;

        // 経過速度を考慮した経過時間[ms]
        public long VisualTime =>
            (long)(ElapsedMilliseconds * LapsingSpeed) + Intercept;

        // 経過速度を変更する
        public void SetSpeed(double speed)
        {
            Intercept += (long)((LapsingSpeed - speed) * ElapsedMilliseconds);
            LapsingSpeed = speed;
        }
    }
}
