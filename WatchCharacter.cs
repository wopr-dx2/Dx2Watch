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
    class WatchCharacter
    {
        Paint paint;

        private Bitmap charPlayer;
        private Bitmap charTemplarDragon;
        private Bitmap charEileen;
        private Bitmap charShionyan;

        private Bitmap charScaledBitmap;

        Handler handler;
        Action action;

        public WatchCharacter(CanvasWatchFaceService owner)
        {
            Character = Characters.none;
            hasPost = false;
            visible = false;

            paint = new Paint();
            paint.AntiAlias = true;

            var charDrawablePlayer = owner.Resources.GetDrawable(Resource.Drawable.CharPlayer);
            var charDrawableTemplarDragon = owner.Resources.GetDrawable(Resource.Drawable.CharTemplarDragon);
            var charDrawableEileen = owner.Resources.GetDrawable(Resource.Drawable.CharEileen);
            var charDrawableShionyan = owner.Resources.GetDrawable(Resource.Drawable.CharShionyan);

            charPlayer = (charDrawablePlayer as BitmapDrawable).Bitmap;
            charTemplarDragon = (charDrawableTemplarDragon as BitmapDrawable).Bitmap;
            charEileen = (charDrawableEileen as BitmapDrawable).Bitmap;
            charShionyan = (charDrawableShionyan as BitmapDrawable).Bitmap;

            handler = new Handler();
            action = () => { visible = false; };
        }

        //void callback()
        //{
        //    visible = false;
        //}

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

            if (Character != Characters.none)
            {
                if (mustRescaled)
                {
                    switch (Character)
                    {
                        case Characters.Player:
                            charScaledBitmap =
                                Bitmap.CreateScaledBitmap(charPlayer, width, height, true);
                            break;
                        case Characters.TemplarDragon:
                            charScaledBitmap =
                                Bitmap.CreateScaledBitmap(charTemplarDragon, width, height, true);
                            break;
                        case Characters.Eileen:
                            charScaledBitmap =
                                Bitmap.CreateScaledBitmap(charEileen, width, height, true);
                            break;
                        case Characters.Shionyan:
                            charScaledBitmap =
                                Bitmap.CreateScaledBitmap(charShionyan, width, height, true);
                            break;
                        default:
                            break;
                    }
                }

                canvas.DrawBitmap(charScaledBitmap, 0, 0, paint);

                if (hasPost)
                {
                    handler.PostDelayed(action, seconds);
                    hasPost = false;
                }
            }
        }

        public void CharSelect(long sec = 3000)
        {
            if (visible)
            {
                ModeNext(sec);
            }
            else
            {
                mustRescaled = true;
                Show(sec);
            }
        }

        public void ModeNext(long sec = 3000)
        {
            switch (Character)
            {
                case Characters.Player:
                    Character = Characters.TemplarDragon;
                    break;
                case Characters.TemplarDragon:
                    Character = Characters.Eileen;
                    break;
                case Characters.Eileen:
                    Character = Characters.Shionyan;
                    break;
                case Characters.Shionyan:
                    Character = Characters.none;
                    break;
                case Characters.none:
                    Character = Characters.Player;
                    break;
                default:
                    Character = Characters.none;
                    break;
            }

            Show(sec);
        }

        public enum Characters { Player, TemplarDragon, Eileen, Shionyan, none }

        private Characters character = Characters.none;
        public Characters Character
        {
            get { return character; }
            set
            {
                mustRescaled = character != value;
                character = value;
            }
        }

        long seconds;

        bool hasPost;

        bool visible;

        bool mustRescaled = true;
    }
}