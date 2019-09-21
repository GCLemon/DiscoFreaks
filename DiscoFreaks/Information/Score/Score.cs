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

        private Score(InitInfo info)
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

            // 読み込んだ情報を格納する変数
            InitInfo init_info = new InitInfo
            {
                Title = string.Empty,
                Subtitle = string.Empty,
                SoundPath = string.Empty,
                JacketPath = string.Empty,
                Difficulty = Difficulty.Casual,
                DetailInfo = new InitInfo.Detail
                {
                    Level = 1,
                    Ofset = 0,
                    InitialBPM = 120,
                    Notes = new List<Note>(),
                    SofLans = new List<Note.SofLan>()
                }
            };

            double bpm = 120;
            double speed = 1;
            double intercept = 0;
            (double curr, double prev) timing = (0, 0);
            (double curr, double prev) beat = (0, 0);

            List<(NoteInfo info, double end_beat)> hold_info = new List<(NoteInfo, double)>();
            int hold_info_count = 0;

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

                        if ((match = Regex.Match(line, "\\s*(.+)\\s*=\\s*(.+)\\s*")).Success)
                        {
                            string key = match.Groups[1].Value;
                            string value = match.Groups[2].Value;

                            switch (key)
                            {
                                case "TITLE": init_info.Title = value; break;
                                case "SUBTITLE": init_info.Subtitle = value; break;
                                case "SOUND": init_info.SoundPath = value; break;
                                case "JACKET": init_info.JacketPath = value; break;

                                case "DIFFICULTY": init_info.Difficulty = Enum.Parse<Difficulty>(value); break;
                                case "LEVEL": init_info.DetailInfo.Level = int.Parse(value); break;
                                case "OFSET": init_info.DetailInfo.Ofset = int.Parse(value); break;
                                case "INITIAL_BPM":
                                    bpm = double.Parse(value);
                                    init_info.DetailInfo.InitialBPM = bpm;
                                    break;
                            }
                        }

                        if ((match = Regex.Match(line, "\\s*(.+)\\s*\\(\\s*(\\d+(\\.\\d+)?\\s*,?\\s*)+\\)\\s*")).Success)
                        {
                            string key = match.Groups[1].Value;
                            var matches = Regex.Matches(line, "\\d+(\\.\\d+)?");
                            beat.curr = double.Parse(matches[0].Value);
                            timing.curr = timing.prev + (beat.curr - beat.prev) / bpm * 60_000;

                            while(hold_info_count < hold_info.Count)
                            {
                                var info = hold_info[hold_info_count].info;
                                var end_beat = hold_info[hold_info_count].end_beat;
                                var _timing = timing.prev + (end_beat - beat.prev) / bpm * 60_000;
                                var endinfo = new NoteInfo
                                {
                                    LeftLane = info.LeftLane,
                                    RightLane = info.RightLane,
                                    VisualTiming = (long)(speed * _timing + intercept),
                                    AudioTiming = (long)_timing
                                };

                                if (end_beat > beat.curr) break;

                                init_info.DetailInfo.Notes.Add(new HoldNote(info, endinfo));
                                ++hold_info_count;
                            }

                            switch (key)
                            {
                                case "tap_note":

                                    init_info.DetailInfo.Notes.Add(new TapNote(new NoteInfo
                                    {
                                        LeftLane = int.Parse(matches[1].Value),
                                        RightLane = int.Parse(matches[1].Value) + int.Parse(matches[2].Value) - 1,
                                        VisualTiming = (long)(speed * timing.curr + intercept),
                                        AudioTiming = (long)timing.curr
                                    }));

                                    break;

                                case "hold_note":

                                    var info = new NoteInfo
                                    {
                                        LeftLane = int.Parse(matches[2].Value),
                                        RightLane = int.Parse(matches[2].Value) + int.Parse(matches[3].Value) - 1,
                                        VisualTiming = (long)(speed * timing.curr + intercept),
                                        AudioTiming = (long)timing.curr
                                    };

                                    hold_info.Add((info, double.Parse(matches[1].Value) + beat.curr));

                                    break;

                                case "slide_note":

                                    init_info.DetailInfo.Notes.Add(new SlideNote(new NoteInfo
                                    {
                                        LeftLane = int.Parse(matches[1].Value),
                                        RightLane = int.Parse(matches[1].Value) + int.Parse(matches[2].Value) - 1,
                                        VisualTiming = (long)(speed * timing.curr + intercept),
                                        AudioTiming = (long)timing.curr
                                    }));

                                    break;

                                case "speed_change":

                                    intercept += (speed - double.Parse(matches[1].Value)) * timing.curr;
                                    speed = double.Parse(matches[1].Value);
                                    var sof_lan = new Note.SofLan((long)timing.curr, speed);
                                    init_info.DetailInfo.SofLans.Add(sof_lan);

                                    break;

                                case "tempo_change":

                                    bpm = double.Parse(matches[1].Value);

                                    break;
                            }

                            beat.prev = beat.curr;
                            timing.prev = timing.curr;
                        }
                    }

                    // 共通の情報を持つ譜面を検出する
                    Score score = list.Find(x =>
                        x.Title == init_info.Title &&
                        x.Subtitle == init_info.Subtitle &&
                        x.SoundPath == init_info.SoundPath &&
                        x.JacketPath == init_info.JacketPath);

                    // 条件を満たすオブジェクトがあった場合
                    // 既存のオブジェクトに詳細情報を追加
                    if (score != null)
                        score[init_info.Difficulty] = new Detail(init_info.DetailInfo);


                    // そうでない場合は新規にオブジェクトを追加する
                    else list.Add(new Score(init_info));
                }
            }

            // 譜面リストを返す
            return list;
        }
    }
}
