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
    class WatchDate
    {
        // 文字書き用 Paint
        Paint paint;

        // 英字表示したい
        readonly System.Globalization.CultureInfo ci;

        public WatchDate()
        {
            paint = new Paint
            {
                Color = Color.White,  // 文字 白
                AntiAlias = true     // アンチエイリアス
            };

            // en-US 固定
            ci = new System.Globalization.CultureInfo("en-US");
        }

        public void Draw(Canvas canvas, MotoRect rect)
        {
            DateTime datetime = WatchfaceUtility.ConvertToDateTime(Calendar);
            string date = "";
            Rect textRect = new Rect();
            int dateHeight = 0;

            int unit = Scale.Unit(rect);

            // d
            //paint.TextSize = 28;
            paint.TextSize = unit * 2.8f;
            date = datetime.ToString("dd", ci);
            paint.GetTextBounds(date, 0, date.Length, textRect);
            dateHeight = textRect.Height();
            canvas.DrawText(date,
                (rect.Width / 4.0f * 3.5f) - (textRect.Width() / 2.0f),
                (rect.Height + textRect.Height()) / 2.0f, paint);

            // ddd
            //paint.TextSize = 16;
            paint.TextSize = unit * 1.6f;
            date = datetime.ToString("ddd", ci).ToUpper();
            paint.GetTextBounds(date, 0, date.Length, textRect);
            //canvas.DrawText(date,
            //    (rect.Width / 4.0f * 3.5f) - (textRect.Width() / 2.0f),
            //    (rect.Height + textRect.Height()) / 2.0f - dateHeight - 10, paint);
            canvas.DrawText(date,
                (rect.Width / 4.0f * 3.5f) - (textRect.Width() / 2.0f),
                (rect.Height + textRect.Height()) / 2.0f - dateHeight - unit, paint);

            // MMM
            //paint.TextSize = 16;
            paint.TextSize = unit * 1.6f;
            date = datetime.ToString("MMM", ci).ToUpper();
            paint.GetTextBounds(date, 0, date.Length, textRect);
            canvas.DrawText(date,
                (rect.Width / 4.0f * 3.5f) - (textRect.Width() / 2.0f),
                (rect.Height + textRect.Height()) / 2.0f + dateHeight + textRect.Height(), paint);
        }

        public Java.Util.Calendar Calendar { get; set; }
    }
}