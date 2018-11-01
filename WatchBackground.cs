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
using Android.Support.V4.Content.Res;
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
    class WatchBackground
    {
        private Color backColor;
        // 背景用のペイントオブジェクト - filter 用
        private Paint backgroundPaint;

        // 背景用 Bitmap
        #region Bitmap from Resource.Drawable

        readonly Bitmap backgroundFullMoon;
        readonly Bitmap backgroundF7N;
        readonly Bitmap backgroundF6N;
        readonly Bitmap backgroundF5N;
        readonly Bitmap backgroundF4N;
        readonly Bitmap backgroundF3N;
        readonly Bitmap backgroundF2N;
        readonly Bitmap backgroundF1N;
        readonly Bitmap backgroundNewMoon;
        readonly Bitmap backgroundN1F;
        readonly Bitmap backgroundN2F;
        readonly Bitmap backgroundN3F;
        readonly Bitmap backgroundN4F;
        readonly Bitmap backgroundN5F;
        readonly Bitmap backgroundN6F;
        readonly Bitmap backgroundN7F;
        readonly Bitmap backgroundDevilSummoner;
        readonly Bitmap backgroundDx2;
        readonly Bitmap backgroundAmbient;
        readonly Bitmap backgroundTetregrammaton;

        #endregion

        #region Bitmap for Scaled

        private Bitmap backgroundScaledFullMoon;
        private Bitmap backgroundScaledF7N;
        private Bitmap backgroundScaledF6N;
        private Bitmap backgroundScaledF5N;
        private Bitmap backgroundScaledF4N;
        private Bitmap backgroundScaledF3N;
        private Bitmap backgroundScaledF2N;
        private Bitmap backgroundScaledF1N;
        private Bitmap backgroundScaledNewMoon;
        private Bitmap backgroundScaledN1F;
        private Bitmap backgroundScaledN2F;
        private Bitmap backgroundScaledN3F;
        private Bitmap backgroundScaledN4F;
        private Bitmap backgroundScaledN5F;
        private Bitmap backgroundScaledN6F;
        private Bitmap backgroundScaledN7F;
        private Bitmap backgroundScaledDevilSummoner;
        private Bitmap backgroundScaledDx2;
        private Bitmap backgroundScaledAmbient;
        private Bitmap backgroundScaledTetregrammaton;

        #endregion

        // Scaled 用 Bitmap
        //private Bitmap backgroundScaledBitmap;
        //private Bitmap backgroundScaledAmbient;

        public WatchBackground(CanvasWatchFaceService owner)
        {
            // 背景色の作成
            backColor = new Color(
                WatchfaceUtility.ConvertARGBToColor(
                    ContextCompat.GetColor(owner, Resource.Color.background)));

            // 背景用 Paint の作成
            backgroundPaint = new Paint()
            {
                // 背景色を設定
                Color = backColor,
                // Scaled された画像をなめらかにする
                FilterBitmap = true
            };

            #region 'Resources.GetDrawable(int)' は旧形式です('deprecated')

            //// リソースから月齢画像を読み込みます
            //var backgroundDrawableFullMoon = owner.Resources.GetDrawable(Resource.Drawable.FullMoon);
            //var backgroundDrawableF7N = owner.Resources.GetDrawable(Resource.Drawable.F7N);
            //var backgroundDrawableF6N = owner.Resources.GetDrawable(Resource.Drawable.F6N);
            //var backgroundDrawableF5N = owner.Resources.GetDrawable(Resource.Drawable.F5N);
            //var backgroundDrawableF4N = owner.Resources.GetDrawable(Resource.Drawable.F4N);
            //var backgroundDrawableF3N = owner.Resources.GetDrawable(Resource.Drawable.F3N);
            //var backgroundDrawableF2N = owner.Resources.GetDrawable(Resource.Drawable.F2N);
            //var backgroundDrawableF1N = owner.Resources.GetDrawable(Resource.Drawable.F1N);
            //var backgroundDrawableNewMoon = owner.Resources.GetDrawable(Resource.Drawable.NewMoon);
            //var backgroundDrawableN1F = owner.Resources.GetDrawable(Resource.Drawable.N1F);
            //var backgroundDrawableN2F = owner.Resources.GetDrawable(Resource.Drawable.N2F);
            //var backgroundDrawableN3F = owner.Resources.GetDrawable(Resource.Drawable.N3F);
            //var backgroundDrawableN4F = owner.Resources.GetDrawable(Resource.Drawable.N4F);
            //var backgroundDrawableN5F = owner.Resources.GetDrawable(Resource.Drawable.N5F);
            //var backgroundDrawableN6F = owner.Resources.GetDrawable(Resource.Drawable.N6F);
            //var backgroundDrawableN7F = owner.Resources.GetDrawable(Resource.Drawable.N7F);
            //var backgroundDrawableDevilSummoner = owner.Resources.GetDrawable(Resource.Drawable.DevilSummoner);
            //var backgroundDrawableDx2 = owner.Resources.GetDrawable(Resource.Drawable.Dx2);
            ////var backgroundDrawableAmbient = owner.Resources.GetDrawable(Resource.Drawable.Ambient);    //

            //// 月齢画像を Bitmap に変換
            //backgroundFullMoon = (backgroundDrawableFullMoon as BitmapDrawable).Bitmap;
            //backgroundF7N = (backgroundDrawableF7N as BitmapDrawable).Bitmap;
            //backgroundF6N = (backgroundDrawableF6N as BitmapDrawable).Bitmap;
            //backgroundF5N = (backgroundDrawableF5N as BitmapDrawable).Bitmap;
            //backgroundF4N = (backgroundDrawableF4N as BitmapDrawable).Bitmap;
            //backgroundF3N = (backgroundDrawableF3N as BitmapDrawable).Bitmap;
            //backgroundF2N = (backgroundDrawableF2N as BitmapDrawable).Bitmap;
            //backgroundF1N = (backgroundDrawableF1N as BitmapDrawable).Bitmap;
            //backgroundNewMoon = (backgroundDrawableNewMoon as BitmapDrawable).Bitmap;
            //backgroundN1F = (backgroundDrawableN1F as BitmapDrawable).Bitmap;
            //backgroundN2F = (backgroundDrawableN2F as BitmapDrawable).Bitmap;
            //backgroundN3F = (backgroundDrawableN3F as BitmapDrawable).Bitmap;
            //backgroundN4F = (backgroundDrawableN4F as BitmapDrawable).Bitmap;
            //backgroundN5F = (backgroundDrawableN5F as BitmapDrawable).Bitmap;
            //backgroundN6F = (backgroundDrawableN6F as BitmapDrawable).Bitmap;
            //backgroundN7F = (backgroundDrawableN7F as BitmapDrawable).Bitmap;
            //backgroundDevilSummoner = (backgroundDrawableDevilSummoner as BitmapDrawable).Bitmap;
            //backgroundDx2 = (backgroundDrawableDx2 as BitmapDrawable).Bitmap;
            ////backgroundAmbient = (backgroundDrawableAmbient as BitmapDrawable).Bitmap;

            #endregion

            #region Resource から Bitmap を読み込み、Rescale 前の状態で変数にセット

            backgroundFullMoon = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.FullMoon, null)
                as BitmapDrawable).Bitmap;
            backgroundF7N = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.F7N, null)
                as BitmapDrawable).Bitmap;
            backgroundF6N = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.F6N, null)
                as BitmapDrawable).Bitmap;
            backgroundF5N = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.F5N, null)
                as BitmapDrawable).Bitmap;
            backgroundF4N = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.F4N, null)
                as BitmapDrawable).Bitmap;
            backgroundF3N = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.F3N, null)
                as BitmapDrawable).Bitmap;
            backgroundF2N = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.F2N, null)
                as BitmapDrawable).Bitmap;
            backgroundF1N = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.F1N, null)
                as BitmapDrawable).Bitmap;
            backgroundNewMoon = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.NewMoon, null)
                as BitmapDrawable).Bitmap;
            backgroundN1F = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.N1F, null)
                as BitmapDrawable).Bitmap;
            backgroundN2F = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.N2F, null)
                as BitmapDrawable).Bitmap;
            backgroundN3F = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.N3F, null)
                as BitmapDrawable).Bitmap;
            backgroundN4F = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.N4F, null)
                as BitmapDrawable).Bitmap;
            backgroundN5F = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.N5F, null)
                as BitmapDrawable).Bitmap;
            backgroundN6F = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.N6F, null)
                as BitmapDrawable).Bitmap;
            backgroundN7F = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.N7F, null)
                as BitmapDrawable).Bitmap;
            backgroundDevilSummoner = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.DevilSummoner, null)
                as BitmapDrawable).Bitmap;
            backgroundDx2 = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.Dx2, null)
                as BitmapDrawable).Bitmap;
            backgroundAmbient = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.Ambient, null)
                as BitmapDrawable).Bitmap;
            backgroundTetregrammaton = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.TETREGRAMMATON, null)
                as BitmapDrawable).Bitmap;

            #endregion
        }

        public void Rescale(MotoRect rect)
        {
            backgroundScaledFullMoon =
                Bitmap.CreateScaledBitmap(
                    backgroundFullMoon, rect.Width, rect.Height, true);
            backgroundScaledF7N =
                Bitmap.CreateScaledBitmap(
                    backgroundF7N, rect.Width, rect.Height, true);
            backgroundScaledF6N =
                Bitmap.CreateScaledBitmap(
                    backgroundF6N, rect.Width, rect.Height, true);
            backgroundScaledF5N =
                Bitmap.CreateScaledBitmap(
                    backgroundF5N, rect.Width, rect.Height, true);
            backgroundScaledF4N =
                Bitmap.CreateScaledBitmap(
                    backgroundF4N, rect.Width, rect.Height, true);
            backgroundScaledF3N =
                Bitmap.CreateScaledBitmap(
                    backgroundF3N, rect.Width, rect.Height, true);
            backgroundScaledF2N =
                Bitmap.CreateScaledBitmap(
                    backgroundF2N, rect.Width, rect.Height, true);
            backgroundScaledF1N =
                Bitmap.CreateScaledBitmap(
                    backgroundF1N, rect.Width, rect.Height, true);
            backgroundScaledNewMoon =
                Bitmap.CreateScaledBitmap(
                    backgroundNewMoon, rect.Width, rect.Height, true);
            backgroundScaledN1F =
                Bitmap.CreateScaledBitmap(
                    backgroundN1F, rect.Width, rect.Height, true);
            backgroundScaledN2F =
                Bitmap.CreateScaledBitmap(
                    backgroundN2F, rect.Width, rect.Height, true);
            backgroundScaledN3F =
                Bitmap.CreateScaledBitmap(
                    backgroundN3F, rect.Width, rect.Height, true);
            backgroundScaledN4F =
                Bitmap.CreateScaledBitmap(
                    backgroundN4F, rect.Width, rect.Height, true);
            backgroundScaledN5F =
                Bitmap.CreateScaledBitmap(
                    backgroundN5F, rect.Width, rect.Height, true);
            backgroundScaledN6F =
                Bitmap.CreateScaledBitmap(
                    backgroundN6F, rect.Width, rect.Height, true);
            backgroundScaledN7F =
                Bitmap.CreateScaledBitmap(
                    backgroundN7F, rect.Width, rect.Height, true);
            backgroundScaledDevilSummoner =
                Bitmap.CreateScaledBitmap(
                    backgroundDevilSummoner, rect.Width, rect.Height, true);
            backgroundScaledDx2 =
                Bitmap.CreateScaledBitmap(
                    backgroundDx2, rect.Width, rect.Height, true);
            backgroundScaledAmbient =
                Bitmap.CreateScaledBitmap(
                    backgroundAmbient, rect.Width, rect.Height, true);
            backgroundScaledTetregrammaton =
                Bitmap.CreateScaledBitmap(
                    backgroundTetregrammaton, rect.Width, rect.Height, true);
        }

        public void DrawInAmbient(Canvas canvas, MotoRect rect)
        {
            canvas.DrawColor(backColor);
            if (backgroundScaledAmbient == null)
            {
                backgroundScaledAmbient =
                    Bitmap.CreateScaledBitmap(
                        backgroundAmbient, rect.Width, rect.Height, true);
            }
            canvas.DrawBitmap(backgroundScaledAmbient, rect.Left, rect.Top, backgroundPaint);
        }

        public void DrawTetregrammaton(Canvas canvas, MotoRect rect)
        {
            canvas.DrawColor(backColor);
            if (backgroundScaledTetregrammaton == null)
            {
                backgroundScaledTetregrammaton =
                    Bitmap.CreateScaledBitmap(
                        backgroundTetregrammaton, rect.Width, rect.Height, true);
            }
            canvas.DrawBitmap(backgroundScaledTetregrammaton, rect.Left, rect.Top, backgroundPaint);
        }

        public void Draw(Canvas canvas, MotoRect rect)
        {
            canvas.DrawColor(backColor);

            if (IsListMode)
            {
                // 背景を暗くしたら終わり
                // WatchGraph が描いてくれる
                return;
            }

            if (FaceMode == FaceModes.Logo)
            {
                if (backgroundScaledDevilSummoner == null)
                {
                    backgroundScaledDevilSummoner =
                        Bitmap.CreateScaledBitmap(
                            backgroundDevilSummoner, rect.Width, rect.Height, true);
                }
                canvas.DrawBitmap(backgroundScaledDevilSummoner, rect.Left, rect.Top, backgroundPaint);
            }
            else
            {
                switch (MoonAge)
                {
                    case MoonAges.none:
                        if (backgroundScaledDx2 == null)
                        {
                            backgroundScaledDx2 =
                                Bitmap.CreateScaledBitmap(
                                    backgroundDx2, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledDx2, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.Full:
                        if (backgroundScaledFullMoon == null)
                        {
                            backgroundScaledFullMoon =
                                Bitmap.CreateScaledBitmap(
                                    backgroundFullMoon, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledFullMoon, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.F7N:
                        if (backgroundScaledF7N == null)
                        {
                            backgroundScaledF7N =
                                Bitmap.CreateScaledBitmap(
                                    backgroundF7N, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledF7N, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.F6N:
                        if (backgroundScaledF6N == null)
                        {
                            backgroundScaledF6N =
                                Bitmap.CreateScaledBitmap(
                                    backgroundF6N, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledF6N, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.F5N:
                        if (backgroundScaledF5N == null)
                        {
                            backgroundScaledF5N =
                                Bitmap.CreateScaledBitmap(
                                    backgroundF5N, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledF5N, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.F4N:
                        if (backgroundScaledF4N == null)
                        {
                            backgroundScaledF4N =
                                Bitmap.CreateScaledBitmap(
                                    backgroundF4N, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledF4N, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.F3N:
                        if (backgroundScaledF3N == null)
                        {
                            backgroundScaledF3N =
                                Bitmap.CreateScaledBitmap(
                                    backgroundF3N, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledF3N, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.F2N:
                        if (backgroundScaledF2N == null)
                        {
                            backgroundScaledF2N =
                                Bitmap.CreateScaledBitmap(
                                    backgroundF2N, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledF2N, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.F1N:
                        if (backgroundScaledF1N == null)
                        {
                            backgroundScaledF1N =
                                Bitmap.CreateScaledBitmap(
                                    backgroundF1N, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledF1N, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.New:
                        if (backgroundScaledNewMoon == null)
                        {
                            backgroundScaledNewMoon =
                                Bitmap.CreateScaledBitmap(
                                    backgroundNewMoon, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledNewMoon, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.N1F:
                        if (backgroundScaledN1F == null)
                        {
                            backgroundScaledN1F =
                                Bitmap.CreateScaledBitmap(
                                    backgroundN1F, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledN1F, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.N2F:
                        if (backgroundScaledN2F == null)
                        {
                            backgroundScaledN2F =
                                Bitmap.CreateScaledBitmap(
                                    backgroundN2F, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledN2F, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.N3F:
                        if (backgroundScaledN3F == null)
                        {
                            backgroundScaledN3F =
                                Bitmap.CreateScaledBitmap(
                                    backgroundN3F, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledN3F, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.N4F:
                        if (backgroundScaledN4F == null)
                        {
                            backgroundScaledN4F =
                                Bitmap.CreateScaledBitmap(
                                    backgroundN4F, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledN4F, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.N5F:
                        if (backgroundScaledN5F == null)
                        {
                            backgroundScaledN5F =
                                Bitmap.CreateScaledBitmap(
                                    backgroundN5F, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledN5F, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.N6F:
                        if (backgroundScaledN6F == null)
                        {
                            backgroundScaledN6F =
                                Bitmap.CreateScaledBitmap(
                                    backgroundN6F, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledN6F, rect.Left, rect.Top, backgroundPaint);
                        break;
                    case MoonAges.N7F:
                        if (backgroundScaledN7F == null)
                        {
                            backgroundScaledN7F =
                                Bitmap.CreateScaledBitmap(
                                    backgroundN7F, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledN7F, rect.Left, rect.Top, backgroundPaint);
                        break;
                    default:
                        if (backgroundScaledDx2 == null)
                        {
                            backgroundScaledDx2 =
                                Bitmap.CreateScaledBitmap(
                                    backgroundDx2, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(backgroundScaledDx2, rect.Left, rect.Top, backgroundPaint);
                        break;
                }
            }
        }

        // 月齢
        public MoonAges MoonAge { get; set; }

        public enum FaceModes { Logo, Moon }

        // 画面表示モード（ロゴか月齢か）
        public FaceModes FaceMode { get; set; }

        // LowBit Ambient（白黒 2 値）の場合
        public bool FilterBitmap
        {
            get { return backgroundPaint.FilterBitmap; }
            set { backgroundPaint.FilterBitmap = value; }
        }

        // アンビエントモードで動作しているかの判定
        //public bool IsInAmbient { get; set; }

        // リスト表示の判定
        public bool IsListMode { get; set; }
    }
}