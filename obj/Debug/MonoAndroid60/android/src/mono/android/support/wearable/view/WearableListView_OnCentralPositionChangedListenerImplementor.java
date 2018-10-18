package mono.android.support.wearable.view;


public class WearableListView_OnCentralPositionChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.wearable.view.WearableListView.OnCentralPositionChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCentralPositionChanged:(I)V:GetOnCentralPositionChanged_IHandler:Android.Support.Wearable.Views.WearableListView/IOnCentralPositionChangedListenerInvoker, Xamarin.Android.Wearable\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Wearable.Views.WearableListView+IOnCentralPositionChangedListenerImplementor, Xamarin.Android.Wearable", WearableListView_OnCentralPositionChangedListenerImplementor.class, __md_methods);
	}


	public WearableListView_OnCentralPositionChangedListenerImplementor ()
	{
		super ();
		if (getClass () == WearableListView_OnCentralPositionChangedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Wearable.Views.WearableListView+IOnCentralPositionChangedListenerImplementor, Xamarin.Android.Wearable", "", this, new java.lang.Object[] {  });
	}


	public void onCentralPositionChanged (int p0)
	{
		n_onCentralPositionChanged (p0);
	}

	private native void n_onCentralPositionChanged (int p0);

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
