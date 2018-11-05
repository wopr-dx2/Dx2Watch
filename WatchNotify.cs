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
    class WatchNotify
    {
        MessageText msgText;
        MessageImage msgImage;

        public WatchNotify(CanvasWatchFaceService owner)
        {
            msgText = new MessageText();
            msgImage = new MessageImage(owner);

            NotifyStyle = NotifyStyles.Text;
            Message = Messages.none;
        }

        public void Rescale(MotoRect rect)
        {
            msgImage.Rescale(rect);
        }

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                switch (NotifyStyle)
                {
                    case NotifyStyles.Text:
                        msgText.Visible = visible;
                        break;
                    case NotifyStyles.Image:
                        msgImage.Visible = visible;
                        break;
                    default:
                        break;
                }
            }
        }

        public void CharSelect(long sec = 3000)
        {
            msgImage.CharSelect(sec);
            if (msgImage.Character == Characters.none)
            {
                NotifyStyle = NotifyStyles.Text;
            }
            else
            {
                NotifyStyle = NotifyStyles.Image;
            }
        }

        public void Draw(Canvas canvas, MotoRect rect)
        {
            switch (NotifyStyle)
            {
                case NotifyStyles.Text:
                    msgText.Draw(canvas, rect);
                    break;
                case NotifyStyles.Image:
                    msgImage.Draw(canvas, rect);
                    break;
                default:
                    break;
            }
        }

        public NotifyStyles NotifyStyle { get; set; }

        private bool antiAlias;
        public bool AntiAlias
        {
            get { return antiAlias; }
            set
            {
                antiAlias = value;
                msgText.AntiAlias = antiAlias;
                msgImage.FilterBitmap = antiAlias;
            }
        }

        private Messages message;
        public Messages Message
        {
            get { return message; }
            set
            {
                message = value;
                msgText.Message = message;
                msgImage.Message = message;
            }
        }

        //public bool IsVisible 
        //    => msgText.Visible | (msgImage.Message != Messages.none & msgImage.Visible);

        public bool IsCharSelecting
             => NotifyStyle == NotifyStyles.Image & msgImage.Message == Messages.none & msgImage.Visible;
    }
}