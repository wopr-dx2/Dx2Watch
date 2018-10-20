﻿using System;

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
    class WatchBalloon
    {
        Paint paint;

        private Bitmap balloonFiveMin;
        private Bitmap balloonOneMin;
        private Bitmap balloonClosed;

        private Bitmap balloonScaledBitmap;

        Handler handler;
        Action action;

        public WatchBalloon(CanvasWatchFaceService owner)
        {
            Balloon = Messages.Before5min;
            hasPost = false;
            visible = false;

            paint = new Paint();
            paint.AntiAlias = true;

            var balloonDrawableFiveMin = owner.Resources.GetDrawable(Resource.Drawable.BalloonBefore5min);
            var balloonDrawableOneMin = owner.Resources.GetDrawable(Resource.Drawable.BalloonBefore1min);
            var balloonDrawableClosed = owner.Resources.GetDrawable(Resource.Drawable.BalloonEnded);

            balloonFiveMin = (balloonDrawableFiveMin as BitmapDrawable).Bitmap;
            balloonOneMin = (balloonDrawableOneMin as BitmapDrawable).Bitmap;
            balloonClosed = (balloonDrawableClosed as BitmapDrawable).Bitmap;

            handler = new Handler();
            action = callback;
        }

        void callback()
        {
            visible = false;
        }

        public void Show(long sec = 3000)
        {
            seconds = sec;

            Cancel();

            if (!visible)
            {
                visible = true;
                hasPost = true;
            }
        }

        public void Cancel()
        {
            if (visible)
            {
                handler.RemoveCallbacksAndMessages(null);
                visible = false;
            }
        }

        public void Draw(Canvas canvas, Rect bounds)
        {
            if (!visible)
            {
                return;
            }

            int width = bounds.Width();
            int height = bounds.Height();

            if (mustRescaled)
            {
                switch (Balloon)
                {
                    case Messages.Before1min:
                        balloonScaledBitmap =
                            Bitmap.CreateScaledBitmap(balloonOneMin, width, height, true);
                        break;
                    case Messages.Before5min:
                        balloonScaledBitmap =
                            Bitmap.CreateScaledBitmap(balloonFiveMin, width, height, true);
                        break;
                    case Messages.Ended:
                        balloonScaledBitmap =
                            Bitmap.CreateScaledBitmap(balloonClosed, width, height, true);
                        break;
                    default:
                        break;
                }
            }

            canvas.DrawBitmap(balloonScaledBitmap, 0, 0, paint);

            if (hasPost)
            {
                handler.PostDelayed(action, seconds);
                hasPost = false;
            }
        }

        private Messages balloon = Messages.Before5min;
        public Messages Balloon
        {
            get { return balloon; }
            set
            {
                mustRescaled = balloon != value;
                balloon = value;
            }
        }

        long seconds;

        bool hasPost;

        bool visible;

        bool mustRescaled = true;
    }
}