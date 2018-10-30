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
    class WatchHands
    {
        // 時針、分針、秒針用のオブジェクトを表します。
        private Paint hourHandPaint;
        private Paint minuteHandPaint;
        private Paint secondHandPaint;

        // 縁用
        private Paint hourHandBackPaint;
        private Paint minuteHandBackPaint;
        private Paint secondHandBackPaint;

        public WatchHands(CanvasWatchFaceService owner)
        {
            var resources = owner.Resources;

            // 時針用のPaintグラフィックスオブジェクトを生成します。
            hourHandPaint = new Paint
            {
                // 時針の色を設定します
                Color =
                    WatchfaceUtility.ConvertARGBToColor(
                        ContextCompat.GetColor(owner, Resource.Color.analog_hands)),
                // 時針の幅を設定します。
                StrokeWidth = resources.GetDimension(Resource.Dimension.hour_hand_stroke),
                // アンチエイリアスを有効にします。
                AntiAlias = true,
                // 線端の形は丸形を指定します。
                StrokeCap = Paint.Cap.Round
            };

            // 時針の縁を準備します
            hourHandBackPaint = new Paint
            {
                Color = Color.Black,
                StrokeWidth = hourHandPaint.StrokeWidth + 3,
                AntiAlias = hourHandPaint.AntiAlias,
                StrokeCap = hourHandPaint.StrokeCap
            };

            // 分針用のPaintグラフィックスオブジェクトを生成します。
            minuteHandPaint = new Paint
            {
                Color = hourHandPaint.Color,
                StrokeWidth = resources.GetDimension(Resource.Dimension.minute_hand_stroke),
                AntiAlias = true,
                StrokeCap = Paint.Cap.Round
            };

            // 分針の縁を準備します
            minuteHandBackPaint = new Paint
            {
                Color = hourHandBackPaint.Color,
                StrokeWidth = minuteHandPaint.StrokeWidth + 3,
                AntiAlias = minuteHandPaint.AntiAlias,
                StrokeCap = minuteHandPaint.StrokeCap
            };

            // 秒針用のPaintグラフィックスオブジェクトを生成します。
            secondHandPaint = new Paint
            {
                Color =
                    WatchfaceUtility.ConvertARGBToColor(
                        ContextCompat.GetColor(owner, Resource.Color.analog_second_hand)),
                StrokeWidth = resources.GetDimension(Resource.Dimension.second_hand_stroke),
                AntiAlias = true,
                StrokeCap = Paint.Cap.Round
            };

            // 秒針の縁を準備します
            secondHandBackPaint = new Paint
            {
                Color = Color.Black,
                StrokeWidth = secondHandPaint.StrokeWidth + 2,
                AntiAlias = secondHandPaint.AntiAlias,
                StrokeCap = secondHandPaint.StrokeCap
            };
        }

        public void DrawBorder(Canvas canvas, MotoRect rect)
        {
            // 中心からの距離
            float margin = 20.0f;

            // 中心のXY座標を求めます。
            float centerX = rect.Width / 2.0f;
            float centerY = rect.Height / 2.0f;

            // 時針の縁を描画します
            // X, Y → 先端の座標 / Xc, Yc → 中心側の座標
            float hourHandBackLength = centerX - 80;
            float hourHandBackRotation = ((Calendar.Get(Java.Util.CalendarField.Hour) + (Calendar.Get(Java.Util.CalendarField.Minute) / 60f)) / 6f) * (float)Math.PI;
            float hourHandBackX = (float)Math.Sin(hourHandBackRotation) * hourHandBackLength;
            float hourHandBackY = (float)-Math.Cos(hourHandBackRotation) * hourHandBackLength;
            float hourHandBackXc = (float)Math.Sin(hourHandBackRotation) * margin;
            float hourHandBackYc = (float)-Math.Cos(hourHandBackRotation) * margin;
            canvas.DrawLine(
                centerX + hourHandBackXc, centerY + hourHandBackYc,
                centerX + hourHandBackX, centerY + hourHandBackY, hourHandBackPaint);

            // 分針の縁を描画します
            // X, Y → 先端の座標 / Xc, Yc → 中心側の座標
            float minuteHandBackLength = centerX - 40;
            float minuteHandBackRotation = Calendar.Get(Java.Util.CalendarField.Minute) / 30f * (float)Math.PI;
            float minuteHandBackX = (float)Math.Sin(minuteHandBackRotation) * minuteHandBackLength;
            float minuteHandBackY = (float)-Math.Cos(minuteHandBackRotation) * minuteHandBackLength;
            float minuteHandBackXc = (float)Math.Sin(minuteHandBackRotation) * margin;
            float minuteHandBackYc = (float)-Math.Cos(minuteHandBackRotation) * margin;
            canvas.DrawLine(
                centerX + minuteHandBackXc, centerY + minuteHandBackYc,
                centerX + minuteHandBackX, centerY + minuteHandBackY, minuteHandBackPaint);
        }

        public void DrawHands(Canvas canvas, MotoRect rect)
        {
            // 中心からの距離
            float margin = 20.0f;

            // 中心のXY座標を求めます。
            float centerX = rect.Width / 2.0f;
            float centerY = rect.Height / 2.0f;

            // 針の長さを求めます。
            float hourHandLength = centerX - 80;
            float minuteHandLength = centerX - 40;

            // 時針の先端のXY座標を求めます
            // X, Y → 先端の座標 / Xc, Yc → 中心側の座標
            float hourHandRotation = ((Calendar.Get(Java.Util.CalendarField.Hour) + (Calendar.Get(Java.Util.CalendarField.Minute) / 60f)) / 6f) * (float)Math.PI;
            float hourHandX = (float)Math.Sin(hourHandRotation) * hourHandLength;
            float hourHandY = (float)-Math.Cos(hourHandRotation) * hourHandLength;
            float hourHandXc = (float)Math.Sin(hourHandRotation) * margin;
            float hourHandYc = (float)-Math.Cos(hourHandRotation) * margin;
            // 時針を描画します。
            canvas.DrawLine(
                centerX + hourHandXc, centerY + hourHandYc,
                centerX + hourHandX, centerY + hourHandY, hourHandPaint);

            // 分針の先端のXY座標を求めます。
            // X, Y → 先端の座標 / Xc, Yc → 中心側の座標
            float minuteHandRotation = Calendar.Get(Java.Util.CalendarField.Minute) / 30f * (float)Math.PI;
            float minuteHandX = (float)Math.Sin(minuteHandRotation) * minuteHandLength;
            float minuteHandY = (float)-Math.Cos(minuteHandRotation) * minuteHandLength;
            float minuteHandXc = (float)Math.Sin(minuteHandRotation) * margin;
            float minuteHandYc = (float)-Math.Cos(minuteHandRotation) * margin;
            // 分針を描画します。
            canvas.DrawLine(
                centerX + minuteHandXc, centerY + minuteHandYc,
                centerX + minuteHandX, centerY + minuteHandY, minuteHandPaint);
        }


        public void DrawSec(Canvas canvas, MotoRect rect)
        {
            // 中心からの距離
            float margin = 20.0f;

            // 中心のXY座標を求めます。
            float centerX = rect.Width / 2.0f;
            float centerY = rect.Height / 2.0f;

            // 針の長さを求めます。
            float secondHandLength = centerX - 20;

            // 秒針の縁を描画します
            float secondHandBackLength = centerX - 20;
            float secondHandBackRotation = Calendar.Get(Java.Util.CalendarField.Second) / 30f * (float)Math.PI;
            float secondHandBackX = (float)Math.Sin(secondHandBackRotation) * secondHandBackLength;
            float secondHandBackY = (float)-Math.Cos(secondHandBackRotation) * secondHandBackLength;
            float secondHandBackXc = (float)Math.Sin(secondHandBackRotation) * (secondHandBackLength - margin);
            float secondHandBackYc = (float)-Math.Cos(secondHandBackRotation) * (secondHandBackLength - margin);
            canvas.DrawLine(
                centerX + secondHandBackXc, centerY + secondHandBackYc,
                centerX + secondHandBackX, centerY + secondHandBackY, secondHandBackPaint);

            // 秒針の先端のXY座標を求めます。
            float secondHandRotation = Calendar.Get(Java.Util.CalendarField.Second) / 30f * (float)Math.PI;
            float secondHandX = (float)Math.Sin(secondHandRotation) * secondHandLength;
            float secondHandY = (float)-Math.Cos(secondHandRotation) * secondHandLength;
            float secondHandXc = (float)Math.Sin(secondHandRotation) * (secondHandLength - margin);
            float secondHandYc = (float)-Math.Cos(secondHandRotation) * (secondHandLength - margin);
            // 秒針を描画します。
            canvas.DrawLine(
                centerX + secondHandXc, centerY + secondHandYc,
                centerX + secondHandX, centerY + secondHandY, secondHandPaint);
        }

        public Java.Util.Calendar Calendar { get; set; }

        private bool antiAlias = false;
        public bool AntiAlias
        {
            get { return antiAlias; }
            set
            {
                antiAlias = value;

                hourHandPaint.AntiAlias = !antiAlias;
                minuteHandPaint.AntiAlias = !antiAlias;
                secondHandPaint.AntiAlias = !antiAlias;

                hourHandBackPaint.AntiAlias = !antiAlias;
                minuteHandBackPaint.AntiAlias = !antiAlias;
                secondHandBackPaint.AntiAlias = !antiAlias;
            }
        }
    }
}