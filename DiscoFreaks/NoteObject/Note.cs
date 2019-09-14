using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// 判定の種類
    /// </summary>
    public enum Judgement
    {
        /// <summary>  未判定の状態 </summary>
        None = -1,

        /// <summary>  誤差 ≦ 33ms </summary>
        Just,

        /// <summary>  誤差 ≦ 67ms </summary>
        Cool,

        /// <summary>  誤差 ≦ 100ms </summary>
        Good,

        /// <summary>  誤差 ≦ 133ms </summary>
        Near,

        /// <summary>  誤差 > 133ms </summary>
        Miss
    }

    /// <summary>
    /// ノーツに関する基本情報
    /// </summary>
    public struct NoteInfo
    {
        public long AudioTiming;
        public long VisualTiming;
        public int LeftLane;
        public int RightLane;
    }

    /// <summary>
    /// ノーツの親クラス
    /// </summary>
    public abstract class Note : TextureObject2D
    {
        /// <summary>
        /// レーン番号に対応するキー
        /// </summary>
        protected readonly Keys[] CorrespondingKeys =
        {
            Keys.Q, Keys.A, Keys.W, Keys.S, Keys.E,
            Keys.D, Keys.R, Keys.F, Keys.T, Keys.G,
            Keys.Y, Keys.H, Keys.U, Keys.J, Keys.I,
            Keys.K, Keys.O, Keys.L, Keys.P,
            Keys.Semicolon, Keys.LeftBracket,
            Keys.Apostrophe, Keys.Backslash
        };

        /// <summary>
        /// ノーツに関する基本情報
        /// </summary>
        protected NoteInfo NoteInfo { get; }

        /// <summary>
        /// ノーツの位置を計算するためのタイマー
        /// </summary>
        public static readonly NoteTimer NoteTimer = new NoteTimer();

        /// <summary>
        /// ノーツが流れる速度
        /// </summary>
        public static double HighSpeed;

        public Note(NoteInfo info)
        {
            // 基本情報の設定
            NoteInfo = info;
        }

        protected override void OnUpdate()
        {
            var error = NoteTimer.VisualTime - NoteInfo.VisualTiming;
            System.Console.WriteLine(NoteInfo.VisualTiming);

            // 描画位置の設定
            var pos_x = 120 + 30 * NoteInfo.LeftLane;
            var pos_y = 600 + error * 0.075f;
            Position = new Vector2DF(pos_x, pos_y);
        }

        /// <summary>
        /// ノーツが重なっているかを判定する
        /// </summary>
        protected bool IsOverlapped(Note note)
        {
            NoteInfo note1 = NoteInfo;
            NoteInfo note2 = note.NoteInfo;
            return note2.LeftLane <= note1.RightLane &&
                   note1.LeftLane <= note2.RightLane;
        }

        /// <summary>
        /// タイミングとの誤差によって判定する
        /// </summary>
        protected Judgement Judge()
        {
            var error = NoteTimer.AudioTime - NoteInfo.AudioTiming;

            if (Math.Abs(error) <= 33) return Judgement.Just;
            if (Math.Abs(error) <= 67) return Judgement.Cool;
            if (Math.Abs(error) <= 100) return Judgement.Good;
            if (Math.Abs(error) <= 133) return Judgement.Near;
            if (error > 133) return Judgement.Miss;

            return Judgement.None;
        }
    }
}
