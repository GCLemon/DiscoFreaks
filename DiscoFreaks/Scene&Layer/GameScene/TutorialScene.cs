using System.Collections.Generic;

namespace DiscoFreaks
{
    public class TutorialScene : GameScene
    {
        public TutorialScene(Score score) : base(score, Difficulty.Casual)
        {

        }

        // コルーチン
        protected override IEnumerator<object> GetCoroutine()
        {
            while (true) yield return null;
        }
    }
}
