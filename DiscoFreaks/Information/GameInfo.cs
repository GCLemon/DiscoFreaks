namespace DiscoFreaks
{
    /// <summary>
    /// ゲームの情報
    /// </summary>
    public struct GameInfo
    {
        /// <summary>
        /// 選択された譜面
        /// </summary>
        public Score Score;

        /// <summary>
        /// 選択された難易度
        /// </summary>
        public Difficulty Difficulty;

        /// <summary>
        /// ユーザーが設定したオプション
        /// </summary>
        public Configuration Configuration;
    }
}
