using asd;

namespace DiscoFreaks
{
    public class PINObject : Makinas
    {
        private new TweetLayer Layer
        {
            get => (TweetLayer)base.Layer;
        }

        public string PINCode;
        public int SettingDigit;
        private bool IsErrorShown;

        private Makinas[] PINValue;
        private Makinas Error;

        public PINObject() : base(48, 4, new Vector2DF(0.5f, 0.5f))
        {
            PINCode = "0000000";

            Text = "PIN番号を入力してください。";
            Position = new Vector2DF(2400, 250);

            var center = new Vector2DF(0.5f, 0.5f);
            PINValue = new Makinas[]
            {
                new Makinas(210, 4, center) { Text = "0", Position = new Vector2DF(-390, 200) },
                new Makinas(210, 4, center) { Text = "0", Position = new Vector2DF(-260, 200) },
                new Makinas(210, 4, center) { Text = "0", Position = new Vector2DF(-130, 200) },
                new Makinas(210, 4, center) { Text = "0", Position = new Vector2DF(0, 200) },
                new Makinas(210, 4, center) { Text = "0", Position = new Vector2DF(130, 200) },
                new Makinas(210, 4, center) { Text = "0", Position = new Vector2DF(260, 200) },
                new Makinas(210, 4, center) { Text = "0", Position = new Vector2DF(390, 200) }
            };

            Error = new Makinas(36, 4, center)
            {
                Text = "無効なPINコードが入力されました。\nもう一度入力しなおしてください。",
                Position = new Vector2DF(0, 350),
                Color = new Color(255, 255, 255, 0)
            };
        }

        protected override void OnAdded()
        {
            var m = ChildManagementMode.RegistrationToLayer;
            var t = ChildTransformingMode.Position;
            var d = ChildDrawingMode.Nothing;
            foreach (var v in PINValue) AddDrawnChild(v, m, t, d);
            Error.AddComponent(new FadeInComponent(30, 255), "FadeIn");
            AddChild(Error, m, t);
        }

        protected override void OnUpdate()
        {
            for(int i = 0; i < 7; ++i)
            {
                PINValue[i].Text = PINCode[i].ToString();
                var alpha = i == SettingDigit ? 255 : 63;
                PINValue[i].Color = new Color(255, 255, 255, alpha);
            }
        }

        public void ShowError()
        {
            if (!IsErrorShown)
            {
                ((ITextComponent)Error.GetComponent("FadeIn")).Trigger();
                IsErrorShown = true;
            }
        }
    }
}
