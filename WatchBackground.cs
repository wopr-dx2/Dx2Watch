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
#if DEBUG
// ログ出力で使用します（デバッグビルドのみ有効）。
using Android.Util;
#endif

using Chronoir_net.Chronica.WatchfaceExtension;

namespace Dx2Watch
{
    class WatchBackground
    {
        // 背景用のペイントオブジェクト - Ambient 用
        private Paint backgroundPaint;

    　　// 背景用 Bitmap
        private Bitmap backgroundFullMoon;
        private Bitmap backgroundF7N;
        private Bitmap backgroundF6N;
        private Bitmap backgroundF5N;
        private Bitmap backgroundF4N;
        private Bitmap backgroundF3N;
        private Bitmap backgroundF2N;
        private Bitmap backgroundF1N;
        private Bitmap backgroundNewMoon;
        private Bitmap backgroundN1F;
        private Bitmap backgroundN2F;
        private Bitmap backgroundN3F;
        private Bitmap backgroundN4F;
        private Bitmap backgroundN5F;
        private Bitmap backgroundN6F;
        private Bitmap backgroundN7F;
        private Bitmap backgroundDevilSummoner;
        private Bitmap backgroundDx2;
        private Bitmap backgroundAmbient;         // Ambient はとりあえず黒で

        // Scaled 用 Bitmap
        private Bitmap backgroundScaledBitmap;
        private Bitmap backgroundScaledAmbient;   // Ambient はとりあえず黒で

        // 書き換え必須フラグ
        bool mustRescaled = true;

        public WatchBackground(CanvasWatchFaceService owner)
        {
            // 背景用 Paint の作成
            backgroundPaint = new Paint
            {
                // リソースから背景色を読み込みます
                Color =
                WatchfaceUtility.ConvertARGBToColor(
                    ContextCompat.GetColor(owner, Resource.Color.background))
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

            #endregion
        }

        public void Draw(Canvas canvas, Rect bounds)
        {
            int width = bounds.Width();
            int height = bounds.Height();

            // アンビエントモード
            if (isInAmbientMode)
            {
                // 黒で塗りつぶして終了
                //canvas.DrawColor(Color.Black);

                // 背景描画
                if (mustRescaled)
                {
                    backgroundScaledAmbient =
                        Bitmap.CreateScaledBitmap(backgroundScaledAmbient, width, height, true);

                    mustRescaled = false;
                }

                canvas.DrawBitmap(backgroundScaledAmbient, 0, 0, null);
            }
            else if (IsListMode)    // ListMode
            {
                // 黒で塗りつぶす
                canvas.DrawColor(Color.Black);
            }
            else
            {
                if (mustRescaled)
                {
                    if (faceMode == FaceModes.Logo)
                    {
                        //backgroundScaledBitmap =
                        //    Bitmap.CreateScaledBitmap(backgroundDx2, width, height, true);
                        backgroundScaledBitmap =
                            Bitmap.CreateScaledBitmap(backgroundDevilSummoner, width, height, true);
                    }
                    else
                    {
                        switch (MoonAge)
                        {
                            case MoonAges.none:
                                //backgroundScaledBitmap =
                                //    Bitmap.CreateScaledBitmap(backgroundDevilSummoner, width, height, true);
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundDx2, width, height, true);
                                break;
                            case MoonAges.Full:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundFullMoon, width, height, true);
                                break;
                            case MoonAges.F7N:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundF7N, width, height, true);
                                break;
                            case MoonAges.F6N:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundF6N, width, height, true);
                                break;
                            case MoonAges.F5N:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundF5N, width, height, true);
                                break;
                            case MoonAges.F4N:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundF4N, width, height, true);
                                break;
                            case MoonAges.F3N:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundF3N, width, height, true);
                                break;
                            case MoonAges.F2N:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundF2N, width, height, true);
                                break;
                            case MoonAges.F1N:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundF1N, width, height, true);
                                break;
                            case MoonAges.New:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundNewMoon, width, height, true);
                                break;
                            case MoonAges.N1F:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundN1F, width, height, true);
                                break;
                            case MoonAges.N2F:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundN2F, width, height, true);
                                break;
                            case MoonAges.N3F:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundN3F, width, height, true);
                                break;
                            case MoonAges.N4F:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundN4F, width, height, true);
                                break;
                            case MoonAges.N5F:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundN5F, width, height, true);
                                break;
                            case MoonAges.N6F:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundN6F, width, height, true);
                                break;
                            case MoonAges.N7F:
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundN7F, width, height, true);
                                break;
                            default:
                                //backgroundScaledBitmap =
                                //    Bitmap.CreateScaledBitmap(backgroundDevilSummoner, width, height, true);
                                backgroundScaledBitmap =
                                    Bitmap.CreateScaledBitmap(backgroundDx2, width, height, true);
                                break;
                        }
                    }

                    mustRescaled = false;
                }

                if (backgroundScaledBitmap == null
                    || backgroundScaledBitmap.Width != width
                    || backgroundScaledBitmap.Height != height)
                {
                    backgroundScaledBitmap =
                        Bitmap.CreateScaledBitmap(backgroundDx2, width, height, true);
                }

                canvas.DrawBitmap(backgroundScaledBitmap, 0, 0, null);
            }
        }

        // 月齢
        private MoonAges moonAge = MoonAges.none;
        public MoonAges MoonAge
        {
            get { return moonAge; }
            set
            {
                if (moonAge != value)
                {
                    mustRescaled = true;
                }
                moonAge = value;
            }
        }

        public enum FaceModes { Logo, Moon }

        // 画面表示モード（ロゴか月齢か）
        private FaceModes faceMode = FaceModes.Moon;
        public FaceModes FaceMode
        {
            get { return faceMode; }
            set
            {
                if (faceMode != value)
                {
                    mustRescaled = true;
                }
                faceMode = value;
            }
        }

        // アンビエントモードの判定
        private bool isInAmbientMode = false;
        public bool IsInAmbientMode
        {
            get { return isInAmbientMode; }
            set
            {
                if (isInAmbientMode != value)
                {
                    mustRescaled = true;
                }
                isInAmbientMode = value;
            }
        }

        // リスト表示の判定
        private bool isListMode = false;
        public bool IsListMode
        {
            get { return isListMode; }
            set
            {
                if (isListMode != value)
                {
                    mustRescaled = true;
                }
                isListMode = value;
            }
        }
    }
}