using System.IO;

namespace DiscoFreaks
{
    public enum EffectType
    {
        Simple,
        Explosion,
        BlueInk,
        Gust,
        MagicCircle,
        Stardust
    }

    /// <summary>
    /// プレイヤーが設定する項目
    /// </summary>
    public struct Configuration
    {
        // Play Configuration
        //--------------------------------------------------
        public double HighSpeed;
        public int Ofset;
        //--------------------------------------------------

        // Visual Configuration
        //--------------------------------------------------
        public EffectType EffectType;
        public int EffectSize;
        public int Luminance;
        public bool ShowLaneBorder;
        public bool ShowBeatBorder;
        //--------------------------------------------------

        // Audio Configuration
        //--------------------------------------------------
        public int BGMVolume;
        public int SEVolume;
        //--------------------------------------------------

        /// <summary>
        /// プレイヤー設定を初期化する
        /// </summary>
        public static Configuration Init()
        {
            return new Configuration
            {
                // Play Configuration
                //--------------------------------------------------
                HighSpeed = 1,
                Ofset = 0,
                //--------------------------------------------------

                // Visual Configuration
                //--------------------------------------------------
                EffectType = 0,
                EffectSize = 100,
                Luminance = 100,
                ShowLaneBorder = true,
                ShowBeatBorder = true,
                //--------------------------------------------------

                // Audio Configuration
                //--------------------------------------------------
                BGMVolume = 100,
                SEVolume = 100
                //--------------------------------------------------
            };
        }

        /// <summary>
        /// 設定をファイルから読み込む
        /// </summary>
        public static Configuration Load()
        {
            // ファイルを読み込み
            using (var stream = new FileStream("PlaySetting.config",　FileMode.Open))
            {
                // バイナリ読み込み用オブジェクト
                var reader = new BinaryReader(stream);

                // ファイル読み込み
                var config = new Configuration
                {
                    // Play Configuration
                    //--------------------------------------------------
                    HighSpeed = reader.ReadDouble(),
                    Ofset = reader.ReadInt32(),
                    //--------------------------------------------------

                    // Visual Configuration
                    //--------------------------------------------------
                    EffectType = (EffectType)reader.ReadInt32(),
                    EffectSize = reader.ReadInt32(),
                    Luminance = reader.ReadInt32(),
                    ShowLaneBorder = reader.ReadBoolean(),
                    ShowBeatBorder = reader.ReadBoolean(),
                    //--------------------------------------------------

                    // Audio Configuration
                    //--------------------------------------------------
                    BGMVolume = reader.ReadInt32(),
                    SEVolume = reader.ReadInt32()
                    //--------------------------------------------------
                };

                // ファイルを閉じる
                reader.Close();
                reader.Dispose();

                // 設定を戻す
                return config;
            }
        }

        /// <summary>
        /// 設定内容をファイルに書き込む
        /// </summary>
        public static void Save(Configuration config)
        {
            var filemode =
                File.Exists("PlaySetting.config") ?
                FileMode.Open :
                FileMode.Create;

            // ファイルを読み込み
            using (var stream = new FileStream("PlaySetting.config", filemode))
            {
                var writer = new BinaryWriter(stream);

                // Play Configuration
                //--------------------------------------------------
                writer.Write(config.HighSpeed);
                writer.Write(config.Ofset);
                //--------------------------------------------------

                // Visual Configuration
                //--------------------------------------------------
                writer.Write((int)config.EffectType);
                writer.Write(config.EffectSize);
                writer.Write(config.Luminance);
                writer.Write(config.ShowLaneBorder);
                writer.Write(config.ShowBeatBorder);
                //--------------------------------------------------

                // Audio Configuration
                //--------------------------------------------------
                writer.Write(config.BGMVolume);
                writer.Write(config.SEVolume);
                //--------------------------------------------------

                writer.Close();
                writer.Dispose();
            }
        }
    }
}
