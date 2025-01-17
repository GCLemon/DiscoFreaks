﻿using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// このゲームで用いるテキストオブジェクト
    /// </summary>
    public abstract class GeneralText : TextObject2D
    {
        private new Vector2DF CenterPosition;
        private string FontPath;

        /// <summary>
        /// 描画する文字列を設定する
        /// </summary>
        public new string Text
        {
            set
            {
                base.Text = value;
                WritingDirection direction = WritingDirection.Horizontal;
                Vector2DF size = Font.CalcTextureSize(base.Text, direction).To2DF();
                base.CenterPosition = size * CenterPosition;
            }
        }

        /// <summary>
        /// オブジェクトを描画する際の拡大率を設定する
        /// </summary>
        public new Vector2DF Scale
        {
            set
            {
                base.Scale = value;
                WritingDirection direction = WritingDirection.Horizontal;
                Vector2DF size = Font.CalcTextureSize(base.Text, direction).To2DF();
                base.CenterPosition = size * CenterPosition;
            }
        }

        protected GeneralText(string path, int size, Color color, int o_size, Color o_color, Vector2DF center = new Vector2DF())
        {
            FontPath = path;
            Font = Graphics.CreateDFont(path, size, color, o_size, o_color);
            CenterPosition = center;
        }

        protected GeneralText(string path, int size, int o_size, Vector2DF center = new Vector2DF())
        {
            FontPath = path;
            Color white = new Color(255, 255, 255);
            Color black = new Color(0, 0, 0);
            Font = Graphics.CreateDFont(path, size, white, o_size, black);
            CenterPosition = center;
        }

        protected GeneralText(string path, Vector2DF center = new Vector2DF())
        {
            FontPath = path;
            CenterPosition = center;
        }

        public void ResetFont(int size, Color color, int o_size, Color o_color)
        {
            Font = Graphics.CreateDFont(FontPath, size, color, o_size, o_color);
        }

        public void ResetFont(int size, int o_size)
        {
            Color white = new Color(255, 255, 255);
            Color black = new Color(0, 0, 0);
            Font = Graphics.CreateDFont(FontPath, size, white, o_size, black);
        }
    }

    /// <summary>
    /// The Strong Gamer フォント
    /// </summary>
    public class TheStrongGamer : GeneralText
    {
        public TheStrongGamer(int size, Color color, int o_size, Color o_color, Vector2DF center = new Vector2DF())
            : base("Font/TheStrongGamer.ttf", size, color, o_size, o_color, center)
        {
        }

        public TheStrongGamer(int size, int o_size, Vector2DF center = new Vector2DF())
            : base("Font/TheStrongGamer.ttf", size, o_size, center)
        {
        }

        public TheStrongGamer(Vector2DF center = new Vector2DF())
            : base("Font/TheStrongGamer.ttf", center)
        {
        }
    }

    /// <summary>
    /// Scan Line フォント
    /// </summary>
    public class ScanLine : GeneralText
    {
        public ScanLine(int size, Color color, int o_size, Color o_color, Vector2DF center = new Vector2DF())
            : base("Font/ScanLine.ttf", size, color, o_size, o_color, center)
        {
        }

        public ScanLine(int size, int o_size, Vector2DF center = new Vector2DF())
            : base("Font/ScanLine.ttf", size, o_size, center)
        {
        }

        public ScanLine(Vector2DF center = new Vector2DF())
            : base("Font/ScanLine.ttf", center)
        {
        }
    }

    /// <summary>
    /// Score Dozer フォント
    /// </summary>
    public class ScoreDozer : GeneralText
    {
        public ScoreDozer(int size, Color color, int o_size, Color o_color, Vector2DF center = new Vector2DF())
            :base("Font/ScoreDozer.ttf", size, color, o_size, o_color, center)
        {
        }

        public ScoreDozer(int size, int o_size, Vector2DF center = new Vector2DF())
            :base("Font/ScoreDozer.ttf", size, o_size, center)
        {
        }

        public ScoreDozer(Vector2DF center = new Vector2DF())
            : base("Font/ScoreDozer.ttf", center)
        {
        }
    }

    /// <summary>
    /// Head Up Daisy フォント
    /// </summary>
    public class HeadUpDaisy : GeneralText
    {
        public HeadUpDaisy(int size, Color color, int o_size, Color o_color, Vector2DF center = new Vector2DF())
            : base("Font/HeadUpDaisy.ttf", size, color, o_size, o_color, center)
        {
        }

        public HeadUpDaisy(int size, int o_size, Vector2DF center = new Vector2DF())
            : base("Font/HeadUpDaisy.ttf", size, o_size, center)
        {
        }

        public HeadUpDaisy(Vector2DF center = new Vector2DF())
            : base("Font/HeadUpDaisy.ttf", center)
        {
        }
    }

    /// <summary>
    /// Grid Gazer フォント
    /// </summary>
    public class GridGazer : GeneralText
    {
        public GridGazer(int size, Color color, int o_size, Color o_color, Vector2DF center = new Vector2DF())
            : base("Font/GridGazer.ttf", size, color, o_size, o_color, center)
        {
        }

        public GridGazer(int size, int o_size, Vector2DF center = new Vector2DF())
            : base("Font/GridGazer.ttf", size, o_size, center)
        {
        }

        public GridGazer(Vector2DF center = new Vector2DF())
            : base("Font/GridGazer.ttf", center)
        {
        }
    }

    /// <summary>
    /// Makinas フォント
    /// </summary>
    public class Makinas : GeneralText
    {
        public Makinas(int size, Color color, int o_size, Color o_color, Vector2DF center = new Vector2DF())
            : base("Font/Makinas.otf", size, color, o_size, o_color, center)
        {
        }

        public Makinas(int size, int o_size, Vector2DF center = new Vector2DF())
            : base("Font/Makinas.otf", size, o_size, center)
        {
        }

        public Makinas(Vector2DF center = new Vector2DF())
            : base("Font/Makinas.otf", center)
        {
        }
    }
}
