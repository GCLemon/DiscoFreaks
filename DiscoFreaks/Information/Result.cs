using System.Linq;

namespace DiscoFreaks
{
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
                    detail.Notes.Count(x => x is TapNote) * 8 +
                    detail.Notes.Count(x => x is HoldNote) * 12 +
                    detail.Notes.Count(x => x is SlideNote) * 4;

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
            var raw_point = (int)(((double)note_point) / max_note_point * 90_0000);
            var combo_point = (int)(((double)best_combo) / max_combo * 10_0000);
            score = raw_point + combo_point;
        }

        public void ChangePointByTapNote(Judgement judgement)
        {
            switch (judgement)
            {
                case Judgement.Just:
                    ChangePoint(ref just, 8, false);
                    break;
                case Judgement.Cool:
                    ChangePoint(ref cool, 6, false);
                    break;
                case Judgement.Good:
                    ChangePoint(ref good, 4, false);
                    break;
                case Judgement.Near:
                    ChangePoint(ref near, 2, false);
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
                    ChangePoint(ref just, 12, false);
                    break;
                case Judgement.Cool:
                    ChangePoint(ref cool, 9, false);
                    break;
                case Judgement.Good:
                    ChangePoint(ref good, 6, false);
                    break;
                case Judgement.Near:
                    ChangePoint(ref near, 3, false);
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
                    ChangePoint(ref just, 4, false);
                    break;
                case Judgement.Cool:
                    ChangePoint(ref cool, 3, false);
                    break;
                case Judgement.Good:
                    ChangePoint(ref good, 2, false);
                    break;
                case Judgement.Near:
                    ChangePoint(ref near, 1, false);
                    break;
                case Judgement.Miss:
                    ChangePoint(ref miss, 0, true);
                    break;
            }
        }
    }
}
