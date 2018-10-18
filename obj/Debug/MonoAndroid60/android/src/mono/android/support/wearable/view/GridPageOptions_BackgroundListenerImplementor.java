package mono.android.support.wearable.view;


public class GridPageOptions_BackgroundListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.wearable.view.GridPageOptions.BackgroundListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_notifyBackgroundChanged:()V:GetNotifyBackgroundChangedHandler:Android.Support.Wearable.Views.IGridPageOptionsBackgroundListenerInvoker, Xamarin.Android.Wearable\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Wearable.Views.IGridPageOptionsBackgroundListenerImplementor, Xamarin.Android.Wearable", GridPageOptions_BackgroundListenerImplementor.class, __md_methods);
	}


	public GridPageOptions_BackgroundListenerImplementor ()
	{
		super ();
		if (getClass () == GridPageOptions_BackgroundListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Wearable.Views.IGridPageOptionsBackgroundListenerImplementor, Xamarin.Android.Wearable", "", this, new java.lang.Object[] {  });
	}


	public void notifyBackgroundChanged ()
	{
		n_notifyBackgroundChanged ();
	}

	private native void n_notifyBackgroundChanged ();

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
