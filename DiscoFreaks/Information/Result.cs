using System.Linq;

namespace DiscoFreaks
{
    /// <summary>
    /// ランク
    /// </summary>
    public enum Rank
    {
        /// <summary>
        /// 0 ≦ 得点 ≦ 499,999
        /// </summary>
        F,

        /// <summary>
        /// 500,000 ≦ 得点 ≦ 599,999
        /// </summary>
        E,

        /// <summary>
        /// 600,000 ≦ 得点 ≦ 699,999
        /// </summary>
        D,

        /// <summary>
        /// 700,000 ≦ 得点 ≦ 799,999
        /// </summary>
        C,

        /// <summary>
        /// 800,000 ≦ 得点 ≦ 89,999
        /// </summary>
        B,

        /// <summary>
        /// 850,000 ≦ 得点 ≦ 899,999
        /// </summary>
        A,

        /// <summary>
        /// 900,000 ≦ 得点 ≦ 949,999
        /// </summary>
        S,

        /// <summary>
        /// 9500,000 ≦ 得点 ≦ 979,999
        /// </summary>
        SS,

        /// <summary>
        /// 9800,000 ≦ 得点 ≦ 999,999
        /// </summary>
        SSS,

        /// <summary>
        /// 得点 = 1,000,000
        /// </summary>
        EXC
    }

    public enum ClearJudgement
    {
        /// <summary>
        /// クリア失敗(ランクがD以下)
        /// </summary>
        Failure,

        /// <summary>
        /// クリア成功(ランクがC以上)
        /// </summary>
        Success,

        /// <summary>
        /// フルコンボ(ランクがC以上かつMiss,Nearの数が0)
        /// </summary>
        Fullcombo,

        /// <summary>
        /// パーフェクト(ランクがEXC)
        /// </summary>
        Excellent
    }

    /// <summary>
    /// ゲームの結果
    /// </summary>
    public struct Result
    {
        public int Just => just;
        public int Cool => cool;
        public int Good => good;
        public int Near => near;
        public int Miss => miss;
        public int Combo => combo;
        public int BestCombo => best_combo;
        public int Score => score;

        public Rank Rank => GetRank(score);

        public ClearJudgement ClearJudgement
        {
            get
            {
                if (700_000 <= score)
                {
                    if (near == 0 && miss == 0)
                    {
                        if (good == 0 && cool == 0)
                        {
                            return ClearJudgement.Excellent;
                        }
                        return ClearJudgement.Fullcombo;
                    }
                    return ClearJudgement.Success;
                }
                return ClearJudgement.Failure;
            }
        }

        private int just;
        private int cool;
        private int good;
        private int near;
        private int miss;
        private int note_point;
        private int max_note_point;
        private int combo;
        private int best_combo;
        private int max_combo;
        private int score;

        public Result(Detail detail)
        {
            just = cool = good = near = miss = note_point = 0;
            max_note_point =
                    detail.Notes.Count(x => x is TapNote) * 50 +
                    detail.Notes.Count(x => x is HoldNote) * 80 +
                    detail.Notes.Count(x => x is SlideNote) * 10;

            combo = best_combo = 0;
            max_combo = detail.Notes.Count();

            score = 0;
        }

        private void ChangePoint(ref int judge_count, int note_point_add, bool reset_combo)
        {
            ++judge_count;
            note_point += note_point_add;
            combo = reset_combo ? 0 : combo + 1;
            if (best_combo < combo) best_combo = combo;
            score = (int)(((double)note_point) / max_note_point * 100_0000);
        }

        public void ChangePointByTapNote(Judgement judgement)
        {
            switch (judgement)
            {
                case Judgement.Just:
                    ChangePoint(ref just, 50, false);
                    break;
                case Judgement.Cool:
                    ChangePoint(ref cool, 35, false);
                    break;
                case Judgement.Good:
                    ChangePoint(ref good, 20, false);
                    break;
                case Judgement.Near:
                    ChangePoint(ref near, 5, true);
                    break;
                case Judgement.Miss:
                    ChangePoint(ref miss, 0, true);
                    break;
            }
        }

        public void ChangePointByHoldNote(Judgement judgement)
        {
            switch (judgement)
            {
                case Judgement.Just:
                    ChangePoint(ref just, 80, false);
                    break;
                case Judgement.Cool:
                    ChangePoint(ref cool, 56, false);
                    break;
                case Judgement.Good:
                    ChangePoint(ref good, 32, false);
                    break;
                case Judgement.Near:
                    ChangePoint(ref near, 8, true);
                    break;
                case Judgement.Miss:
                    ChangePoint(ref miss, 0, true);
                    break;
            }
        }

        public void ChangePointBySlideNote(Judgement judgement)
        {
            switch (judgement)
            {
                case Judgement.Just:
                    ChangePoint(ref just, 10, false);
                    break;
                case Judgement.Cool:
                    ChangePoint(ref cool, 7, false);
                    break;
                case Judgement.Good:
                    ChangePoint(ref good, 4, false);
                    break;
                case Judgement.Near:
                    ChangePoint(ref near, 1, true);
                    break;
                case Judgement.Miss:
                    ChangePoint(ref miss, 0, true);
                    break;
            }
        }

        public static Rank GetRank(int score)
        {
            if (500_000 <= score && score <= 599_999) return Rank.E;
            if (600_000 <= score && score <= 699_999) return Rank.D;
            if (700_000 <= score && score <= 799_999) return Rank.C;
            if (800_000 <= score && score <= 849_999) return Rank.B;
            if (850_000 <= score && score <= 899_999) return Rank.A;
            if (900_000 <= score && score <= 949_999) return Rank.S;
            if (950_000 <= score && score <= 979_999) return Rank.SS;
            if (980_000 <= score && score <= 999_999) return Rank.SSS;
            if (score == 1000_000) return Rank.EXC;

            return Rank.F;
        }
    }
}
