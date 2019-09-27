﻿using System.Collections.Generic;
using asd;

namespace DiscoFreaks
{
    public class TuneLayer : Layer2D
    {
        // 読み込んだ譜面
        private List<Score> Scores = Score.CreateList();

        // 現在選択している譜面のID
        private int ScoreID;

        // 選択された譜面
        public Score SelectedScore
        {
            get => Scores[ScoreID];
        }

        // レイヤーが登録されているシーン
        private new SelectScene Scene
        {
            get => (SelectScene)base.Scene;
        }

        // コンポーネント
        public UIComponent UIComponent
        {
            get => (UIComponent)GetComponent("UI");
        }

        // ジャケット画像
        private readonly ScoreInfoObject ScoreInfo;

        // 選択中ではないが表示する曲名
        private readonly List<Makinas> AppearingScores;

        // コンストラクタ
        public TuneLayer()
        {
            // コンポーネントを作成・追加
            AddComponent(new UIComponent(), "UI");

            // オブジェクト
            ScoreInfo = new ScoreInfoObject();
            AppearingScores = new List<Makinas>
            {
                new Makinas(32, 3) { Position = new Vector2DF(30, 100), Color = new Color(255, 255, 255,  15) },
                new Makinas(32, 3) { Position = new Vector2DF(30, 150), Color = new Color(255, 255, 255,  31) },
                new Makinas(32, 3) { Position = new Vector2DF(30, 200), Color = new Color(255, 255, 255,  63) },
                new Makinas(32, 3) { Position = new Vector2DF(30, 250), Color = new Color(255, 255, 255, 127) },
                new Makinas(32, 3) { Position = new Vector2DF(30, 500), Color = new Color(255, 255, 255, 127) },
                new Makinas(32, 3) { Position = new Vector2DF(30, 550), Color = new Color(255, 255, 255,  63) },
                new Makinas(32, 3) { Position = new Vector2DF(30, 600), Color = new Color(255, 255, 255,  31) },
                new Makinas(32, 3) { Position = new Vector2DF(30, 650), Color = new Color(255, 255, 255,  15) }
            };
        }

        protected override void OnAdded()
        {
            // レイヤーに諸々のオブジェクトを追加
            AddObject(ScoreInfo);
            foreach (var text_obj in AppearingScores)
                AddObject(text_obj);

            ScoreInfo.SetInfo();
        }

        protected override void OnUpdated()
        {
            // View
            //--------------------------------------------------
            // 画面に表示する曲名の変更
            for (int i = 0; i < AppearingScores.Count; ++i)
            {
                int id = ScoreID + i - (i >= 4 ? 3 : 4);
                id = Math.Mod(id, Scores.Count);
                AppearingScores[i].Text = Scores[id].Title;
            }
            //--------------------------------------------------

            // Controll
            //--------------------------------------------------
            if (Scene.CurrentMode == SelectScene.Mode.Music)
            {
                if (Input.KeyPush(Keys.Up))
                {
                    ScoreID = Math.Mod(ScoreID - 1, Scores.Count);
                    Scene.PlayBGM();
                    ScoreInfo.SetInfo();
                }

                if (Input.KeyPush(Keys.Down))
                {
                    ScoreID = Math.Mod(ScoreID + 1, Scores.Count);
                    Scene.PlayBGM();
                    ScoreInfo.SetInfo();
                }

                // 選択中の難易度の変更
                foreach (var difficulty in Enum.GetValues<Difficulty>())
                {
                    var diff = (Difficulty)difficulty;
                    if (Scores[ScoreID][diff] != null)
                    {
                        Scene.Difficulty = diff;
                        break;
                    }
                }
            }
            //--------------------------------------------------
        }
    }
}