namespace DiscoFreaks
{
    public class SpeedChange : EmptyObject2D
    {
        long Timing;
        double AfterSpeed;

        public SpeedChange(long Timing, double AfterSpeed)
        {
            this.Timing = Timing;
            this.AfterSpeed = AfterSpeed;
        }

        protected override void OnUpdate()
        {
            if(Note.NoteTimer.AudioTime >= Timing)
                Note.NoteTimer.SetSpeed(AfterSpeed);
        }
    }
}
