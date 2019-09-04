using System.Collections.Generic;

namespace DiscoFreaks.Core
{
    public class SelectSceneModel
    {
        // 曲選択シーンのモード
        public enum Mode
        {
            Music,
            Difficulty,
            Option
        }

        // オプションでの設定項目
        public enum OptionItem
        {
            HighSpeed,
            Ofset,
        }

        // 現在のモード
        public Mode CurrentMode
        {
            get;
            private set;
        }

        public void MoveMode(int mode_move)
        {
            CurrentMode += mode_move;
            if (CurrentMode == Mode.Difficulty)
                foreach (var d in Enum.GetValues<Difficulty>())
                    if (SelectedScore[(Difficulty)d] != null)
                    {
                        SelectedDifficulty = (Difficulty)d;
                        break;
                    }
        }

        #region 楽曲選択

        // 読み込んだ譜面
        private List<Score> Scores = Score.CreateList();

        // 現在選択している譜面のID
        private int ScoreID;

        // 現在選択している譜面
        public Score SelectedScore => Scores[ScoreID];

        // 画面に表示する譜面
        public List<string> AppearingScores
        {
            get
            {
                List<string> score_list = new List<string>();
                for (int i = -4; i <= 4; ++i)
                    if (i != 0)
                    {
                        int id = Math.Mod(ScoreID + i, Scores.Count);
                        Score score = Scores[id];
                        score_list.Add(score.Title);
                    }
                return score_list;
            }
        }

        public void MoveScore(int id_move) =>
            ScoreID = Math.Mod(ScoreID + id_move, Scores.Count);

        #endregion

        #region 難易度選択

        // 選択された難易度
        public Difficulty SelectedDifficulty
        {
            get;
            private set;
        }

        public void MoveDifficulty(int dif_move)
        {
            int d = (int)SelectedDifficulty;
            for (
                int i = d + dif_move;
                0 <= i && i < 4;
                i += 1 * System.Math.Sign(dif_move)
            )
            {
                if (SelectedScore[(Difficulty)i] != null)
                {
                    SelectedDifficulty = (Difficulty)i;
                    break;
                }
            }
        }

        #endregion

        #region その他設定

        public OptionItem CurrentOptionItem
        {
            get;
            private set;
        }

        public void MoveOptionItem(int dif_move)
        {
            int d = (int)SelectedDifficulty;
            for (
                int i = d + dif_move;
                0 <= i && i < 2;
                i += 1 * System.Math.Sign(dif_move)
            )
            {
                if (SelectedScore[(Difficulty)i] != null)
                {
                    SelectedDifficulty = (Difficulty)i;
                    break;
                }
            }
        }

        #endregion
    }
}
