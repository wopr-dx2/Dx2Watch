using System;

#region Javaのimport構文とC#のusingディレクティブとの関係
/*
	基本的に、
	・Javaのimport構文のパッケージ名
	・C#のusingディレクティブの名前空間
	が相互に対応しています。
	（但し、パッケージ名は全て小文字で、名前空間はパスカルケース（略称の場合はすべて大文字）です）

	◆ Android.Content
	・Java
		import android.content.BroadcastReceiver;
		import android.content.Context;
		import android.content.Intent;
		import android.content.IntentFilter;
		import android.content.res.Resources;
	・C#
		using Android.Content;

	◆ Android.Graphics
	・Java
		import android.graphics.Canvas;
		import android.graphics.Color;
		import android.graphics.Paint;
		import android.graphics.Rect;
	・C#
		using Android.Graphics;

	◆ Android.Graphics.Drawable
	・Java
		import android.graphics.drawable.BitmapDrawable;
	・C#
		using Android.Graphics.Drawable;
	
	◆ Android.OS
	・Java
		import android.os.Bundle;
		import android.os.Handler;
		import android.os.Message;
	・C#
		using Android.OS;
	
	◆ Android.Support.Wearable.Watchface
	・Java
		import android.support.wearable.watchface.CanvasWatchFaceService;
		import android.support.wearable.watchface.WatchFaceStyle;
	・C#
		using Android.Support.Wearable.Watchface;

	◆ Android.Support.V4.Content
	・Java
		import android.support.v4.content.ContextCompat;
	・C#
		using Android.Support.V4.Content;
	
	◆ Android.Text.Format
	・Java
		import android.text.format.Time;
	・C#
		using Android.Text.Format;

	◆ Android.View
	・Java
		import android.view.SurfaceHolder;
	・C#
		using Android.Views;
*/
#endregion
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.Wearable.Watchface;
using Android.Text.Format;
using Android.Views;

// Service属性、Metadata属性で使用します。
using Android.App;
// WallpaperServiceクラスで使用します。
using Android.Service.Wallpaper;
//#if DEBUG
//// ログ出力で使用します（デバッグビルドのみ有効）。
//using Android.Util;
//#endif

using Chronoir_net.Chronica.WatchfaceExtension;

namespace Dx2Watch
{
    class MoonAge
    {
        const int INTERVAL_MINUTES = 118;   // 満月開始から次の満月まで
        const int TEN_MINUTES = 10;         // 満月と新月は 10 分間
        const int SEVEN_MINUTES = 7;        // 他の月齢は 7 分間

        const int MESSAGE_MINUTES = -5;     // 5 分前
        const int SNOOZE_MINUTES = -1;      // 1 分前

        private DateTime lastMessage;
        private DateTime lastSnooze;
        private DateTime lastEnded;

        const int DURING_5MIN_SECONDS = 30;
        const int DURING_1MIN_SECONDS = 30;
        const int DURING_ENDED_SECONDS = 10;

        public void Initialize(DateTime dateTime)
        {
            now = dateTime;

            // 満月基準日を 2018/08/01 01:55 とする（wiki より）
            DateTime reference = new DateTime(2018, 8, 1, 1, 55, 0);
            // 現在時刻との差を求める
            TimeSpan span = Now.Subtract(reference);
            // 分に換算
            double min = span.TotalMinutes;
            // 118 で割って切り捨て
            int div = (int)Math.Floor(min / INTERVAL_MINUTES);
            // 最終満月を求める
            LastFullMoon = reference.AddMinutes(INTERVAL_MINUTES * div);

            // タイマー用の設定
            lastMessage = LastFullMoon.AddMinutes(MESSAGE_MINUTES);
            lastSnooze = LastFullMoon.AddMinutes(SNOOZE_MINUTES);
            lastEnded = LastFullMoon.AddMinutes(TEN_MINUTES);
        }

        void CalcMoon()
        {
            // 最終満月 + 118 分を越えたら 1 周したことになる
            if (LastFullMoon.AddMinutes(INTERVAL_MINUTES) <= Now)
            {
                LastFullMoon = LastFullMoon.AddMinutes(INTERVAL_MINUTES);
            }

            #region 月齢 判定（ダサいしｗ）

            if (Now < LastFullMoon.AddMinutes(TEN_MINUTES))
            {
                Age = MoonAges.Full;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES))
            {
                Age = MoonAges.F7N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 2))
            {
                Age = MoonAges.F6N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 3))
            {
                Age = MoonAges.F5N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 4))
            {
                Age = MoonAges.F4N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 5))
            {
                Age = MoonAges.F3N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 6))
            {
                Age = MoonAges.F2N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7))
            {
                Age = MoonAges.F1N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES))
            {
                Age = MoonAges.New;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES))
            {
                Age = MoonAges.N1F;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 2))
            {
                Age = MoonAges.N2F;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 3))
            {
                Age = MoonAges.N3F;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 4))
            {
                Age = MoonAges.N4F;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 5))
            {
                Age = MoonAges.N5F;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 6))
            {
                Age = MoonAges.N6F;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7))
            {
                Age = MoonAges.N7F;
            }
            else
            {
                Age = MoonAges.none;    // エラーのような気がするけどなぁ
            }

            #endregion
        }

        void CalcMessage()
        {
            string hhmm = "yyyyMMddHHmm";

            // 5 分前及び 1 分前の判定

            DateTime before5min =
                LastFullMoon
                    .AddMinutes(INTERVAL_MINUTES)
                    .AddMinutes(-5);

            if (Now.ToString(hhmm) == before5min.ToString(hhmm))
            {
                if (lastMessage.ToString(hhmm) != before5min.ToString(hhmm))
                {
                    lastMessage = Now;
                    IsBefore5min = true;
                }
                else
                {
                    IsBefore5min = false;
                }

                if (Now < before5min.AddSeconds(DURING_5MIN_SECONDS))
                {
                    During5minMessage = true;
                }
                else
                {
                    During5minMessage = false;
                }
            }
            else
            {
                IsBefore5min = false;
                During5minMessage = false;
            }

            DateTime before1min =
                LastFullMoon
                    .AddMinutes(INTERVAL_MINUTES)
                    .AddMinutes(-1);

            if (Now.ToString(hhmm) == before1min.ToString(hhmm))
            {
                if (lastSnooze.ToString(hhmm) != before1min.ToString(hhmm))
                {
                    lastSnooze = Now;
                    IsBefore1min = true;
                }
                else
                {
                    IsBefore1min = false;
                }

                if (Now < before1min.AddSeconds(DURING_1MIN_SECONDS))
                {
                    During1minMessage = true;
                }
                else
                {
                    During1minMessage = false;
                }
            }
            else
            {
                IsBefore1min = false;
                During1minMessage = false;
            }

            // 終了の判定

            DateTime ended =
                LastFullMoon
                    .AddMinutes(TEN_MINUTES);

            if (Now.ToString(hhmm) == ended.ToString(hhmm))
            {
                if (lastEnded.ToString(hhmm) != ended.ToString(hhmm))
                {
                    lastEnded = Now;
                    IsFullmoonEnded = true;
                }
                else
                {
                    IsFullmoonEnded = false;
                }

                if (Now < ended.AddSeconds(DURING_ENDED_SECONDS))
                {
                    DuringEndedMessage = true;
                }
                else
                {
                    DuringEndedMessage = false;
                }
            }
            else
            {
                IsFullmoonEnded = false;
                DuringEndedMessage = false;
            }
        }

        public DateTime LastFullMoon { get; set; }

        private MoonAges age = MoonAges.none;
        public MoonAges Age
        {
            get { return age; }
            set
            {
                IsMoonAgeChanged = age != value;
                age = value;
            }
        }

        private DateTime now;
        public DateTime Now
        {
            get { return now; }
            set
            {
                // システムで時刻合わせをすると盤面とかメッセージのタイミングが狂う
                // プログラム側では秒単位で変化してるはずなので
                // 急に 2 分以上の変化があったら初期化する
                if (Math.Abs(now.Subtract(value).TotalMinutes) > 2)
                {
                    Initialize(value);
                }
                now = value;
                CalcMoon();
                CalcMessage();
            }
        }

        public bool IsMoonAgeChanged { get; set; }

        public bool IsBefore5min { get; set; }
        public bool IsBefore1min { get; set; }
        public bool IsFullmoonEnded { get; set; }

        public bool During5minMessage { get; set; }
        public bool During1minMessage { get; set; }
        public bool DuringEndedMessage { get; set; }
    }
}