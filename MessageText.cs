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
    class MessageText
    {
        Paint paint;

        const string MSG_ONE = "1 min later";
        const string MSG_FIVE = "5 min later";
        const string MSG_CLOSED = "Closed...";

        Handler handler;
        Action action;

        public MessageText()
        {
            Message = Messages.Before5min;
            hasPost = false;
            Visible = false;

            paint = new Paint();

            paint.AntiAlias = true;
            paint.Color = Color.White;
            paint.TextSize = 18;

            handler = new Handler();
            action = () => { Visible = false; };
        }

        //void callback()
        //{
        //    visible = false;
        //}

        public void Show(long sec = 3000)
        {
            seconds = sec;

            Cancel();

            if (!Visible)
            {
                Visible = true;
                hasPost = true;
            }
        }

        public void Cancel()
        {
            if (Visible)
            {
                handler.RemoveCallbacksAndMessages(null);
                Visible = false;
            }
        }

        public void Draw(Canvas canvas, Rect bounds)
        {
            if (!Visible)
            {
                return;
            }

            int width = bounds.Width();
            int height = bounds.Height();

            string text = "";
            switch (Message)
            {
                case Messages.Before1min:
                    text = MSG_ONE;
                    break;
                case Messages.Before5min:
                    text = MSG_FIVE;
                    break;
                case Messages.Ended:
                    text = MSG_CLOSED;
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                float textWidth = paint.MeasureText(text);
                canvas.DrawText(text, (width - textWidth) / 2.0f, 270, paint);

                if (hasPost)
                {
                    handler.PostDelayed(action, seconds);
                    hasPost = false;
                }
            }
        }

        public Messages Message { get; set; }

        private long seconds;

        private bool hasPost;

        public bool Visible { get; set; }
    }
}