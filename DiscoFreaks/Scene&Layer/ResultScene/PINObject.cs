using asd;

namespace DiscoFreaks
{
    public class PINObject : Makinas
    {
        private new TweetLayer Layer
        {
            get => (TweetLayer)base.Layer;
        }

        public string PINCode { get; private set; }
        private int SettingDigit;
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

            if (Layer.Scene.CurrentMode == ResultScene.Mode.Tweet
                && Layer.CurrentMode == TweetLayer.Mode.Authorize)
            {
                foreach (var k in Enum.GetValues<Keys>())
                {
                    Keys key = (Keys)k;

                    if (Input.KeyPush(key))
                    {
                        int value = (int)key;

                        // 入力されたものが数字だった場合
                        if (7 <= value && value <= 16)
                        {
                            var pin_code = PINCode.ToCharArray();
                            pin_code[SettingDigit] = (value - 7).ToString()[0];
                            PINCode = new string(pin_code);
                            SettingDigit = Math.Mod(++SettingDigit, 7);
                        }

                        // 入力されたものが上下だった場合
                        if (59 <= value && value <= 60)
                        {
                            var pin_code = PINCode.ToCharArray();
                            var digit = int.Parse(pin_code[SettingDigit].ToString());
                            digit = Math.Mod(digit + (value == 60 ? 1 : -1), 10);
                            pin_code[SettingDigit] = digit.ToString()[0];
                            PINCode = new string(pin_code);
                        }

                        // 入力されたものが左右だった場合
                        if (57 <= value && value <= 58)
                        {
                            SettingDigit += value == 57 ? 1 : -1;
                            SettingDigit = Math.Mod(SettingDigit, 7);
                        }
                    }
                }
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
