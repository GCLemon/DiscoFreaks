namespace DiscoFreaks.Core
{
    /// <summary>
    /// ゲーム全体が持つモデル
    /// </summary>
    public class GameModel
    {
        #region 譜面関係

        /// <summary>
        /// 譜面を取得・設定する
        /// </summary>
        public Score Score { get; set; }

        /// <summary>
        /// 選択された難易度を取得・設定する
        /// </summary>
        public Difficulty Difficulty { get; set; }

        /// <summary>
        /// 譜面の詳細情報を取得する
        /// </summary>
        public Score.Detail ScoreDetail => Score[Difficulty];

        #endregion

        #region プレイヤー関係

        /// <summary>
        /// ノーツの速さを取得・設定する
        /// </summary>
        public double HighSpeed
        {
            get => high_speed;
            set => high_speed = Math.Clamp(value, 1.0, 10.0);
        }
        private double high_speed = 1;

        /// <summary>
        /// ノーツのタイミング誤差を取得・設定する
        /// </summary>
        public int Ofset { get; set; }

        #endregion

        #region スコア関係

        #endregion
    }
}
