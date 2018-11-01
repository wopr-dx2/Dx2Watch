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
    /// <summary>
    /// 	アナログ時計のウォッチフェイスサービスを提供します。
    /// </summary>
    /// <remarks>
    ///		<see cref="CanvasWatchFaceService"/>クラスを継承して実装します。
    /// </remarks>
    // C#の属性で、ウォッチフェイスサービス要素の内容を構成します。
    // Labelプロパティには、ウォッチフェイスの選択に表示する名前を設定します。
    // 注：Nameプロパティについて : 
    //		Xamarinでは、「.」から始まる AndroidのShortcut が利用できません。
    //　		なお、省略した場合、Nameには「md[GUID].クラス名」となります。
    [Service(Label = "@string/my_watch_name", Permission = "android.permission.BIND_WALLPAPER")]
    // 壁紙にバインドするパーミッションを宣言します。
    [MetaData("android.service.wallpaper", Resource = "@xml/watch_face")]
    // プレビューに表示する画像を指定します。
    // previewが四角形用、preview_circularが丸形用です。
    [MetaData("com.google.android.wearable.watchface.preview", Resource = "@drawable/preview")]
    [MetaData("com.google.android.wearable.watchface.preview_circular", Resource = "@drawable/preview_circular")]
    // このWatch Faceで使用するIntentFilterを宣言します。
    [IntentFilter(new[] { "android.service.wallpaper.WallpaperService" }, Categories = new[] { "com.google.android.wearable.watchface.category.WATCH_FACE" })]
    public class MyWatchFaceService : CanvasWatchFaceService
    {

//#if DEBUG
//		/// <summary>
//		///		ログ出力用のタグを表します。
//		/// </summary>
//		private const string logTag = nameof( MyWatchFaceService );
//#endif

        /// <summary>
        /// 	インタラクティブモードにおける更新間隔（ミリ秒単位）を表します。
        /// </summary>
        /// <remarks>
        ///		アンビエントモードの時は、このフィールドの設定にかかわらず、1分間隔で更新されます。
        /// </remarks>
        #region 例 : 更新間隔を1秒に設定する場合
        /*
			// Java.Util.Concurrent.TimeUnitを使用する場合
			Java.Util.Concurrent.TimeUnit.Seconds.ToMillis( 1 )

			// System.TimeSpanを使用する場合（ 戻り値はdouble型なので、long型にキャストします ）
			( long )System.TimeSpan.FromSeconds( 1 ).TotalMilliseconds
		*/
        #endregion
        private static readonly long InteractiveUpdateRateMilliseconds = Java.Util.Concurrent.TimeUnit.Seconds.ToMillis(1);

        /// <summary>
        /// 	インタラクティブモードにて、定期的に時刻を更新するための、ハンドラー用のメッセージのIDを表します。
        /// </summary>
        private const int MessageUpdateTime = 0;

        /// <summary>
        ///		<see cref="CanvasWatchFaceService.Engine"/>オブジェクトが作成される時に実行します。
        /// </summary>
        /// <returns><see cref="CanvasWatchFaceService.Engine"/>クラスを継承したオブジェクト</returns>
        public override WallpaperService.Engine OnCreateEngine()
        {
            // CanvasWatchFaceService.Engineクラスを継承したMyWatchFaceEngineのコンストラクターに、
            // MyWatchFaceオブジェクトの参照を指定します。
            return new MyWatchFaceEngine(this);
        }

        /// <summary>
        ///		アナログ時計のウォッチフェイスの<see cref="CanvasWatchFaceService.Engine"/>機能を提供します。
        /// </summary>
        /// <remarks>
        ///		<see cref="CanvasWatchFaceService.Engine"/>クラスを継承して実装します。※<see cref="CanvasWatchFaceService"/>は省略可能です。
        /// </remarks>
        private class MyWatchFaceEngine : Engine
        {

            /// <summary>
            ///		<see cref="CanvasWatchFaceService"/>オブジェクトの参照を表します。
            /// </summary>
            private CanvasWatchFaceService owner;

            #region タイマー系

            /// <summary>
            ///		時刻を更新した時の処理を表します。
            /// </summary>
            /// <remarks>
            ///		Android Studio（ Java系 ）側でいう、EngineHandlerの役割を持ちます。
            /// </remarks>
            private readonly Handler updateTimeHandler;

            #region 現在時刻の取得に、どのライブラリを使用すればよいのですか？
            /*
			 *		1. Android.Text.Format.Timeクラス
			 *		　　AndroidのAPIで用意されている日付・時刻のクラスです。
			 *		　　Timeオブジェクト.SetToNowメソッドで、現在のタイムゾーンに対する現在時刻にセットすることができます。
			 *		　　Timeオブジェクト.Clearメソッドで、指定したタイムゾーンのIDに対するタイムゾーンを設定します。
			 *		　　※2032年までしか扱えない問題があるため、Android API Level 22以降では旧形式となっています。
			 *		　　
			 *		2. Java.Util.Calendarクラス
			 *		　　Javaで用意されている日付・時刻のクラスです。
			 *		　　Calendar.GetInstanceメソッドで、現在のタイムゾーンに対する現在時刻にセットすることができます。
			 *		　　Calendarオブジェクト.TimeZoneプロパティで、タイムゾーンを設定することができます。
			 *		
			 *		3. System.DateTime構造体
			 *		　　.NET Frameworkで用意されている日付・時刻のクラスです。
			 *		　　DateTime.Nowで、現在のタイムゾーンに対する現在時刻を取得することができます。
			 *		　　※タイムゾーンは、Android Wearデバイスとペアリングしているスマートフォンのタイムゾーンとなります。
			 */
            #endregion

            /// <summary>
            ///		時刻を格納するオブジェクトを表します。
            /// </summary>
            // Time ( Android )
            //private Time nowTime;
            // Calendar ( Java )
            private Java.Util.Calendar nowTime;
            // DateTime ( C# )
            //private DateTime nowTime;

            #endregion

            #region グラフィックス系

            /// <summary>
            ///		背景用のペイントオブジェクトを表します。
            /// </summary>
            //private Paint backgroundPaint;

            /// <summary>
            ///		時針用のオブジェクトを表します。
            /// </summary>
            //private HourAnalogHandStroke hourHand;
            /// <summary>
            ///		分針用のオブジェクトを表します。
            /// </summary>
            //private MinuteAnalogHandStroke minuteHand;
            /// <summary>
            ///		秒針用のオブジェクトを表します。
            /// </summary>
            //private SecondAnalogHandStroke secondHand;

            #endregion

            #region モード系

            /// <summary>
            ///		アンビエントモードであるかどうかを表します。
            /// </summary>
            private bool isAmbient;

            /// <summary>
            ///		デバイスがLowBitアンビエントモードを必要としているかどうかを表します。
            /// </summary>
            /// <remarks>
            ///		<para>デバイスがLowBitアンビエントモードを使用する場合、アンビエントモードの時は、以下の2点の工夫が必要になります。</para>
            ///		<para>・使用できる色が8色（ ブラック、ホワイト、ブルー、レッド、マゼンタ、グリーン、シアン、イエロー ）のみとなります。</para>
            ///		<para>・アンチエイリアスが無効となります。</para>
            /// </remarks>
            private bool isRequiredLowBitAmbient;

            /// <summary>
            ///		デバイスがBurn-in-protection（焼き付き防止）を必要としているかどうかを表します。
            /// </summary>
            /// <remarks>
            ///		<para>
            ///			ディスプレイが有機ELなど、Burn-in-protectionが必要な場合、アンビエントモードの時は、以下の2点の工夫が必要になります。
            ///		</para>
            ///		<para>・画像はなるべく輪郭のみにします。</para>
            ///		<para>・ディスプレイの端から数ピクセルには描画しないようにします。</para>
            /// </remarks>
            private bool isReqiredBurnInProtection;

            /// <summary>
            ///		ミュート状態であるかどうかを表します。
            /// </summary>
            private bool isMute;

            #endregion

            #region レシーバー系

            /// <summary>
            ///		タイムゾーンを変更した時に通知を受け取るレシーバーを表します。
            /// </summary>
            private ActionExecutableBroadcastReceiver timeZoneReceiver;

            #endregion

            #region Dx2 Watch

            /// <summary>
            /// 日付を表示します
            /// </summary>
            private WatchDate watchDate;
            /// <summary>
            /// 時刻を表示します
            /// </summary>
            private WatchTime watchTime;

            /// <summary>
            /// 背景用画像の制御
            /// </summary>
            private WatchBackground watchBackground;

            /// <summary>
            /// 時針、分針、秒針の制御
            /// </summary>
            private WatchHands watchHands;

            /// <summary>
            /// 満月グラフを左上またはリスト表示する制御
            /// </summary>
            private WatchGraph watchGraph;

            /// <summary>
            /// 40 分前から表示する円
            /// </summary>
            private WatchReminder watchReminder;

            /// <summary>
            /// メッセージを表示します
            /// </summary>
            private WatchNotify watchNotify;
            //private MessageText messageText;

            /// <summary>
            /// 月齢判定
            /// </summary>
            private MoonAge moon;

            /// <summary>
            /// OnTapCommand で X, Y 判定をするために使用します
            /// </summary>
            //private Rect rect;
            private MotoRect motoRect;

            #endregion

            /// <summary>
            ///		<see cref="MyWatchFaceEngine"/>クラスの新しいインスタンスを生成します。
            /// </summary>
            /// <param name="owner"><see cref="CanvasWatchFaceService"/>クラスを継承したオブジェクトの参照</param>
            public MyWatchFaceEngine(CanvasWatchFaceService owner) : base(owner)
            {
                // CanvasWatchFaceServiceクラスを継承したオブジェクトの参照をセットします。
                this.owner = owner;
                // 時刻を更新した時の処理を構成します。
                updateTimeHandler = new Handler(
                    message =>
                    {
//#if DEBUG
//						if( Log.IsLoggable( logTag, LogPriority.Info ) ) {
//							Log.Info( logTag, $"Updating timer: Message = {message.What}" );
//						}
//#endif

                        // Whatプロパティでメッセージを判別します。
                        switch (message.What)
                        {
                            case MessageUpdateTime:
                                // TODO : 時刻の更新のメッセージの時の処理を入れます。
                                // ウォッチフェイスを再描画します。
                                Invalidate();
                                // タイマーを動作させるかどうかを判別します。
                                if (ShouldTimerBeRunning)
                                {
                                    /*
										Javaでは、System.currentTimeMillisメソッドで世界協定時（ミリ秒）を取得します。
										一方C#では、DateTime.UtcNow.Ticksプロパティで世界協定時（100ナノ秒）取得し、
										TimeSpan.TicksPerMillisecondフィールドで割って、ミリ秒の値を求めます。
									*/
                                    long timeMillseconds = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
                                    // delayMs = 更新間隔 - ( 現在時刻（ミリ秒） % 更新間隔) -> 更新間隔との差
                                    long delayMilliseconds = InteractiveUpdateRateMilliseconds - (timeMillseconds % InteractiveUpdateRateMilliseconds);
                                    // UpdateTimeHandlerにメッセージをセットします。
                                    // SendEmptyMessageDelayedメソッドは指定した時間後にメッセージを発行します。
                                    updateTimeHandler.SendEmptyMessageDelayed(MessageUpdateTime, delayMilliseconds);
                                }
                                break;
                        }
                    }
                );

                // TimeZoneReceiverのインスタンスを生成します。
                timeZoneReceiver = new ActionExecutableBroadcastReceiver(
                    intent =>
                    {
                        // TODO : ブロードキャストされた Intent.ActionTimezoneChanged のIntentオブジェクトを受け取った時に実行する処理を入れます。
                        // IntentからタイムゾーンIDを取得して、Timeオブジェクトのタイムゾーンに設定し、現在時刻を取得します。
                        // intent.GetStringExtra( "time-zone" )の戻り値はタイムゾーンのIDです。
                        // Time ( Android )
                        //nowTime.Clear( intent.GetStringExtra( "time-zone" ) );
                        //nowTime.SetToNow();
                        // Calendar ( Java )
                        nowTime = Java.Util.Calendar.GetInstance(Java.Util.TimeZone.GetTimeZone(intent.GetStringExtra("time-zone")));
                        // DateTime ( C# )
                        //nowTime = DateTime.Now;
                    },
                    // Intentフィルターに「ActionTimezoneChanged」を指定します。
                    Intent.ActionTimezoneChanged
                );
            }

            /// <summary>
            ///		<see cref="MyWatchFaceEngine"/>のインスタンスが生成された時に実行します。
            /// </summary>
            /// <param name="holder">ディスプレイ表面を表すオブジェクト</param>
            public override void OnCreate(ISurfaceHolder holder)
            {
                // TODO : ここでは主に、以下の処理を行います。
                // ・リソースから画像の読み込み
                // ・Paintなどのグラフィックスオブジェクトを生成
                // ・時刻を格納するオブジェクトの作成
                // ・システムのUI（インジケーターやOK Googleの表示など）の設定

                // システムのUIの配置方法を設定します。
                SetWatchFaceStyle(
                    new WatchFaceStyle.Builder(owner)
                #region ウォッチフェイスのスタイルの設定

                        // ユーザーからのタップイベントを有効にするかどうか設定します。
                        //   true  : 有効
                        //   false : 無効（デフォルト）
                        .SetAcceptsTapEvents(true)

                        // 通知が来た時の通知カードの高さを設定します。
                        //   WatchFaceStyle.PeekModeShort    : 通知カードをウィンドウの下部に小さく表示します。（デフォルト）
                        //   WatchFaceStyle.PeekModeVariable : 通知カードをウィンドウの全面に表示します。
                        .SetCardPeekMode(WatchFaceStyle.PeekModeShort)

                        // 通知カードの背景の表示方法を設定します。
                        //   WatchFaceStyle.BackgroundVisibilityInterruptive : 電話の着信など一部の通知のみ、背景を用事します。（デフォルト）
                        //   WatchFaceStyle.BackgroundVisibilityPersistent   : 通知カードの種類にかかわらず、その背景を常に表示します。
                        .SetBackgroundVisibility(WatchFaceStyle.BackgroundVisibilityInterruptive)

                        // アンビエントモード時に通知カードを表示するかどうかを設定します。
                        //   WatchFaceStyle.AmbientPeekModeVisible : 通知カードを表示します。（デフォルト）
                        //   WatchFaceStyle.AmbientPeekModeHidden  : 通知カードを表示しません。
                        //.SetAmbientPeekMode( WatchFaceStyle.AmbientPeekModeHidden )

                        // システムUIのデジタル時計を表示するするかどうかを設定します。（使用している例として、デフォルトで用意されている「シンプル」があります。）
                        //   true  : 表示
                        //   false : 非表示（デフォルト）
                        .SetShowSystemUiTime(false)

                        // ステータスアイコンなどに背景を付けるかどうかを設定します。
                        //   デフォルト                               : ステータスアイコンなどに背景を表示しません。
                        //   WatchFaceStyle.ProtectStatusBar        : ステータスアイコンに背景を表示します。
                        //   WatchFaceStyle.ProtectHotwordIndicator : 「OK Google」に背景を表示します。
                        //   WatchFaceStyle.ProtectWholeScreen      :　ウォッチフェイスの背景を少し暗めにします。
                        // ※パラメーターは論理和で組み合わせることができます。
                        //.SetViewProtectionMode( WatchFaceStyle.ProtectStatusBar | WatchFaceStyle.ProtectHotwordIndicator )

                        // 通知カードを透明にするかどうかを設定します。
                        //   WatchFaceStyle.PeekOpacityModeOpaque      : 不透明（デフォルト）
                        //   WatchFaceStyle.PeekOpacityModeTranslucent : 透明
                        //.SetPeekOpacityMode( WatchFaceStyle.PeekOpacityModeTranslucent )

                        // ステータスアイコンや「OK Google」の位置を設定します。
                        //   GravityFlags.Top | GravityFlags.Left   : 左上（角形のデフォルト）
                        //   GravityFlags.Top | GravityFlags.Center : 上部の中央（丸形のデフォルト）
                        // 注 : GravityFlagsは列挙体なので、int型にキャストします。
                        //.SetStatusBarGravity( ( int )( GravityFlags.Top | GravityFlags.Center ) )
                        //.SetHotwordIndicatorGravity( ( int )( GravityFlags.Top | GravityFlags.Center ) )

                #endregion
                        // 設定したスタイル情報をビルドします。このメソッドは最後に呼び出します。
                        .Build()
                );
                // ベースクラスのOnCreateメソッドを実行します。
                base.OnCreate(holder);

                #region 最新のAndroid SDKにおける、Android.Content.Res.Resources.GetColorメソッドについて
                /*
					Android.Content.Res.Resources.GetColorメソッドは、Android SDK Level 23以降で非推奨（Deprecated）となっています。
					代わりの方法として、Android.Support.V4.Content.ContextCompat.GetColorメソッドを使用します。
					
					[CanvasWatchFaceServiceオブジェクト].Resources.GetColor( Resource.Color.[リソース名] );
					↓
					ContextCompat.GetColor( [CanvasWatchFaceServiceオブジェクト], Resource.Color.[リソース名] );
					※CanvasWatchFaceServiceクラスはContextクラスを継承しています。

					なお、ContextCompat.GetColorの戻り値はColor型でなく、ARGB値を格納したint型となります。
					Chronoir_net.Chronica.WatchfaceExtension.WatchfaceUtility.ConvertARGBToColor( int )メソッドで、Color型に変換することができます。
				*/
                #endregion

                // 時刻を格納するオブジェクトを生成します。
                // Time ( Android )
                //nowTime = new Time();
                // Calendar ( Java )
                nowTime = Java.Util.Calendar.GetInstance(Java.Util.TimeZone.Default);
                // DateTime ( C# )
                // DateTime構造体は値型なので、オブジェクトの生成はは不要です。

                // 画面サイズ準備（OnDraw で更新）
                // とりあえず Moto 360 1st gen に設定
                motoRect = new MotoRect(0, 0, 320, 320)
                {
                    IsMoto360 = true
                };

                // 背景
                watchBackground = new WatchBackground(owner);
                watchBackground.Rescale(motoRect);
                // グラフ
                watchGraph = new WatchGraph();
                // リマインダ
                watchReminder = new WatchReminder(motoRect);
                // 時針、分針、秒針
                watchHands = new WatchHands(owner);
                // 日時
                watchDate = new WatchDate();
                watchTime = new WatchTime();
                // 月齢判定
                moon = new MoonAge();
                moon.Initialize(
                    WatchfaceUtility.ConvertToDateTime(nowTime));
                // メッセージ
                watchNotify = new WatchNotify(owner);
                watchNotify.Rescale(motoRect);
            }

            /// <summary>
            ///		このウォッチフェイスサービスが破棄される時に実行します。
            /// </summary>
            /// <remarks>
            ///		例えば、このウォッチフェイスから別のウォッチフェイスに切り替えた時に呼び出されます。
            /// </remarks>
            public override void OnDestroy()
            {
                // UpdateTimeHandlerにセットされているメッセージを削除します。
                updateTimeHandler.RemoveMessages(MessageUpdateTime);
                // ベースクラスのOnDestroyメソッドを実行します。
                base.OnDestroy();
            }

            /// <summary>
            ///		<see cref="WindowInsets"/>を適用する時に実行します。
            /// </summary>
            /// <param name="insets">適用される<see cref="WindowInsets"/>オブジェクト</param>
            /// <remarks>Android Wearが丸形がどうかを、このメソッド内で判別することができます。</remarks>
            public override void OnApplyWindowInsets(WindowInsets insets)
            {
                base.OnApplyWindowInsets(insets);

//#if DEBUG
//				if( Log.IsLoggable( logTag, LogPriority.Info ) ) {
//					Log.Info( logTag, $"{nameof( OnApplyWindowInsets )}: Round = {insets.IsRound}" );
//				}
//#endif

                // TODO: ウィンドウの形状によって設定する処理を入れます。
                // Android Wearが丸形かどうかを判別します。
                //bool isRound = insets.IsRound;
            }

            /// <summary>
            ///		ウォッチフェイスのプロパティが変更された時に実行します。
            /// </summary>
            /// <param name="properties">プロパティ値を格納したバンドルオブジェクト</param>
            public override void OnPropertiesChanged(Bundle properties)
            {
                // ベースクラスのOnPropertiesChangedメソッドを実行します。
                base.OnPropertiesChanged(properties);
                // LowBitアンビエントモードを使用するかどうかの値を取得します。
                isRequiredLowBitAmbient = properties.GetBoolean(PropertyLowBitAmbient, false);
                // Burn-in-protectionが必要かどうかの値を取得します。
                isReqiredBurnInProtection = properties.GetBoolean(PropertyBurnInProtection, false);

//#if DEBUG
//				if( Log.IsLoggable( logTag, LogPriority.Info ) ) {
//					Log.Info( logTag, $"{nameof( OnPropertiesChanged )}: Low-bit ambient = {isRequiredLowBitAmbient}" );
//					Log.Info( logTag, $"{nameof( OnPropertiesChanged )}: Burn-in-protection = {isReqiredBurnInProtection}" );
//				}
//#endif
            }

            /// <summary>
            ///		時間を更新した時に実行します。
            /// </summary>
            /// <remarks>
            ///		画面の表示・非表示やモードに関わらず、1分ごとに呼び出されます。
            /// </remarks>
            public override void OnTimeTick()
            {
                // ベースクラスのOnTimeTickメソッドを実行します。
                base.OnTimeTick();

//#if DEBUG
//				if( Log.IsLoggable( logTag, LogPriority.Info ) ) {
//					Log.Info( logTag, $"OnTimeTick" );
//				}
//#endif

                // ウォッチフェイスを再描画します。
                Invalidate();
            }

            /// <summary>
            ///		アンビエントモードが変更された時に実行されます。
            /// </summary>
            /// <param name="inAmbientMode">アンビエントモードであるかどうかを示す値</param>
            public override void OnAmbientModeChanged(bool inAmbientMode)
            {
                // ベースクラスのOnAmbientModeChangedメソッドを実行します。
                base.OnAmbientModeChanged(inAmbientMode);

                // アンビエントモードが変更されたかどうかを判別します。
                if (isAmbient != inAmbientMode)
                {
//#if DEBUG
//					if( Log.IsLoggable( logTag, LogPriority.Info ) ) {
//						Log.Info( logTag, $"{nameof( OnAmbientModeChanged )}: Ambient-mode = {isAmbient} -> {inAmbientMode}" );
//					}
//#endif

                    // 現在のアンビエントモードをセットします。
                    isAmbient = inAmbientMode;
                    // デバイスがLowBitアンビエントモードをサポートしているかどうかを判別します。
                    if (isRequiredLowBitAmbient)
                    {
                        // TODO : LowBitアンビエントモードがサポートされている時の処理を入れます。
                        // アンビエントモードの時は、針のPaintオブジェクトのアンチエイリアスを無効にし、
                        // そうでなければ有効にします。
                        watchBackground.FilterBitmap = !isAmbient;
                        watchHands.AntiAlias = !isAmbient;
                        watchTime.AntiAlias = !isAmbient;
                        watchNotify.AntiAlias = !isAmbient;
                    }
                    // ウォッチフェイスを再描画します。
                    Invalidate();
                }
                // タイマーを更新します。
                UpdateTimer();
            }

            /// <summary>
            ///		Interruptionフィルターが変更された時に実行します。
            /// </summary>
            /// <param name="interruptionFilter">Interruptionフィルター</param>
            public override void OnInterruptionFilterChanged(int interruptionFilter)
            {
                // ベースクラスのOnInterruptionFilterChangedメソッドを実行します。
                base.OnInterruptionFilterChanged(interruptionFilter);
                // Interruptionフィルターが変更されたかどうか判別します。
                bool inMuteMode = (interruptionFilter == InterruptionFilterNone);

                // ミュートモードが変更されたかどうか判別します。
                if (isMute != inMuteMode)
                {
//#if DEBUG
//					if( Log.IsLoggable( logTag, LogPriority.Info ) ) {
//						Log.Info( logTag, $"{nameof( OnInterruptionFilterChanged )}: Mute-mode = {isMute} -> {inMuteMode}" );
//					}
//#endif

                    isMute = inMuteMode;
                    // TODO : 通知状態がOFFの時の処理を入れます。
                    // ウォッチフェイスを再描画します。
                    Invalidate();
                }
            }

            /// <summary>
            ///		ユーザーがウォッチフェイスをタップした時に実行されます。
            /// </summary>
            /// <param name="tapType">タップの種類</param>
            /// <param name="x">タップのX位置</param>
            /// <param name="y">タップのY位置</param>
            /// <param name="eventTime">画面をタッチしている時間？</param>
            /// <remarks>
            ///		Android Wear 1.3以上に対応しています。
            ///		このメソッドが呼び出させるには、<see cref="WatchFaceStyle.Builder"/>の生成において、SetAcceptsTapEvents( true )を呼び出す必要があります。
            ///		インタラクティブモードのみ有効です。
            ///	</remarks>
            public override void OnTapCommand(int tapType, int x, int y, long eventTime)
            {
//#if DEBUG
//				if( Log.IsLoggable( logTag, LogPriority.Info ) ) {
//					Log.Info( logTag, $"{nameof( OnTapCommand )}: Type = {tapType}, ( x, y ) = ( {x}, {y} ), Event time = {eventTime}" );
//				}
//#endif

                //var resources = owner.Resources;

                // タップの種類を判別します。
                switch (tapType)
                {
                    case TapTypeTouch:
                        // TODO : ユーザーが画面をタッチした時の処理を入れます。
                        break;
                    case TapTypeTouchCancel:
                        // TODO : ユーザーが画面をタッチしたまま、指を動かした時の処理を入れます。
                        break;
                    case TapTypeTap:
                        // TODO : ユーザーがタップした時の処理を入れます。
                        if (x < motoRect.Center.X)
                        {
                            if (y < motoRect.Center.Y)   // 左上
                            {
                                // 文字盤に絵を描くかリスト表示にするか切り替え
                                watchBackground.IsListMode = !watchBackground.IsListMode;
                                watchGraph.IsListMode = !watchGraph.IsListMode;
                            }
                            else                        // 左下
                            {
                                // メッセージ表示のキャンセル
                                //watchNotify.Cancel();
                                //messageText.Message = Messages.Ended;
                                //messageText.Show();
                            }
                        }
                        else
                        {
                            if (y < motoRect.Center.Y)  // 右上
                            {
                                // 文字盤に絵を描く場合、ロゴか月か切り替え
                                if (watchBackground.FaceMode == WatchBackground.FaceModes.Logo)
                                {
                                    watchBackground.FaceMode = WatchBackground.FaceModes.Moon;
                                }
                                else
                                {
                                    watchBackground.FaceMode = WatchBackground.FaceModes.Logo;
                                }
                            }
                            else                        // 右下
                            {
                                // キャラクタ セレクト
                                watchNotify.CharSelect();
                            }
                        }
                        Invalidate();
                        break;
                }
            }

            /// <summary>
            ///		ウォッチフェイスの描画時に実行されます。
            /// </summary>
            /// <param name="canvas">ウォッチフェイスに描画するためのキャンバスオブジェクト</param>
            /// <param name="bounds">画面のサイズを格納するオブジェクト</param>
            public override void OnDraw(Canvas canvas, Rect bounds)
            {
                // TODO : 現在時刻を取得し、ウォッチフェイスを描画する処理を入れます。
                // 現在時刻にセットします。
                // Time ( Android )
                //nowTime.SetToNow();
                // Calendar ( Java )
                nowTime = Java.Util.Calendar.GetInstance(nowTime.TimeZone);
                // DateTime ( C# )
                //nowTime = DateTime.Now;

//#if DEBUG
//				if( Log.IsLoggable( logTag, LogPriority.Info ) ) {
//					Log.Info( logTag, $"{nameof( OnDraw )}: Now time = {WatchfaceUtility.ConvertToDateTime( nowTime ):yyyy/MM/dd HH:mm:ss K}" );
//				}
//#endif

                // OnTapCommand & 画像の Scaled 用に画面サイズを取得
                motoRect.SetBounds(bounds);
                // OnCreate 時に初期値で済んでるはずやけど念のため
                if (motoRect.IsSizeChanged)
                {
                    watchBackground.Rescale(motoRect);
                    watchNotify.Rescale(motoRect);
                }

                // 月齢計算
                moon.Now =
                    WatchfaceUtility.ConvertToDateTime(nowTime);

                //canvas.DrawColor(Color.Black);
                //watchBackground.IsInAmbient = IsInAmbientMode;
                //watchHands.IsAmbient = isAmbient;
                watchHands.Calendar = nowTime;
                watchTime.Calendar = nowTime;
                watchDate.Calendar = nowTime;
                watchReminder.Calendar = nowTime;

                // 背景を描画します。
                // アンビエントモードであるかどうか判別します。
                if (IsInAmbientMode)
                {
                    // アンビエントモードの時は、黒色で塗りつぶします。
                    //canvas.DrawColor(Color.Black);
                    //watchBackground.DrawInAmbient(canvas, motoRect);

                    if (watchBackground.IsListMode)
                    {
                        watchBackground.DrawTetregrammaton(canvas, motoRect);
                        // リストモードの場合は上部に時間を表示
                        //watchTime.Draw(canvas, motoRect);
                    }
                    else
                    {
                        watchBackground.DrawInAmbient(canvas, motoRect);
                        // 時針、分針の線だけ表示する
                        watchHands.DrawHands(canvas, motoRect);
                    }
                }
                else
                {
                    // そうでない時は、背景画像を描画します。

                    // 背景描画
                    watchBackground.MoonAge = moon.Age;
                    watchBackground.Draw(canvas, motoRect);

                    // リマインダ
                    if (!watchBackground.IsListMode)
                    {
                        watchReminder.LastFullMoon = moon.LastFullMoon;
                        watchReminder.Draw(canvas, motoRect);
                    }

                    #region next fullmoon graph

                    watchGraph.LastFullMoon = moon.LastFullMoon;
                    watchGraph.MoonAge = moon.Age;
                    watchGraph.Draw(canvas, motoRect);

                    #endregion

                    #region 日付 右側 ddd d MMM

                    // 満月の時は描かへんいうんもありかな
                    if (watchBackground.FaceMode == WatchBackground.FaceModes.Logo ||
                        moon.Age != MoonAges.Full ||
                        watchGraph.IsListMode)
                    {
                        watchDate.Draw(canvas, motoRect);
                    }

                    #endregion

                    #region メッセージ

                    //watchNotify.Draw(canvas, bounds);

                    #endregion

                    #region メッセージ表示

                    //if (moon.IsBefore5min)
                    //{
                    //    //Xamarin.Essentials.Platform.Vibrator.Vibrate(300);
                    //    watchNotify.Show(Messages.Before5min, 30000);
                    //}
                    //else if (moon.IsBefore1min)
                    //{
                    //    //Xamarin.Essentials.Platform.Vibrator.Vibrate(500);
                    //    watchNotify.Show(Messages.Before1min, 30000);
                    //}
                    //else if (moon.IsFullmoonEnded)
                    //{
                    //    //Xamarin.Essentials.Platform.Vibrator.Vibrate(100);
                    //    watchNotify.Show(Messages.Ended, 5000);
                    //}

                    // メッセージ描画
                    watchNotify.Draw(canvas, motoRect);

                    #endregion

                    // 時針、分針、秒針
                    if (watchBackground.IsListMode)
                    {
                        // リストモードの場合は上部に時間を表示
                        watchTime.Draw(canvas, motoRect);
                        // 秒針だけ表示する
                        watchHands.DrawSec(canvas, motoRect);
                    }
                    else
                    {
                        // 月齢・ロゴ描画時
                        // Draw2 → 針の中心部分が無い
                        watchHands.DrawBorder(canvas, motoRect);
                        watchHands.DrawHands(canvas, motoRect);
                        watchHands.DrawSec(canvas, motoRect);
                    }
                }

                #region メッセージ

                if (moon.During5minMessage)
                {
                    watchNotify.Message = Messages.Before5min;
                    watchNotify.Visible = true;
                }
                else if (moon.During1minMessage)
                {
                    watchNotify.Message = Messages.Before1min;
                    watchNotify.Visible = true;
                }
                else if (moon.DuringEndedMessage)
                {
                    watchNotify.Message = Messages.Ended;
                    watchNotify.Visible = true;
                }
                else if (!watchNotify.IsCharSelecting)
                {
                    watchNotify.Visible = false;
                }

                #endregion

                #region バイブレーション

                if (moon.IsBefore5min)
                {
                    Xamarin.Essentials.Platform.Vibrator.Vibrate(300);
                }
                else if (moon.IsBefore1min)
                {
                    Xamarin.Essentials.Platform.Vibrator.Vibrate(500);
                }
                else if (moon.IsFullmoonEnded)
                {
                    Xamarin.Essentials.Platform.Vibrator.Vibrate(100);
                }

                #endregion
            }

            /// <summary>
            ///		ウォッチフェイスの表示・非表示が切り替わった時に実行します。
            /// </summary>
            /// <param name="visible">ウォッチフェイスの表示・非表示</param>
            public override void OnVisibilityChanged(bool visible)
            {
                // ベースクラスのOnVisibilityChangedメソッドを実行します。
                base.OnVisibilityChanged(visible);

//#if DEBUG
//				if( Log.IsLoggable( logTag, LogPriority.Info ) ) {
//					Log.Info( logTag, $"{nameof( OnVisibilityChanged )}: Visible = {visible}" );
//				}
//#endif

                // ウォッチフェイスの表示・非表示を判別します。
                if (visible)
                {
                    if (timeZoneReceiver == null)
                    {
                        timeZoneReceiver = new ActionExecutableBroadcastReceiver(
                            intent =>
                            {
                                // Time ( Android )
                                //nowTime.Clear( intent.GetStringExtra( "time-zone" ) );
                                //nowTime.SetToNow();
                                // Calendar ( Java )
                                nowTime = Java.Util.Calendar.GetInstance(Java.Util.TimeZone.GetTimeZone(intent.GetStringExtra("time-zone")));
                                // DateTime ( C# )
                                //nowTime = DateTime.Now;
                            },
                            Intent.ActionTimezoneChanged
                        );
                    }
                    // タイムゾーン用のレシーバーを登録します。
                    timeZoneReceiver.IsRegistered = true;
                    // ウォッチフェイスが非表示の時にタイムゾーンが変化した場合のために、タイムゾーンを更新します。
                    // Time ( Android )
                    //nowTime.Clear( Java.Util.TimeZone.Default.ID );
                    //nowTime.SetToNow();
                    // Calendar ( Java )
                    nowTime = Java.Util.Calendar.GetInstance(Java.Util.TimeZone.Default);
                    // DateTime ( C# )
                    //nowTime = DateTime.Now;
                }
                else
                {
                    // タイムゾーン用のレシーバーを登録解除します。
                    timeZoneReceiver.IsRegistered = false;
                }
                // タイマーの動作を更新します。
                UpdateTimer();
            }

            /// <summary>
            ///		タイマーの動作を更新します。
            /// </summary>
            private void UpdateTimer()
            {
//#if DEBUG
//				if( Log.IsLoggable( logTag, LogPriority.Info ) ) {
//					Log.Info( logTag, $"{nameof( UpdateTimer )}" );
//				}
//#endif

                // UpdateTimeHandlerからMessageUpdateTimeメッセージを取り除きます。
                updateTimeHandler.RemoveMessages(MessageUpdateTime);
                // タイマーを動作させるかどうかを判別します。
                if (ShouldTimerBeRunning)
                {
                    // UpdateTimeHandlerにMessageUpdateTimeメッセージをセットします。
                    updateTimeHandler.SendEmptyMessage(MessageUpdateTime);
                }
            }

            /// <summary>
            ///		タイマーを動作させるかどうかを表す値を取得します。
            /// </summary>
            private bool ShouldTimerBeRunning =>
                IsVisible && !IsInAmbientMode;
        }
    }
}