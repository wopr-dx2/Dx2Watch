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
        private Bitmap charPlayer;
        private Bitmap charTemplarDragon;
        private Bitmap charEileen;
        private Bitmap charShionyan;

        //  吹き出し Bitmap
        private Bitmap balloonBefore5min;
        private Bitmap balloonBefore1min;
        private Bitmap balloonEnded;

        // キャラクタ用、吹き出し用 ScaledBitmap
        //private Bitmap charScaledBitmap;
        private Bitmap charScaledPlayer;
        private Bitmap charScaledTemplarDragon;
        private Bitmap charScaledEileen;
        private Bitmap charScaledShionyan;
        //private Bitmap balloonScaledBitmap;
        private Bitmap balloonScaledBefore5min;
        private Bitmap balloonScaledBefore1min;
        private Bitmap balloonScaledEnded;

        // 遅延実行
        Handler handler;
        Action action;

        // 表示秒数の保管（Show で設定して Draw で使用する）
        long seconds;
        // true: PostDelayed の実行
        //bool hasPost;
        //bool mustCharRescaled;      // Rescale をするか
        //bool mustBalloonRescaled;   // 　〃

        public MessageImage(CanvasWatchFaceService owner)
        {
            //mustCharRescaled = true;
            //mustBalloonRescaled = true;

            Character = Characters.none;
            Message = Messages.none;

            //hasPost = false;
            Visible = false;

            paint = new Paint
            {
                FilterBitmap = true
            };

            #region Resource から Bitmap を読み込み、Rescale 前の状態で変数にセット

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
            charPlayer = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.CharPlayer, null)
                as BitmapDrawable).Bitmap;
            charTemplarDragon = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.CharTemplarDragon, null)
                as BitmapDrawable).Bitmap;
            charEileen = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.CharEileen, null)
                as BitmapDrawable).Bitmap;
            charShionyan = (
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
            balloonBefore5min = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.BalloonBefore5min, null)
                as BitmapDrawable).Bitmap;
            balloonBefore1min = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.BalloonBefore1min, null)
                as BitmapDrawable).Bitmap;
            balloonEnded = (
                ResourcesCompat.GetDrawable(owner.Resources, Resource.Drawable.BalloonEnded, null)
                as BitmapDrawable).Bitmap;

            #endregion

            // 遅延実行
            handler = new Handler();
            action = () => { Visible = false; };
        }

        /// <summary>
        /// 画像のリスケール
        /// </summary>
        /// <param name="rect">リスケールするサイズ</param>
        public void Rescale(MotoRect rect)
        {
            charScaledPlayer =
                Bitmap.CreateScaledBitmap(
                    charPlayer, rect.Width, rect.Height, true);
            charScaledTemplarDragon =
                Bitmap.CreateScaledBitmap(
                    charTemplarDragon, rect.Width, rect.Height, true);
            charScaledEileen =
                Bitmap.CreateScaledBitmap(
                    charEileen, rect.Width, rect.Height, true);
            charScaledShionyan =
                Bitmap.CreateScaledBitmap(
                    charShionyan, rect.Width, rect.Height, true);
            balloonScaledBefore5min =
                Bitmap.CreateScaledBitmap(
                    balloonBefore5min, rect.Width, rect.Height, true);
            balloonScaledBefore1min =
                Bitmap.CreateScaledBitmap(
                    balloonBefore1min, rect.Width, rect.Height, true);
            balloonScaledEnded =
                Bitmap.CreateScaledBitmap(
                    balloonEnded, rect.Width, rect.Height, true);
        }

        /// <summary>
        /// 表示処理
        /// </summary>
        /// <param name="sec">表示時間（初期値 3,000 ミリ秒）</param>
        public void Show(long sec = 3000)
        {
            seconds = sec;

            // 表示済みの場合も考えて、一旦キャンセルしておく
            cancel();

            Visible = true;
            handler.PostDelayed(action, seconds);
        }

        /// <summary>
        /// 表示のキャンセル
        /// 表示中の場合は handler.RemoveCallbacksAndMessages を実行
        /// </summary>
        void cancel()
        {
            if (Visible)
            {
                handler.RemoveCallbacksAndMessages(null);
                Visible = false;
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

            Message = Messages.none;

            Show(sec);
        }

        /// <summary>
        /// 描画（基本は AnalogWatchFaceEngine.OnDraw から呼ぶ）
        /// </summary>
        /// <param name="canvas">OnDraw の canvas オブジェクト</param>
        /// <param name="rect">OnDraw の bounds オブジェクトから変換された MotoRect オブジェクト</param>
        public void Draw(Canvas canvas, MotoRect rect)
        {
            // 非表示状態の場合は終了
            if (!Visible)
            {
                return;
            }

            // キャラクタが設定されていない場合（Text モード）は終了
            // 呼び出し元で握り潰してるはずやけど、もれたら嫌なんで
            if (Character == Characters.none)
            {
                return;
            }

            switch (Character)
            {
                case Characters.Player:
                    if (charScaledPlayer == null)
                    {
                        charScaledPlayer =
                            Bitmap.CreateScaledBitmap(
                                charPlayer, rect.Width, rect.Height, true);
                    }
                    canvas.DrawBitmap(charScaledPlayer, rect.Left, rect.Top, paint);
                    break;
                case Characters.TemplarDragon:
                    if (charScaledTemplarDragon == null)
                    {
                        charScaledTemplarDragon =
                            Bitmap.CreateScaledBitmap(
                                charTemplarDragon, rect.Width, rect.Height, true);
                    }
                    canvas.DrawBitmap(charScaledTemplarDragon, rect.Left, rect.Top, paint);
                    break;
                case Characters.Eileen:
                    if (charScaledEileen == null)
                    {
                        charScaledEileen =
                            Bitmap.CreateScaledBitmap(
                                charEileen, rect.Width, rect.Height, true);
                    }
                    canvas.DrawBitmap(charScaledEileen, rect.Left, rect.Top, paint);
                    break;
                case Characters.Shionyan:
                    if (charScaledShionyan == null)
                    {
                        charScaledShionyan =
                            Bitmap.CreateScaledBitmap(
                                charShionyan, rect.Width, rect.Height, true);
                    }
                    canvas.DrawBitmap(charScaledShionyan, rect.Left, rect.Top, paint);
                    break;
                default:
                    break;
            }

            if (Message != Messages.none)
            {
                switch (Message)
                {
                    case Messages.Before5min:
                        if (balloonScaledBefore5min == null)
                        {
                            balloonScaledBefore5min =
                                Bitmap.CreateScaledBitmap(
                                    balloonBefore5min, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(balloonScaledBefore5min, rect.Left, rect.Top, paint);
                        break;
                    case Messages.Before1min:
                        if (balloonScaledBefore1min == null)
                        {
                            balloonScaledBefore1min =
                                Bitmap.CreateScaledBitmap(
                                    balloonBefore1min, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(balloonScaledBefore1min, rect.Left, rect.Top, paint);
                        break;
                    case Messages.Ended:
                        if (balloonScaledEnded == null)
                        {
                            balloonScaledEnded =
                                Bitmap.CreateScaledBitmap(
                                    balloonEnded, rect.Width, rect.Height, true);
                        }
                        canvas.DrawBitmap(balloonScaledEnded, rect.Left, rect.Top, paint);
                        break;
                    default:
                        break;
                }
            }
        }

        public Characters Character { get; set; }
        public Messages Message { get; set; }

        public bool FilterBitmap
        {
            get { return paint.FilterBitmap; }
            set { paint.FilterBitmap = value; }
        }

        // 表示状態
        public bool Visible { get; set; }
    }
}