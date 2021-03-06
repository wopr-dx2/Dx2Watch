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
//#if DEBUG
//// ログ出力で使用します（デバッグビルドのみ有効）。
//using Android.Util;
//#endif

using Chronoir_net.Chronica.WatchfaceExtension;

namespace Dx2Watch
{
    class MotoRect
    {
        //bool isInitialized = false;

        public MotoRect(Rect r)
            : this(r.Left, r.Top, r.Right, r.Bottom)
        {
        }

        public MotoRect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;

            Center = new Point((right - left) / 2, (bottom - top) / 2);
        }

        public void SetBounds(Rect bounds)
        {
            if (Left != bounds.Left)
            {
                Left = bounds.Left;
                IsSizeChanged = true;
            }

            if (Top != bounds.Top)
            {
                Top = bounds.Top;
                IsSizeChanged = true;
            }

            if (Right != bounds.Right)
            {
                Right = bounds.Right;
                IsSizeChanged = true;
            }

            if (IsMoto360)
            {
                if (Bottom != Right)
                {
                    Bottom = Right;
                    IsSizeChanged = true;
                }
            }
            else
            {
                if (Bottom != bounds.Bottom)
                {
                    Bottom = bounds.Bottom;
                    IsSizeChanged = true;
                }
            }

            Center.X = Width / 2;
            Center.Y = Height / 2;
        }

        public Rect ToRect()
        {
            return new Rect(Left, Top, Right, Bottom);
        }

        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public int Width => Right - Left;
        public int Height => Bottom - Top;

        public Point Center { get; private set; }

        public bool IsMoto360 { get; set; } = true;

        public bool IsSizeChanged { get; set; }
    }
}