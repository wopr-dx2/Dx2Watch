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
    /// <summary>
    /// イメージでのメッセージ描画
    /// </summary>
    class MessageImage
    {
        // 描画用 Paint オブジェクト
        Paint paint;

        // キャラクタ Bitmap
        private Bitmap bmpPlayer;
        private Bitmap bmpTemplarDragon;
        private Bitmap bmpEileen;
        private Bitmap bmpShionyan;

        //  吹き出し Bitmap
        private Bitmap bmpBefore5min;
        private Bitmap bmpBefore1min;
        private Bitmap bmpEnded;

        // キャラクタ用、吹き出し用 ScaledBitmap
        private Bitmap charScaledBitmap;
        private Bitmap balloonScaledBitmap;

        // 遅延実行
        Handler handler;
        Action action;

        // 表示秒数の保管（Show で設定して Draw で使用する）
        long seconds;
        // true: PostDelayed の実行
        bool hasPost;
        // 表示状態
        bool visible;        
        bool mustCharRescaled;      // Rescale をするか
        bool mustBalloonRescaled;   // 　〃

        public MessageImage(CanvasWatchFaceService owner)
        {
            mustCharRescaled = true;
            mustBalloonRescaled = true;

            Character = Characters.none;
            Balloon = Messages.none;

            hasPost = false;
            visible = false;

            paint = new Paint
            {
                AntiAlias = true
            };

            //var drawablePlayer = owner.Resources.GetDrawable(Resource.Drawable.CharPlayer);
            //var drawableTemplarDragon = owner.Resources.GetDrawable(Resource.Drawable.CharTemplarDragon);
            //var drawableEileen = owner.Resources.GetDrawable(Resource.Drawable.CharEileen);
            //var drawableShionyan = owner.Resources.GetDrawable(Resource.Drawable.CharShionyan);

            // 'Resources.GetDrawable(int)' は旧形式です('deprecated')
            //bmpPlayer = (owner.Resources.GetDrawable(Resource.Drawable.CharPlayer) as BitmapDrawable).Bitmap;
            //bmpTemplarDragon = (owner.Resources.GetDrawable(Resource.Drawable.CharTemplarDragon) as BitmapDrawable).Bitmap;
            //bmpEileen = (owner.Resources.GetDrawable(Resource.Drawable.CharEileen) as BitmapDrawable).Bitmap;
            //bmpShionyan = (owner.Resources.GetDrawable(Resource.Drawable.CharShionyan) as BitmapDrawable).Bitmap;

            // Resource から Bitmap を読み込み、Rescale 前の状態で変数にセット
            bmpPlayer = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.CharPlayer, null)
                as BitmapDrawable).Bitmap;
            bmpTemplarDragon = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.CharTemplarDragon, null)
                as BitmapDrawable).Bitmap;
            bmpEileen = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.CharEileen, null)
                as BitmapDrawable).Bitmap;
            bmpShionyan = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.CharShionyan, null)
                as BitmapDrawable).Bitmap;

            //var drawableBefore5min = owner.Resources.GetDrawable(Resource.Drawable.BalloonBefore5min);
            //var drawableBefore1min = owner.Resources.GetDrawable(Resource.Drawable.BalloonBefore1min);
            //var drawableEnded = owner.Resources.GetDrawable(Resource.Drawable.BalloonEnded);

            // 'Resources.GetDrawable(int)' は旧形式です('deprecated')
            //bmpBefore5min = (owner.Resources.GetDrawable(Resource.Drawable.BalloonBefore5min) as BitmapDrawable).Bitmap;
            //bmpBefore1min = (owner.Resources.GetDrawable(Resource.Drawable.BalloonBefore1min) as BitmapDrawable).Bitmap;
            //bmpEnded = (owner.Resources.GetDrawable(Resource.Drawable.BalloonEnded) as BitmapDrawable).Bitmap;

            // Resource から Bitmap を読み込み、Rescale 前の状態で変数にセット
            bmpBefore5min = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.BalloonBefore5min, null)
                as BitmapDrawable).Bitmap;
            bmpBefore1min = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.BalloonBefore1min, null)
                as BitmapDrawable).Bitmap;
            bmpEnded = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.BalloonEnded, null)
                as BitmapDrawable).Bitmap;

            // 遅延実行
            handler = new Handler();
            action = () => { visible = false; };
        }

        /// <summary>
        /// 表示処理
        /// </summary>
        /// <param name="sec">表示時間（初期値 3,000 ミリ秒）</param>
        public void Show(long sec = 3000)
        {
            seconds = sec;

            // 表示済みの場合も考えて、一旦キャンセルしておく
            Cancel();

            visible = true;
            hasPost = true;
            mustCharRescaled = true;
            mustBalloonRescaled = true;
        }

        /// <summary>
        /// 表示のキャンセル
        /// 表示中の場合は handler.RemoveCallbacksAndMessages を実行
        /// </summary>
        public void Cancel()
        {
            if (visible)
            {
                handler.RemoveCallbacksAndMessages(null);
                visible = false;
            }
        }

        /// <summary>
        /// キャラ選択
        /// プレイヤー → テンプラドラゴン → アイリーン → しおにゃん → 非表示
        /// 吹き出しは非表示にする
        /// </summary>
        /// <param name="sec">表示時間（初期値 3,000 ミリ秒）</param>
        public void CharSelect(long sec = 3000)
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
                    Character = Characters.Player;
                    break;
            }

            Balloon = Messages.none;

            Show(sec);
        }

        /// <summary>
        /// 描画（基本は AnalogWatchFaceEngine.OnDraw から呼ぶ）
        /// </summary>
        /// <param name="canvas">OnDraw の canvas オブジェクト</param>
        /// <param name="bounds">OnDraw の bounds オブジェクト</param>
        public void Draw(Canvas canvas, Rect bounds)
        {
            // 非表示状態の場合（Text モードの場合）は終了
            if (!visible)
            {
                return;
            }

            // キャラクタが設定されていない場合はしゅう
            if (Character == Characters.none)
            {
                return;
            }

            int width = bounds.Width();
            int height = bounds.Height();


            if (Character != Characters.none)
            {
                if (mustCharRescaled)
                {
                    switch (Character)
                    {
                        case Characters.Player:
                            charScaledBitmap =
                                Bitmap.CreateScaledBitmap(bmpPlayer, width, height, true);
                            break;
                        case Characters.TemplarDragon:
                            charScaledBitmap =
                                Bitmap.CreateScaledBitmap(bmpTemplarDragon, width, height, true);
                            break;
                        case Characters.Eileen:
                            charScaledBitmap =
                                Bitmap.CreateScaledBitmap(bmpEileen, width, height, true);
                            break;
                        case Characters.Shionyan:
                            charScaledBitmap =
                                Bitmap.CreateScaledBitmap(bmpShionyan, width, height, true);
                            break;
                        default:
                            break;
                    }

                    mustCharRescaled = false;
                }

                canvas.DrawBitmap(charScaledBitmap, 0, 0, paint);

                if (Balloon != Messages.none)   // Character == none で Balloon != none は無い
                {
                    if (mustBalloonRescaled)
                    {
                        switch (Balloon)
                        {
                            case Messages.Before5min:
                                balloonScaledBitmap =
                                    Bitmap.CreateScaledBitmap(bmpBefore5min, width, height, true);
                                break;
                            case Messages.Before1min:
                                balloonScaledBitmap =
                                    Bitmap.CreateScaledBitmap(bmpBefore1min, width, height, true);
                                break;
                            case Messages.Ended:
                                balloonScaledBitmap =
                                    Bitmap.CreateScaledBitmap(bmpEnded, width, height, true);
                                break;
                            default:
                                break;
                        }

                        mustBalloonRescaled = false;
                    }

                    canvas.DrawBitmap(balloonScaledBitmap, 0, 0, paint);
                }

                if (hasPost)
                {
                    handler.PostDelayed(action, seconds);
                    hasPost = false;
                }
            }
        }

        public Characters Character { get; set; }
        public Messages Balloon { get; set; }
    }
}