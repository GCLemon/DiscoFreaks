using System.Collections.Generic;
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

    public class NoteInfo
    {
        public int LeftLane;
        public int RightLane;
        public long VisualTiming;
        public long AudioTiming;
    }

    /// <summary>
    /// ノーツの親クラス
    /// </summary>
    public abstract class Note : TextureObject2D
    {
        /// <summary>
        /// ソフランに関する情報を格納
        /// </summary>
        public class SofLan : EmptyObject2D
        {
            long Timing;
            double AfterSpeed;

            public SofLan(long Timing, double AfterSpeed)
            {
                this.Timing = Timing;
                this.AfterSpeed = AfterSpeed;
            }

            protected override void OnUpdate()
            {
                if (NoteTimer.AudioTime >= Timing)
                    NoteTimer.SetSpeed(AfterSpeed);
            }
        }

        // ノーツが流れる速度
        public static double HighSpeed;

        // 判定調整
        public static long Ofset;

        // ノーツの位置を計算するためのタイマー
        public static readonly NoteTimer NoteTimer = new NoteTimer();

        // 基本情報
        public readonly NoteInfo NoteInfo;

        // レーン番号に対応するキー
        protected readonly List<Keys> JudgeKeys;

        // 登録されているシーン
        public GameScene Scene { get => (GameScene)Layer.Scene; }

        public Note(NoteInfo NoteInfo)
        {
            // 基本情報の設定
            this.NoteInfo = NoteInfo;

            // キーの設定
            JudgeKeys = new List<Keys>();
            for (int i = NoteInfo.LeftLane - 1; i <= NoteInfo.RightLane + 1; ++i)
            {
                Keys[] keys =
                {
                    Keys.Q, Keys.A, Keys.W, Keys.S, Keys.E,
                    Keys.D, Keys.R, Keys.F, Keys.T, Keys.G,
                    Keys.Y, Keys.H, Keys.U, Keys.J, Keys.I,
                    Keys.K, Keys.O, Keys.L, Keys.P,
                    Keys.Semicolon, Keys.LeftBracket,
                    Keys.Apostrophe, Keys.RightBracket,
                    Keys.Backslash
                };

                if (0 <= i && i < 24) JudgeKeys.Add(keys[i]);
            }

            System.Console.WriteLine(NoteInfo.VisualTiming);
        }


        protected override void OnUpdate()
        {
            var error = NoteTimer.VisualTime - NoteInfo.VisualTiming;

            // 描画位置の設定
            var pos_x = 120 + 30 * NoteInfo.LeftLane;
            var pos_y = 600 + error * 0.075f * HighSpeed;
            Position = new Vector2DF(pos_x, (float)pos_y);
        }

        protected override void OnDispose()
        {
            foreach (var c in Children) c.Dispose();
        }

        /// <summary>
        /// ノーツが重なっているかを判定する
        /// </summary>
        protected bool IsOverlapped(Note note)
        {
            var state1 = note.Position.Y > Position.Y;
            var state2 = NoteInfo.LeftLane - 1 < note.NoteInfo.RightLane + 1;
            var state3 = note.NoteInfo.LeftLane - 1 < NoteInfo.RightLane + 1;
            return state1 && state2 && state3;
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
