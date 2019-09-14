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

        /// <summary>
        /// 経過時間を取得[ms]
        /// </summary>
        public long AudioTime =>
            ElapsedMilliseconds;

        /// <summary>
        /// 経過速度を考慮した経過時間[ms]
        /// </summary>
        public long VisualTime =>
            (long)(ElapsedMilliseconds * LapsingSpeed) + Intercept;

        /// <summary>
        /// 経過速度を変更する
        /// </summary>
        public void SetSpeed(double speed)
        {
            Intercept += (long)((LapsingSpeed - speed) * ElapsedMilliseconds);
            LapsingSpeed = speed;
        }
    }
}
