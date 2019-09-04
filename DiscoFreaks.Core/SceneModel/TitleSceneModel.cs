namespace DiscoFreaks.Core
{
    public class TitleSceneModel
    {
        public enum MenuItem
        {
            StartGame,
            Tutorial,
            Credits,
            QuitGame
        }

        public MenuItem SelectingItem
        {
            get;
            private set;
        }

        public void PrevItem() =>
            SelectingItem = (MenuItem)Math.Mod((int)SelectingItem - 1, 4);

        public void NextItem() =>
            SelectingItem = (MenuItem)Math.Mod((int)SelectingItem + 1, 4);
    }
}
