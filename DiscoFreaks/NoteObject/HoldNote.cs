﻿using System.Diagnostics;
using System.Linq;
using asd;

namespace DiscoFreaks
{
    /// <summary>
    /// ホールドノート
    /// </summary>
    public class HoldNote : Note
    {
        private readonly GeometryObject2D LinkPart;
        private readonly EndNote EndNote;

        private bool IsMoving = true;
        private int TempJudge;
        private Stopwatch HoldTimer;
        private Stopwatch TotalTimer;

        public HoldNote(int left_lane, int right_lane, long visual_timing, long audio_timing, long visual_length, long audio_length)
            : base(left_lane, right_lane, visual_timing, audio_timing)
        {
            DrawingPriority = 1;

            AddComponent(
                new TapNoteComponent("Image/HoldNote.png", RightLane, LeftLane),
                "TapNote"
            );
            AddComponent(new EffectEmitComponent(), "Effect");

            LinkPart = new GeometryObject2D
            {
                Shape = new RectangleShape(),
                Color = new Color(230, 219, 116, 127),
                DrawingPriority = 0
            };

            EndNote = new EndNote(left_lane, right_lane, visual_timing + visual_length, audio_timing + audio_length);
            EndNote.DrawingPriority = 1;

            HoldTimer = new Stopwatch();
            TotalTimer = new Stopwatch();
        }

        protected override void OnAdded()
        {
            var m = ChildManagementMode.RegistrationToLayer;
            var t = ChildTransformingMode.Nothing;
            var d = ChildDrawingMode.Color;
            AddDrawnChild(LinkPart, m, t, d);
            AddDrawnChild(EndNote, m, t, d);
        }

        protected override void OnUpdate()
        {
            if(IsMoving) base.OnUpdate();

            // 描画設定
            LinkPart.Position = Position + new Vector2DF(12, 0);
            var size_x = (RightLane - LeftLane + 1) * 30 - 24;
            var size_y = EndNote.GetGlobalPosition().Y - LinkPart.GetGlobalPosition().Y;
            var area = new RectF(0, 0, size_x, size_y);
            ((RectangleShape)LinkPart.Shape).DrawingArea = area;

            // 先頭のノートが未反応の状態における動作
            if (IsMoving)
            {

                if (IsAutoPlaying)
                {
                    var judgement = Judge();
                    if (judgement == Judgement.Just || NoteTimer.AudioTime - AudioTiming > 0)
                    {
                        Position = new Vector2DF(Position.X, 600);
                        IsMoving = false;
                        TempJudge = (int)Judge();
                        HoldTimer.Start();
                        TotalTimer.Start();
                    }
                }
                else
                {
                    if (!Layer.Objects
                        .Where(x => x is Note)
                        .Any(x => IsOverlapped((Note)x))
                    )
                    {
                        bool is_pressed = JudgeKeys.Any(x => Input.KeyPush(x));
                        bool is_judgable = Judge() != Judgement.None;
                        if (is_pressed && is_judgable)
                        {
                            Position = new Vector2DF(Position.X, 600);
                            IsMoving = false;
                            TempJudge = (int)Judge();
                            HoldTimer.Start();
                            TotalTimer.Start();
                        }
                    }
                }

                // Miss判定の場合は強制的に判定する
                if (Judge() == Judgement.Miss)
                {
                    Scene.Result.ChangePointByTapNote(Judge());
                    EndNote.RemoveComponent("Effect");
                    Dispose();
                }
            }

            // 先頭のノートが反応済の状態における動作
            else
            {
                bool is_holding = IsAutoPlaying;

                // ホールドされているかを判定
                foreach (var key in JudgeKeys)
                    is_holding |= Input.KeyHold(key);

                // ホールドされている場合
                if (is_holding)
                {
                    if (!HoldTimer.IsRunning) HoldTimer.Start();
                    Color = new Color(255, 255, 255, 255);
                }

                // ホールドされていない場合
                else
                {
                    if (HoldTimer.IsRunning) HoldTimer.Stop();
                    Color = new Color(255, 255, 255, 63);
                }

                // 終端のノートがDisposeされた時に判定
                if (!EndNote.IsAlive)
                {
                    HoldTimer.Stop();
                    TotalTimer.Stop();

                    var h_msec = HoldTimer.ElapsedMilliseconds;
                    var t_msec = TotalTimer.ElapsedMilliseconds;
                    double rate = (double)h_msec / t_msec;

                    if (rate > 0.9) TempJudge -= 1;
                    else if (rate > 0.7) TempJudge += 0;
                    else if (rate > 0.5) TempJudge += 1;
                    else if (rate > 0.3) TempJudge += 2;
                    else                 TempJudge += 3;

                    if (TempJudge >= 5) TempJudge = 4;
                    if (TempJudge <  0) TempJudge = 0;
                    if (TempJudge >= 3)  RemoveComponent("Effect");
                    Scene.Result.ChangePointByHoldNote((Judgement)TempJudge);

                    Dispose();
                }
            }
        }
    }
}
