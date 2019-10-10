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

            public SofLan(long timing, double after_speed)
            {
                Timing = timing;
                AfterSpeed = after_speed;
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

        // 自動演奏が有効か
        public static bool IsAutoPlaying;

        // ノーツの位置を計算するためのタイマー
        public static readonly NoteTimer NoteTimer = new NoteTimer();

        // 基本情報
        public readonly int LeftLane;
        public readonly int RightLane;
        public readonly long VisualTiming;
        public readonly long AudioTiming;

        // レーン番号に対応するキー
        protected readonly List<Keys> JudgeKeys;

        // 登録されているシーン
        public GameScene Scene { get => (GameScene)Layer.Scene; }

        public Note(int left_lane, int right_lane, long visual_timing, long audio_timing)
        {
            // 基本情報の設定
            LeftLane = left_lane;
            RightLane = right_lane;
            VisualTiming = visual_timing;
            AudioTiming = audio_timing;

            // キーの設定
            JudgeKeys = new List<Keys>();
            for (int i = LeftLane - 1; i <= RightLane + 1; ++i)
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
        }


        protected override void OnUpdate()
        {
            // 描画位置の設定
            var error = NoteTimer.VisualTime -VisualTiming;
            var pos_x = 120 + 30 * LeftLane;
            var pos_y = 600 + error * 0.075f * HighSpeed;
            Position = new Vector2DF(pos_x, (float)pos_y);

            // 色の設定
            if (NoteTimer.ElapsedMilliseconds < 1000)
            {
                var alpha = (int)(NoteTimer.ElapsedMilliseconds * 255 / 1000);
                Color = new Color(255, 255, 255, alpha);
            }
            else
            {
                Color = new Color(255, 255, 255, 255);
            }
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
            var state2 = LeftLane < note.RightLane;
            var state3 = note.LeftLane < RightLane;
            return state1 && state2 && state3;
        }

        /// <summary>
        /// タイミングとの誤差によって判定する
        /// </summary>
        protected Judgement Judge()
        {
            var error = NoteTimer.AudioTime - AudioTiming;

            if (Math.Abs(error) <= 33) return Judgement.Just;
            if (Math.Abs(error) <= 67) return Judgement.Cool;
            if (Math.Abs(error) <= 100) return Judgement.Good;
            if (Math.Abs(error) <= 133) return Judgement.Near;
            if (error > 133) return Judgement.Miss;

            return Judgement.None;
        }
    }
}
