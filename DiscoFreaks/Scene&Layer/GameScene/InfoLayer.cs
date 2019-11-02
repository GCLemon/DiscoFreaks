using asd;

namespace DiscoFreaks
{
    public class InfoLayer : Layer2D
    {
        // レイヤーが登録されているシーン
        private new GameScene Scene
        {
            get => (GameScene)base.Scene;
        }

        private readonly Makinas Title;
        private readonly Makinas Level;
        private readonly Makinas Score;

        public InfoLayer(Score score, Difficulty difficulty)
        {
            Title = new Makinas(24, 4)
            {
                Text = score.Title,
                Position = new Vector2DF(10, 10)
            };

            var level = score[difficulty].Level;
            var level_text = "Lv." + level;
            var level_color = new Color();
            switch (difficulty)
            {
                case Difficulty.Casual:
                    level_text = "CASUAL     " + level_text;
                    level_color = new Color(166, 226, 46);
                    break;
                case Difficulty.Stylish:
                    level_text = "STYLISH    " + level_text;
                    level_color = new Color(253, 151, 31);
                    break;
                case Difficulty.Freaky:
                    level_text = "FREEKY     " + level_text;
                    level_color = new Color(249, 38, 114);
                    break;
                case Difficulty.Psychic:
                    level_text = "PSYCHIC    " + level_text;
                    level_color = new Color(174, 129, 255);
                    break;
            }
            Level = new Makinas(18, level_color, 4, new Color())
            {
                Text = level_text,
                Position = new Vector2DF(10, 40)
            };

            Score = new Makinas(36, 4, new Vector2DF(1, 0))
            {
                Text = "SCORE : 000000",
                Position = new Vector2DF(950, 10)
            };
        }

        protected override void OnAdded()
        {
            AddObject(Title);
            AddObject(Level);
            AddObject(Score);
        }

        protected override void OnUpdated()
        {
            Score.Text = "SCORE : " + Scene.Result.Score.ToString("000000");
        }
    }
}
