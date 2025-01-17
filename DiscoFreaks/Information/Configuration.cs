﻿using System.IO;

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
        public bool AutoMode;
        //--------------------------------------------------

        // Visual Configuration
        //--------------------------------------------------
        public EffectType EffectType;
        public int EffectSize;
        public int Luminance;
        //--------------------------------------------------

        // Audio Configuration
        //--------------------------------------------------
        public int BGMVolume;
        public int SEVolume;
        //--------------------------------------------------

        /// <summary>
        /// 設定をファイルから読み込む
        /// </summary>
        public static Configuration Load()
        {
            // ファイルを読み込み
            try
            {
                using (var stream = new FileStream("PlaySetting.config", FileMode.Open))
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
                        AutoMode = reader.ReadBoolean(),
                        //--------------------------------------------------

                        // Visual Configuration
                        //--------------------------------------------------
                        EffectType = (EffectType)reader.ReadInt32(),
                        EffectSize = reader.ReadInt32(),
                        Luminance = reader.ReadInt32(),
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
            catch(IOException)
            {
                // 設定の初期化
                var config = new Configuration
                {
                    // Play Configuration
                    //--------------------------------------------------
                    HighSpeed = 1,
                    Ofset = 0,
                    AutoMode = false,
                    //--------------------------------------------------

                    // Visual Configuration
                    //--------------------------------------------------
                    EffectType = 0,
                    EffectSize = 100,
                    Luminance = 100,
                    //--------------------------------------------------

                    // Audio Configuration
                    //--------------------------------------------------
                    BGMVolume = 50,
                    SEVolume = 50
                    //--------------------------------------------------
                };

                // 設定を保存
                Save(config);

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
                writer.Write(config.AutoMode);
                //--------------------------------------------------

                // Visual Configuration
                //--------------------------------------------------
                writer.Write((int)config.EffectType);
                writer.Write(config.EffectSize);
                writer.Write(config.Luminance);
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
