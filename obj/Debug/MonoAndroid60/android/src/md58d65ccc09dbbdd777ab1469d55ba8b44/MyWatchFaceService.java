package md58d65ccc09dbbdd777ab1469d55ba8b44;


public class MyWatchFaceService
	extends android.support.wearable.watchface.CanvasWatchFaceService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static final String __md_1_methods;
	static {
		__md_methods = 
			"n_onCreateEngine:()Landroid/support/wearable/watchface/CanvasWatchFaceService$Engine;:GetOnCreateEngineHandler\n" +
			"";
		mono.android.Runtime.register ("Dx2Watch.MyWatchFaceService, Dx2Watch", MyWatchFaceService.class, __md_methods);
		__md_1_methods = 
			"n_onCreate:(Landroid/view/SurfaceHolder;)V:GetOnCreate_Landroid_view_SurfaceHolder_Handler\n" +
			"n_onDestroy:()V:GetOnDestroyHandler\n" +
			"n_onApplyWindowInsets:(Landroid/view/WindowInsets;)V:GetOnApplyWindowInsets_Landroid_view_WindowInsets_Handler\n" +
			"n_onPropertiesChanged:(Landroid/os/Bundle;)V:GetOnPropertiesChanged_Landroid_os_Bundle_Handler\n" +
			"n_onTimeTick:()V:GetOnTimeTickHandler\n" +
			"n_onAmbientModeChanged:(Z)V:GetOnAmbientModeChanged_ZHandler\n" +
			"n_onInterruptionFilterChanged:(I)V:GetOnInterruptionFilterChanged_IHandler\n" +
			"n_onTapCommand:(IIIJ)V:GetOnTapCommand_IIIJHandler\n" +
			"n_onDraw:(Landroid/graphics/Canvas;Landroid/graphics/Rect;)V:GetOnDraw_Landroid_graphics_Canvas_Landroid_graphics_Rect_Handler\n" +
			"n_onVisibilityChanged:(Z)V:GetOnVisibilityChanged_ZHandler\n" +
			"";
		mono.android.Runtime.register ("Dx2Watch.MyWatchFaceService+MyWatchFaceEngine, Dx2Watch", MyWatchFaceService_MyWatchFaceEngine.class, __md_1_methods);
	}


	public MyWatchFaceService ()
	{
		super ();
		if (getClass () == MyWatchFaceService.class)
			mono.android.TypeManager.Activate ("Dx2Watch.MyWatchFaceService, Dx2Watch", "", this, new java.lang.Object[] {  });
	}


	public android.support.wearable.watchface.CanvasWatchFaceService.Engine onCreateEngine ()
	{
		return n_onCreateEngine ();
	}

	private native android.support.wearable.watchface.CanvasWatchFaceService.Engine n_onCreateEngine ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}

public class MyWatchFaceService_MyWatchFaceEngine
	extends android.support.wearable.watchface.CanvasWatchFaceService.Engine
	implements
		mono.android.IGCUserPeer
{


	public MyWatchFaceService_MyWatchFaceEngine ()
	{
		super ();
		if (getClass () == MyWatchFaceService_MyWatchFaceEngine.class)
			mono.android.TypeManager.Activate ("Dx2Watch.MyWatchFaceService+MyWatchFaceEngine, Dx2Watch", "Dx2Watch.MyWatchFaceService, Dx2Watch", this, new java.lang.Object[] { MyWatchFaceService.this });
	}


	public void onCreate (android.view.SurfaceHolder p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.view.SurfaceHolder p0);


	public void onDestroy ()
	{
		n_onDestroy ();
	}

	private native void n_onDestroy ();


	public void onApplyWindowInsets (android.view.WindowInsets p0)
	{
		n_onApplyWindowInsets (p0);
	}

	private native void n_onApplyWindowInsets (android.view.WindowInsets p0);


	public void onPropertiesChanged (android.os.Bundle p0)
	{
		n_onPropertiesChanged (p0);
	}

	private native void n_onPropertiesChanged (android.os.Bundle p0);


	public void onTimeTick ()
	{
		n_onTimeTick ();
	}

	private native void n_onTimeTick ();


	public void onAmbientModeChanged (boolean p0)
	{
		n_onAmbientModeChanged (p0);
	}

	private native void n_onAmbientModeChanged (boolean p0);


	public void onInterruptionFilterChanged (int p0)
	{
		n_onInterruptionFilterChanged (p0);
	}

	private native void n_onInterruptionFilterChanged (int p0);


	public void onTapCommand (int p0, int p1, int p2, long p3)
	{
		n_onTapCommand (p0, p1, p2, p3);
	}

	private native void n_onTapCommand (int p0, int p1, int p2, long p3);


	public void onDraw (android.graphics.Canvas p0, android.graphics.Rect p1)
	{
		n_onDraw (p0, p1);
	}

	private native void n_onDraw (android.graphics.Canvas p0, android.graphics.Rect p1);


	public void onVisibilityChanged (boolean p0)
	{
		n_onVisibilityChanged (p0);
	}

	private native void n_onVisibilityChanged (boolean p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
}
