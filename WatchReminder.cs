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
    class WatchReminder
    {
        const int INTERVAL_MINUTES = 118;   // 満月開始から次の満月まで

        Path path;
        Paint paint;
        readonly RectF rectF;

        readonly Color COLOR_BLUE = Color.Argb(255, 0, 103, 128);

        public WatchReminder(MotoRect motoRect)
        {
            path = new Path();
            paint = new Paint
            {
                AntiAlias = true,
                Color = COLOR_BLUE,
                StrokeWidth = 10,
                StrokeCap = Paint.Cap.Butt
            };
            paint.SetStyle(Paint.Style.Stroke);
            rectF = new RectF(50, 50, 270, 270);
        }

        public void Draw(Canvas canvas, MotoRect rect)
        {
            DateTime now = WatchfaceUtility.ConvertToDateTime(Calendar);
            int min = (int)NextFullMoon.Subtract(now).TotalMinutes;

            if (min > 40 | min == 0)
            {
                return;
            }

            path.Reset();

            float startAngle = (45 - min) * 6;
            float sweepAngle = min * 6;

            path.AddArc(rectF, startAngle, sweepAngle);

            canvas.DrawPath(path, paint);
        }

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

        public Java.Util.Calendar Calendar { get; set; }
    }
}