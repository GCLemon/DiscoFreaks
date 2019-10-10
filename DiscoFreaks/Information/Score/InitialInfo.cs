using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DiscoFreaks
{
    /// <summary>
    /// 譜面オブジェクトの初期化に使う構造体
    /// </summary>
    public struct InitInfo
    {
        public struct Detail
        {
            public int Level;
            public int Ofset;
            public double InitialBPM;

            public List<NoteInfo> Notes;
            public List<SofLanInfo> SofLans;
        }

        public string Title;
        public string Subtitle;
        public string SoundPath;
        public string JacketPath;
        public Difficulty Difficulty;
        public Detail DetailInfo;

        public static InitInfo Create(string path)
        {
            // 読み込んだ情報を格納する変数
            InitInfo init_info = new InitInfo
            {
                Title = string.Empty,
                Subtitle = string.Empty,
                SoundPath = string.Empty,
                JacketPath = string.Empty,
                Difficulty = Difficulty.Casual,
                DetailInfo = new Detail
                {
                    Level = 1,
                    Ofset = 0,
                    InitialBPM = 120,
                    Notes = new List<NoteInfo>(),
                    SofLans = new List<SofLanInfo>()
                }
            };

            double bpm = 120;
            double speed = 1;
            double intercept = 0;
            (double curr, double prev) timing = (0, 0);
            (double curr, double prev) beat = (0, 0);

            List<(NoteInfo info, double end_beat)> hold_info = new List<(NoteInfo, double)>();
            int hold_info_count = 0;

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

                        while (hold_info_count < hold_info.Count)
                        {
                            // ホールドノートの追加
                            var info = hold_info[hold_info_count].info;
                            var end_beat = hold_info[hold_info_count].end_beat;
                            var _timing = timing.prev + (end_beat - beat.prev) / bpm * 60_000;
                            info.VisualLength = (long)(speed * _timing + intercept) - info.VisualTiming;
                            info.AudioLength = (long)_timing - info.AudioTiming;

                            if (end_beat > beat.curr) break;

                            init_info.DetailInfo.Notes.Add(info);
                            ++hold_info_count;
                        }

                        switch (key)
                        {
                            case "tap_note":

                                // タップノートを追加する
                                init_info.DetailInfo.Notes.Add(new NoteInfo
                                {
                                    Type = NoteType.TapNote,
                                    LeftLane = int.Parse(matches[1].Value),
                                    RightLane = int.Parse(matches[1].Value) + int.Parse(matches[2].Value) - 1,
                                    VisualTiming = (long)(speed * timing.curr + intercept),
                                    AudioTiming = (long)timing.curr
                                }); ;

                                break;

                            case "hold_note":

                                // ホールドノートを一旦保留する
                                var info = new NoteInfo
                                {
                                    Type = NoteType.HoldNote,
                                    LeftLane = int.Parse(matches[2].Value),
                                    RightLane = int.Parse(matches[2].Value) + int.Parse(matches[3].Value) - 1,
                                    VisualTiming = (long)(speed * timing.curr + intercept),
                                    AudioTiming = (long)timing.curr
                                };

                                hold_info.Add((info, double.Parse(matches[1].Value) + beat.curr));

                                break;

                            case "slide_note":

                                // スライドノートを追加する
                                init_info.DetailInfo.Notes.Add(new NoteInfo
                                {
                                    Type = NoteType.SlideNote,
                                    LeftLane = int.Parse(matches[1].Value),
                                    RightLane = int.Parse(matches[1].Value) + int.Parse(matches[2].Value) - 1,
                                    VisualTiming = (long)(speed * timing.curr + intercept),
                                    AudioTiming = (long)timing.curr
                                });

                                break;

                            case "speed_change":

                                // ソフランを追加する
                                intercept += (speed - double.Parse(matches[1].Value)) * timing.curr;
                                speed = double.Parse(matches[1].Value);
                                var sof_lan = new SofLanInfo
                                {
                                    Timing = (long)timing.curr,
                                    AfterSpeed = speed
                                };
                                init_info.DetailInfo.SofLans.Add(sof_lan);

                                break;

                            case "tempo_change":

                                // テンポを変更する
                                bpm = double.Parse(matches[1].Value);

                                break;
                        }

                        beat.prev = beat.curr;
                        timing.prev = timing.curr;
                    }
                }
            }

            return init_info;
        }
    }
}
