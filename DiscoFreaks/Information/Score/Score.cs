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
        public string Title { get; }
        public string Subtitle { get; }
        public string SoundPath { get; }
        public string JacketPath { get; }

        private Dictionary<Difficulty, Detail> Detail;

        public Detail this[Difficulty difficulty]
        {
            get
            {
                return Detail.ContainsKey(difficulty) ? Detail[difficulty] : null;
            }

            set
            {
                if (Detail.ContainsKey(difficulty))
                    Detail.Remove(difficulty);
                Detail.Add(difficulty, value);
            }
        }

        public Score(InitInfo info)
        {
            // 基本情報の設定
            Title = info.Title;
            Subtitle = info.Subtitle;
            SoundPath = info.SoundPath;
            JacketPath = info.JacketPath;

            // 詳細情報の設定
            Detail = new Dictionary<Difficulty, Detail>();
            Detail[info.Difficulty] = new Detail(info.DetailInfo);
        }

        public static List<Score> CreateList()
        {
            // 譜面リスト
            List<Score> list = new List<Score>();

            // Score 配下の譜面ファイル全てに対して処理
            foreach (string path in Directory.GetFiles("./Score", "*.frk", SearchOption.AllDirectories))
            {
                InitInfo init_info = InitInfo.Create(path);

                // 共通の情報を持つ譜面を検出する
                Score score = list.Find(x =>
                    x.Title == init_info.Title &&
                    x.Subtitle == init_info.Subtitle &&
                    x.SoundPath == init_info.SoundPath &&
                    x.JacketPath == init_info.JacketPath
                );

                // 条件を満たすオブジェクトがあった場合
                // 既存のオブジェクトに詳細情報を追加
                if (score != null)
                    score[init_info.Difficulty] = new Detail(init_info.DetailInfo);


                // そうでない場合は新規にオブジェクトを追加する
                else list.Add(new Score(init_info));
            }

            // 譜面リストを返す
            return list;
        }
    }
}
