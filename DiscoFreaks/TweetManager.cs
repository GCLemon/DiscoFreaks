using System.Diagnostics;
using System.IO;
using CoreTweet;

namespace DiscoFreaks
{
    public static class TweetManager
    {
        // セッション
        private static OAuth.OAuthSession Session;

        // トークン
        private static Tokens Tokens;

        // トークンが null か
        public static bool IsTokensNull => Tokens == null;

        // 認証
        public static void Authorize()
        {
            // OAuth 認証
            var key = "yOQVwQb1sFLzd018R2vicI4kv";
            var key_secret = "yfBIRcD2lgHvwCB4VqUmpvOpwX0sTmyvErIUevgiSdLcrktVDZ";
            Session = OAuth.Authorize(key, key_secret);
            Process.Start(Session.AuthorizeUri.AbsoluteUri);
        }

        // トークンの生成
        public static void CreateToken(string pin)
        {
            // PIN コードの入力
            Tokens = Session.GetTokens(pin);
        }

        // リザルトをツイートする
        public static void TweetResult(string title, int score)
        {
            string message = "";

            message += "【曲名】\n" + title + "\n\n";
            message += "【得点】\n" + score + "点\n\n";
            message += "【檸檬茶からひとこと】\n";

            switch (Result.GetRank(score))
            {
                case Rank.F:
                    message += "話にならん。帰れ。\n\n";
                    break;
                case Rank.E:
                    message += "全然ダメだ。お前の実力は所詮そんなものか。\n\n";
                    break;
                case Rank.D:
                    message += "まだまだ実力が足りんな。腕前に磨きをかけるべし。\n\n";
                    break;
                case Rank.C:
                    message += "まあ、こんなもんでしょ。\n\n";
                    break;
                case Rank.B:
                    message += "割とできてる方なんじゃないかな。悪くないね。\n\n";
                    break;
                case Rank.A:
                    message += "なかなかやるじゃぁないか。よくできました。\n\n";
                    break;
                case Rank.S:
                    message += "上出来! 素晴らしいスコアだな。\n\n";
                    break;
                case Rank.SS:
                    message += "すげぇ……。よくこんなスコア叩き出せるなぁ。\n\n";
                    break;
                case Rank.SSS:
                    message += "信じられない……何なんだこのスコアは……!?\n\n";
                    break;
                case Rank.EXC:
                    message += "もうさ……人間……卒業して……どうぞ。\n\n";
                    break;
            }

            message += "#DiscoFreaks";
            var result = Tokens.Media.Upload(new FileInfo("Result.png"));
            Tokens.Statuses.Update(
                message,
                media_ids: new long[] { result.MediaId }
            );
        }
    }
}
