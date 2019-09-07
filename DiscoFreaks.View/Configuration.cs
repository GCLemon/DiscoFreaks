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
    }
}
