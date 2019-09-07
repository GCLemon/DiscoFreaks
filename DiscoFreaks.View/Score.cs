using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DiscoFreaks
{
    /// <summary>
    /// 難易度
    /// </summary>
    public enum Difficulty
    {
        Casual,
        Stylish,
        Freeky,
        Psychic
    }

    /// <summary>
    /// 譜面の情報を格納する型
    /// </summary>
    public class Score
    {
        /// <summary>
        /// 難易度ごとに設ける譜面の詳細部分
        /// </summary>
        public class Detail
        {
            /// <summary>
            /// レベル
            /// </summary>
            public int Level { get; }

            /// <summary>
            /// オフセット
            /// </summary>
            public int Ofset { get; }

            /// <summary>
            /// 初期状態のテンポ
            /// </summary>
            public double InitialBPM { get; }

            /// <summary>
            /// 文字列状態の譜面
            /// </summary>
            public Queue<string> RawScore { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            internal Detail
            (
                int level,
                int ofset,
                double initial_bpm,
                Queue<string> raw_score
            )
            {
                Level = level;
                Ofset = ofset;
                InitialBPM = initial_bpm;
                RawScore = raw_score;
            }
        }

        /// <summary>
        /// 曲名
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// サブタイトル
        /// </summary>
        public string Subtitle { get; }

        /// <summary>
        /// 音源のファイルパス
        /// </summary>
        public string SoundPath { get; }

        /// <summary>
        /// ジャケット画像のファイルパス
        /// </summary>
        public string JacketPath { get; }

        /// <summary>
        /// Casual 譜面
        /// </summary>
        private Detail Casual;

        /// <summary>
        /// Stylish 譜面
        /// </summary>
        private Detail Stylish;

        /// <summary>
        /// Freeky 譜面
        /// </summary>
        private Detail Freeky;

        /// <summary>
        /// Psychic 譜面
        /// </summary>
        private Detail Psychic;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal Score
        (
            string title,
            string subtitle,
            string sound_path,
            string jacket_path,
            Difficulty difficulty,
            Detail detail
        )
        {
            // 基本情報の設定
            Title = title;
            Subtitle = subtitle;
            SoundPath = sound_path;
            JacketPath = jacket_path;

            // 詳細情報の設定
            SetScore(difficulty, detail);
        }

        /// <summary>
        /// 難易度ごとの譜面を取得する
        /// </summary>
        public Detail this[Difficulty difficulty]
        {
            get
            {
                switch (difficulty)
                {
                    case Difficulty.Casual:  return Casual;
                    case Difficulty.Stylish: return Stylish;
                    case Difficulty.Freeky:  return Freeky;
                    case Difficulty.Psychic: return Psychic;
                }
                return null;
            }

            set
            {
                switch (difficulty)
                {
                    case Difficulty.Casual:  Casual  = value; break;
                    case Difficulty.Stylish: Stylish = value; break;
                    case Difficulty.Freeky:  Freeky  = value; break;
                    case Difficulty.Psychic: Psychic = value; break;
                }
            }
        }

        /// <summary>
        /// 譜面を設定する
        /// </summary>
        public void SetScore(Difficulty difficulty, Detail detail)
        {
            switch (difficulty)
            {
                case Difficulty.Casual:  Casual  = detail; break;
                case Difficulty.Stylish: Stylish = detail; break;
                case Difficulty.Freeky:  Freeky  = detail; break;
                case Difficulty.Psychic: Psychic = detail; break;
            }
        }

        /// <summary>
        /// 譜面のリストを作成する
        /// </summary>
        public static List<Score> CreateList()
        {
            // 譜面リスト
            List<Score> list = new List<Score>();

            // 読み込んだ情報を格納する変数
            string title = string.Empty;
            string subtitle = string.Empty;
            string sound_path = string.Empty;
            string jacket_path = string.Empty;
            Difficulty difficulty = Difficulty.Casual;
            int level = 1;
            int ofset = 0;
            double initial_bpm = 120;
            Queue<string> raw_score = new Queue<string>();

            // Score 配下の譜面ファイル全てに対して処理
            foreach (string path in Directory.GetFiles("./Score", "*.frk", SearchOption.AllDirectories))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    // ファイル読み込み
                    while (reader.Peek() != -1)
                    {
                        string line = reader.ReadLine();
                        Match match;

                        if ((match = Regex.Match(line, "(.+)\\s*=\\s*(.+)")).Success)
                        {
                            string key = match.Groups[1].Value;
                            string value = match.Groups[2].Value;

                            switch (key)
                            {
                                case "TITLE": title = value; break;
                                case "SUBTITLE": subtitle = value; break;
                                case "SOUND": sound_path = value; break;
                                case "JACKET": jacket_path = value; break;

                                case "DIFFICULTY": difficulty = Enum.Parse<Difficulty>(value); break;
                                case "LEVEL": level = int.Parse(value); break;
                                case "OFSET": ofset = int.Parse(value); break;
                                case "INITIAL_BPM": initial_bpm = double.Parse(value); break;
                            }
                        }
                        else
                        {
                            if (line != "") raw_score.Enqueue(line);
                        }
                    }

                    // 検出関数
                    bool det(Score x) =>
                        x.Title == title &&
                        x.Subtitle == subtitle &&
                        x.SoundPath == sound_path &&
                        x.JacketPath == jacket_path;

                    Score score = list.Find(obj => det(obj));

                    // 条件を満たすオブジェクトがあった場合
                    // 既存のオブジェクトに詳細情報を追加
                    if (score != null)
                        score[difficulty] =
                            new Detail
                            (
                                level,
                                ofset,
                                initial_bpm,
                                raw_score
                            );

                    // そうでない場合は新規にオブジェクトを追加する
                    else
                        list.Add
                        (
                            new Score
                            (
                                title,
                                subtitle,
                                sound_path,
                                jacket_path,
                                difficulty,
                                new Detail
                                (
                                    level,
                                    ofset,
                                    initial_bpm,
                                    raw_score
                                )
                            )
                        );
                }
            }

            // 譜面リストを返す
            return list;
        }
    }
}
