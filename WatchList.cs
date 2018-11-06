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
    class WatchList
    {
        const int INTERVAL_MINUTES = 118;
        const string FORMAT_MMDD = "MM/dd"; // 年月
        const string FORMAT_HHMM = "HH:mm";

        readonly Paint listPaint;
        readonly Paint listGrayPaint;
        readonly Paint listTextPaint;
        readonly Rect[] listRects;

        float dateLeft;
        float timeLeft;

        Path scalePath;
        Path scaleGrayPath;
        Path scaleTextPath;
        Path reminderPath;

        Paint scalePaint;
        Paint scaleGrayPaint;
        Paint scaleTextPaint;
        Paint reminderPaint;

        RectF scaleRectF;
        RectF scaleTextRectF;
        RectF reminderRectF;

        readonly int scaleStartAngle = 210;
        readonly int scaleSweepAngle = 60;

        // グラフ色（紫っぽい部分）
        readonly Color COLOR_PURPLE = Color.Argb(255, 101, 31, 156);
        // メンテ時間色（グレー）
        readonly Color COLOR_GRAY = Color.Argb(255, 80, 80, 80);
        // リマインダー色（青っぽい）
        readonly Color COLOR_BLUE = Color.Argb(255, 0, 103, 128);

        // 時刻表記
        readonly System.Globalization.CultureInfo ci;

        public WatchList()
        {
            scalePath = new Path();
            scaleGrayPath = new Path();
            scaleTextPath = new Path();
            reminderPath = new Path();

            scalePaint = new Paint()
            {
                AntiAlias = true,
                Color = COLOR_PURPLE,
                StrokeCap = Paint.Cap.Butt
            };
            scalePaint.SetStyle(Paint.Style.Stroke);

            scaleGrayPaint = new Paint()
            {
                AntiAlias = true,
                Color = COLOR_GRAY,
                StrokeCap = Paint.Cap.Butt
            };
            scaleGrayPaint.SetStyle(Paint.Style.Stroke);

            scaleTextPaint = new Paint()
            {
                AntiAlias = true,
                Color = Color.White
            };

            reminderPaint = new Paint()
            {
                AntiAlias = true,
                Color = COLOR_BLUE,
                StrokeCap = Paint.Cap.Butt
            };
            reminderPaint.SetStyle(Paint.Style.Stroke);

            listPaint = new Paint()
            {
                AntiAlias = true,
                Color = COLOR_PURPLE
            };

            listGrayPaint = new Paint()
            {
                AntiAlias = true,
                Color = COLOR_GRAY
            };

            listTextPaint = new Paint()
            {
                AntiAlias = true,
                Color = Color.White
            };

            listRects = new Rect[6];

            // ja-JP 固定
            ci = new System.Globalization.CultureInfo("ja-JP");
        }

        public void Rescale(MotoRect motoRect)
        {
            //int imageWidth = 360;
            //int moonWidth = 200;

            //int scaledMoonWidth = motoRect.Width * moonWidth / imageWidth;
            //int leftMargin = (motoRect.Width - scaledMoonWidth) / 2;

            //int unit = leftMargin / 7;
            int unit = Scale.Unit(motoRect);

            int margin = 3;

            // リスト
            for (int i = 0; i < 6; i++)
            {
                //listRects[i].Left = unit * 13;
                //listRects[i].Top = ((unit * 10) + i * (int)(unit * 2.4f));
                //listRects[i].Right = listRects[i].Left + unit * 12;
                //listRects[i].Bottom = listRects[i].Top + unit * 2;
                listRects[i] = new Rect(
                    unit * 13,
                    (unit * 10) + i * (int)(unit * 2.4f),
                    unit * 13 + unit * 12,
                    ((unit * 10) + i * (int)(unit * 2.4f)) + unit * 2);
            }

            listTextPaint.TextSize = (float)unit * 2f * 0.9f;
            float textWidth = listTextPaint.MeasureText("00:00");

            dateLeft = unit * 7;
            timeLeft = listRects[0].CenterX() - (int)(textWidth / 2f);

            // スケール
            scaleRectF = new RectF(unit * 3, unit * 3,
                motoRect.Right - unit * 3, motoRect.Bottom - unit * 3);

            scalePath.AddArc(scaleRectF, scaleStartAngle, scaleSweepAngle);
            scalePaint.StrokeWidth = unit * 2;

            // テキスト
            scaleTextRectF = new RectF(unit * 4 - margin, unit * 4 - margin,
                motoRect.Right - unit * 4 + margin, motoRect.Bottom - unit * 4 + margin);

            scaleTextPaint.TextSize = listTextPaint.TextSize;

            float textSweep =
                360.0f * textWidth / ((float)Math.PI * scaleTextRectF.Width());
            //360.0f * textWidth / (2.0f * (float)Math.PI * scaleTextRectF.Width());
            float textStart =
                scaleStartAngle + (scaleSweepAngle / 2) - textSweep / 2;

            scaleTextPath.AddArc(scaleTextRectF, textStart, textSweep);

            // リマインダ
            reminderRectF = new RectF(unit * 4.5f, unit * 4.5f,
                motoRect.Right - unit * 4.5f, motoRect.Bottom - unit * 4.5f);

            reminderPaint.StrokeWidth = unit;
        }

        public void Draw(Canvas canvas, MotoRect rect)
        {
            if (IsListMode)
            {
                DrawList(canvas, rect);
            }
            else
            {
                DrawScale(canvas, rect);
                DrawReminder(canvas, rect);
            }
        }

        void DrawList(Canvas canvas, MotoRect rect)
        {
            Rect grayRect;
            DateTime dateTime;

            for (int i = 0; i < 6; i++)
            {
                dateTime = NextFullMoon.AddMinutes(INTERVAL_MINUTES * i);

                canvas.DrawRect(listRects[i], listPaint);

                #region gray mm = 46～59

                if (dateTime.Minute >= 46)
                {
                    grayRect = new Rect(listRects[i]);

                    if (dateTime.Minute <= 54)
                    {
                        grayRect.Left +=
                            (55 - dateTime.Minute) * (listRects[i].Width() / 10);
                    }

                    if (dateTime.Minute >= 51)
                    {
                        grayRect.Right -=
                            (dateTime.Minute - 50) * (listRects[i].Width() / 10);
                    }

                    canvas.DrawRect(grayRect, listGrayPaint);
                }

                #endregion

                int margin = 3;
                int y = listRects[i].Top + listRects[i].Height() - margin;

                //if (i == 0)
                //{
                //    canvas.DrawText(
                //        dateTime.ToString(FORMAT_MMDD),
                //        dateLeft, y, listTextPaint);
                //}
                //else
                //{
                //    if (dateTime.AddMinutes(INTERVAL_MINUTES * -1).Day != dateTime.Day)
                //    {
                //        canvas.DrawText(
                //            dateTime.ToString(FORMAT_MMDD),
                //            dateLeft, y, listTextPaint);
                //    }
                //}
                if (i == 0 |
                    i != 0 & dateTime.AddMinutes(INTERVAL_MINUTES * -1).Day != dateTime.Day)
                {
                    canvas.DrawText(
                        dateTime.ToString(FORMAT_MMDD),
                        dateLeft, y, listTextPaint);
                }

                canvas.DrawText(
                    dateTime.ToString(FORMAT_HHMM),
                    timeLeft, y, listTextPaint);
            }
        }

        void DrawScale(Canvas canvas, MotoRect rect)
        {
            //if (MoonAge == MoonAges.Full & FaceStyle == FaceStyles.Moon)
            if (MoonAge == MoonAges.Full)
            {
                return;
            }

            canvas.DrawPath(scalePath, scalePaint);

            #region graph gray mm = 46～59

            int minute = NextFullMoon.Minute;

            if (minute >= 46)
            {
                scaleGrayPath.Reset();

                float startAngle = scaleStartAngle + (55 - minute) * 6;
                if (startAngle < scaleStartAngle)
                {
                    startAngle = scaleStartAngle;
                }

                float sweepAngle = 30;
                if (minute <= 49)
                {
                    sweepAngle = (minute - 45) * 6;
                }
                else if (minute >= 56)
                {
                    sweepAngle = (60 - minute) * 6;
                }

                scaleGrayPath.AddArc(scaleRectF, startAngle, sweepAngle);
                canvas.DrawPath(scaleGrayPath, scaleGrayPaint);
            }

            #endregion

            canvas.DrawTextOnPath(
                NextFullMoon.ToString(FORMAT_HHMM, ci),
                scaleTextPath, 0, 0, scaleTextPaint);
        }

        void DrawReminder(Canvas canvas, MotoRect rect)
        {
            DateTime now = WatchfaceUtility.ConvertToDateTime(Calendar);
            int totalMins = (int)NextFullMoon.Subtract(now).TotalMinutes;

            if (totalMins > 40 | totalMins < 0)
            {
                return;
            }

            reminderPath.Reset();

            //float startAngle = (45 - min) * 6;
            //float sweepAngle = min * 6;

            int nowMin = now.Minute;
            int nextMin = NextFullMoon.Minute;

            float startAngle = 0f;
            float sweepAngle = 0f;

            if (nowMin < 15 & nextMin <= 15)
            {
                startAngle = 270f + nowMin * 6f;
                sweepAngle = (nextMin - nowMin) * 6f;

                reminderPath.AddArc(reminderRectF, startAngle, sweepAngle);
            }
            else if (nowMin < 15)
            {
                startAngle = 270f + nowMin * 6f;
                sweepAngle = 360f - startAngle;

                reminderPath.AddArc(reminderRectF, startAngle, sweepAngle);

                startAngle = 0f;
                sweepAngle = (nextMin - 15) * 6f;

                reminderPath.AddArc(reminderRectF, startAngle, sweepAngle);
            }
            else if (nextMin <= 15)
            {
                startAngle = (nowMin - 15) * 6f;
                sweepAngle = 270f - startAngle;

                reminderPath.AddArc(reminderRectF, startAngle, sweepAngle);

                startAngle = 270f;
                sweepAngle = nextMin * 6f;

                reminderPath.AddArc(reminderRectF, startAngle, sweepAngle);
            }
            else if (nextMin < nowMin)
            {
                startAngle = (nowMin - 15) * 6f;
                sweepAngle = 360f - startAngle;

                reminderPath.AddArc(reminderRectF, startAngle, sweepAngle);

                startAngle = 0f;
                sweepAngle = (nextMin - 15) * 6f;

                reminderPath.AddArc(reminderRectF, startAngle, sweepAngle);
            }
            else
            {
                startAngle = (nowMin - 15) * 6f;
                sweepAngle = (nextMin - nowMin) * 6f;

                reminderPath.AddArc(reminderRectF, startAngle, sweepAngle);
            }

            //float startAngle = (nextMin * 6f) + 15f;
            //float sweepAngle = totalMins * 6;

            //reminderPath.AddArc(reminderRectF, startAngle, sweepAngle);

            canvas.DrawPath(reminderPath, reminderPaint);
        }

        public bool IsListMode { get; set; }

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
        public FaceStyles FaceStyle { get; set; }
        public Java.Util.Calendar Calendar { get; set; }
    }
}