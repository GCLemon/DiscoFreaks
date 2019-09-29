using System.Collections.Generic;
using System.IO;

namespace DiscoFreaks
{
    /// <summary>
    /// 自己ベスト
    /// </summary>
    public struct HighScore
    {
        // 自己ベスト
        private Dictionary<Difficulty, (int score, ClearJudgement judge)> Score;

        // 得点の取得・設定
        public (int score, ClearJudgement judge) this[Difficulty d]
        {
            get => Score[d];
            set => Score[d] = value;
        }

        public static HighScore Load(string tune_title)
        {
            // ファイルを読み込み
            try
            {
                using (var stream = new FileStream("HighScore/" + tune_title + ".dat", FileMode.Open))
                {
                    // バイナリ読み込み用オブジェクト
                    var reader = new BinaryReader(stream);

                    // ファイル読み込み
                    var high_score = new HighScore
                    {
                        Score = new Dictionary<Difficulty, (int score, ClearJudgement judge)>
                        {
                            { Difficulty.Casual, (reader.ReadInt32(), (ClearJudgement)reader.ReadInt32()) },
                            { Difficulty.Stylish, (reader.ReadInt32(), (ClearJudgement)reader.ReadInt32()) },
                            { Difficulty.Freeky, (reader.ReadInt32(), (ClearJudgement)reader.ReadInt32()) },
                            { Difficulty.Psychic, (reader.ReadInt32(), (ClearJudgement)reader.ReadInt32()) }
                        }
                    };

                    // 自己ベストを返す
                    return high_score;
                }
            }
            catch (IOException)
            {
                // 自己ベストの初期化
                var high_score = new HighScore
                {
                    Score = new Dictionary<Difficulty, (int Score, ClearJudgement Judge)>
                    {
                        { Difficulty.Casual, (0, ClearJudgement.Failure) },
                        { Difficulty.Stylish, (0, ClearJudgement.Failure) },
                        { Difficulty.Freeky, (0, ClearJudgement.Failure) },
                        { Difficulty.Psychic, (0, ClearJudgement.Failure) }
                    }
                };

                // 自己ベストの保存
                Save(high_score, tune_title);

                // 自己ベストを返す
                return high_score;
            }
        }

        public static void Save(HighScore high_score, string tune_title)
        {
            var filemode =
                File.Exists(tune_title + ".dat") ?
                FileMode.Open :
                FileMode.Create;

            // ファイルを読み込み
            using (var stream = new FileStream("HighScore/" + tune_title + ".dat", filemode))
            {
                var writer = new BinaryWriter(stream);

                foreach(var d in Enum.GetValues<Difficulty>())
                {
                    writer.Write(high_score[(Difficulty)d].score);
                    writer.Write((int)high_score[(Difficulty)d].judge);
                }

                writer.Close();
                writer.Dispose();
            }
        }
    }
}
