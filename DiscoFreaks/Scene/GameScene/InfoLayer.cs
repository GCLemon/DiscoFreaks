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

        // 得点の表示
        private readonly Makinas Score;

        public InfoLayer()
        {
            Score = new Makinas(36, 4, new Vector2DF(1, 0))
            {
                Text = "SCORE : 000000",
                Position = new Vector2DF(950, 10)
            };
        }

        protected override void OnAdded()
        {
            AddObject(new Makinas(24, 4)
            {
                Text = Scene.Score.Title,
                Position = new Vector2DF(10, 10)
            });

            switch (Scene.Difficulty)
            {
                case Difficulty.Casual:
                    AddObject(new Makinas(18, new Color(166, 226, 46), 4, new Color())
                    {
                        Text = Scene.Difficulty + "     Lv." + Scene.Score[Scene.Difficulty].Level,
                        Position = new Vector2DF(10, 40)
                    });
                    break;
                case Difficulty.Stylish:
                    AddObject(new Makinas(18, new Color(230, 219, 116), 4, new Color())
                    {
                        Text = Scene.Difficulty + "    Lv." + Scene.Score[Scene.Difficulty].Level,
                        Position = new Vector2DF(10, 40)
                    });
                    break;
                case Difficulty.Freeky:
                    AddObject(new Makinas(18, new Color(249, 38, 114), 4, new Color())
                    {
                        Text = Scene.Difficulty + "     Lv." + Scene.Score[Scene.Difficulty].Level,
                        Position = new Vector2DF(10, 40)
                    });
                    break;
                case Difficulty.Psychic:
                    AddObject(new Makinas(18, new Color(174, 129, 255), 4, new Color())
                    {
                        Text = Scene.Difficulty + "    Lv." + Scene.Score[Scene.Difficulty].Level,
                        Position = new Vector2DF(10, 40)
                    });
                    break;
            }

            AddObject(Score);
        }
    }
}
