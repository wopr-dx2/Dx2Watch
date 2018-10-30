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
#if DEBUG
// ログ出力で使用します（デバッグビルドのみ有効）。
using Android.Util;
#endif

using Chronoir_net.Chronica.WatchfaceExtension;

namespace Dx2Watch
{
    class WatchGraph
    {
        const int INTERVAL_MINUTES = 118;   // 満月開始から次の満月まで
        const string FORMAT_MMDD = "MM/dd"; // 年月
        const string FORMAT_HHMM = "HH:mm"; // 時分

        Path path;
        Paint paint;
        readonly RectF rectF;

        // グラフ色（紫っぽい部分）
        readonly Color COLOR_PURPLE = Color.Argb(255, 101, 31, 156);
        // メンテ時間色（グレー）
        readonly Color COLOR_GRAY = Color.Argb(255, 80, 80, 80);

        // 時刻表記
        readonly System.Globalization.CultureInfo ci;

        public WatchGraph()
        {
            path = new Path();
            paint = new Paint();
            rectF = new RectF(40, 40, 280, 280);    // 左上の円弧

            // ja-JP 固定
            ci = new System.Globalization.CultureInfo("ja-JP");
        }

        public void Draw(Canvas canvas, Rect bounds)
        {
            if (!IsListMode)   // 左上の円弧
            {
                if (MoonAge != MoonAges.Full)
                {
                    // init

                    path.Reset();
                    paint.Reset();

                    #region graph purple

                    path.AddArc(rectF, 200, 60);        // 左上 200 度から 60 度分
                    paint.AntiAlias = true;
                    paint.Color = COLOR_PURPLE;
                    paint.SetStyle(Paint.Style.Stroke);
                    paint.StrokeWidth = 20;
                    paint.StrokeCap = Paint.Cap.Butt;
                    canvas.DrawPath(path, paint);

                    #endregion

                    #region graph gray mm = 46～59

                    if (NextFullMoon.Minute >= 46)
                    {
                        path.Reset();

                        float start = 200f + (55 - NextFullMoon.Minute) * 6;
                        if (start < 200)
                        {
                            start = 200;
                        }

                        float sweep = 30;
                        if (NextFullMoon.Minute <= 49)
                        {
                            sweep = (NextFullMoon.Minute - 45) * 6;
                        }
                        else if (NextFullMoon.Minute >= 56)
                        {
                            sweep = (60 - NextFullMoon.Minute) * 6;
                        }

                        path.AddArc(rectF, start, sweep);
                        paint.Color = COLOR_GRAY;
                        canvas.DrawPath(path, paint);
                    }

                    #endregion

                    #region text on graph

                    paint.Reset();
                    paint.AntiAlias = true;
                    paint.Color = Color.White;
                    paint.TextSize = 18;

                    path.Reset();
                    path.AddCircle(180f, 180f, 141f, Path.Direction.Cw);

                    canvas.DrawTextOnPath(
                        NextFullMoon.ToString(FORMAT_HHMM, ci),
                        path, 540, 0, paint);

                    #endregion
                }
            }
            else                        // リスト表示
            {
                paint.Reset();

                paint.AntiAlias = true;
                paint.TextSize = 18;

                int width = bounds.Width();
                float widthF = 0f;
                Rect rect = new Rect();

                DateTime dateTime;

                for (int i = 0; i < 6; i++)
                {
                    dateTime = NextFullMoon.AddMinutes(INTERVAL_MINUTES * i);

                    #region graph purple

                    paint.Color = COLOR_PURPLE;

                    rect.Left = width / 2 - 30;
                    rect.Top = 100 + i * 24;
                    rect.Right = rect.Left + 120;
                    rect.Bottom = rect.Top + 20;

                    canvas.DrawRect(rect, paint);

                    #endregion

                    #region graph gray mm = 46～59

                    if (dateTime.Minute >= 46)
                    {
                        paint.Color = COLOR_GRAY;

                        if (dateTime.Minute <= 54)
                        {
                            rect.Left += (55 - dateTime.Minute) * 12;
                        }

                        if (dateTime.Minute >= 51)
                        {
                            rect.Right -= (dateTime.Minute - 50) * 12;
                        }

                        canvas.DrawRect(rect, paint);

                        // 元に戻す（時間表示のため）
                        rect.Left = width / 2 - 30;
                        rect.Top = 100 + i * 24;
                        rect.Right = rect.Left + 120;
                        rect.Bottom = rect.Top + 20;
                    }

                    #endregion

                    #region text: date

                    paint.Color = Color.White;

                    if (i == 0)
                    {
                        canvas.DrawText(
                            dateTime.ToString(FORMAT_MMDD),
                            rect.Left - 60,
                            rect.Top + rect.Height() - 4, paint);
                    }
                    else
                    {
                        if (dateTime.AddMinutes(INTERVAL_MINUTES * -1).Day != dateTime.Day)
                        {
                            canvas.DrawText(
                                dateTime.ToString(FORMAT_MMDD),
                                rect.Left - 60,
                                rect.Top + rect.Height() - 4, paint);
                        }
                    }

                    #endregion

                    #region text: time

                    widthF = paint.MeasureText(dateTime.ToString(FORMAT_HHMM));
                    canvas.DrawText(
                        dateTime.ToString(FORMAT_HHMM),
                        rect.Left + (rect.Width() - widthF) / 2,
                        rect.Top + rect.Height() - 4, paint);

                    #endregion
                }
            }
        }

        public bool IsListMode { get; set; } = false;

        private DateTime lastFullMoon;
        public DateTime LastFullMoon
        {
            get { return lastFullMoon; }
            set
            {
                lastFullMoon = value;
                NextFullMoon =
                    WatchfaceUtility.ConvertToDateTime(
                        lastFullMoon.AddMinutes(INTERVAL_MINUTES));
            }
        }

        public DateTime NextFullMoon { get; private set; }
        public MoonAges MoonAge { get; set; }
    }
}