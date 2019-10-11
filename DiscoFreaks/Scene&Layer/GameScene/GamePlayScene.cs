using System.Collections.Generic;
using asd;

namespace DiscoFreaks
{
    public class GamePlayScene : GameScene
    {
        // ゲーム開始時に再生するエフェクト
        private readonly ReadyGoEffect ReadyGo;

        public GamePlayScene(Score score, Difficulty difficulty) : base(score, difficulty)
        {
            ReadyGo = new ReadyGoEffect();
        }

        // コルーチン
        protected override IEnumerator<object> GetCoroutine()
        {
            // 待機
            for (int i = 0; i < 180; ++i) yield return null;

            // エフェクト追加
            EffectLayer.AddObject(ReadyGo);
            while (ReadyGo.IsAlive) yield return null;

            // 再生開始を待機
            Note.NoteTimer.Start();
            while (Note.NoteTimer.ElapsedMilliseconds < 2000) yield return null;

            // 音を再生する
            var source = Sound.CreateBGM(Score.SoundPath);
            source.IsLoopingMode = false;
            SoundID = Sound.Play(source);
            Sound.SetVolume(SoundID, Configuration.BGMVolume);

            // 音が再生されている間はゲームを続ける
            while (Sound.GetIsPlaying(SoundID))
            {
                if (Input.KeyPush(Keys.Backspace))
                {
                    Sound.Pause(SoundID);
                    Note.NoteTimer.Stop();
                    PauseLayer.IsDrawn = true;
                    PauseLayer.IsUpdated = true;

                    while (true)
                    {
                        if (Input.KeyPush(Keys.Up))
                        {
                            int x = Math.Mod((int)PauseLayer.SelectingItem - 1, 3);
                            PauseLayer.SelectingItem = (PauseLayer.Item)x;
                        }

                        if (Input.KeyPush(Keys.Down))
                        {
                            int x = Math.Mod((int)PauseLayer.SelectingItem + 1, 3);
                            PauseLayer.SelectingItem = (PauseLayer.Item)x;
                        }

                        if (Input.KeyPush(Keys.Enter))
                        {
                            if (PauseLayer.SelectingItem == PauseLayer.Item.Resume)
                            {
                                Sound.Resume(SoundID);
                                Note.NoteTimer.Start();
                                PauseLayer.IsDrawn = false;
                                PauseLayer.IsUpdated = false;
                                break;
                            }

                            if (PauseLayer.SelectingItem == PauseLayer.Item.Retry)
                            {
                                var new_scene = new GamePlayScene(Score, Difficulty);
                                Engine.ChangeSceneWithTransition(new_scene, new TransitionFade(1, 1));
                            }

                            if (PauseLayer.SelectingItem == PauseLayer.Item.Return)
                            {
                                Engine.ChangeSceneWithTransition(new SelectScene(Score), new TransitionFade(1, 1));
                            }
                        }

                        yield return null;
                    }
                }

                yield return null;
            }

            // ゲームを終了する
            var next_scene = new ResultScene(Score, Difficulty, Result, !Note.IsAutoPlaying);
            Engine.ChangeSceneWithTransition(next_scene, new TransitionFade(1, 1));
        }
    }
}
