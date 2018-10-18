package mono.android.support.wearable.view;


public class WearableListView_OnOverScrollListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.wearable.view.WearableListView.OnOverScrollListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onOverScroll:()V:GetOnOverScrollHandler:Android.Support.Wearable.Views.WearableListView/IOnOverScrollListenerInvoker, Xamarin.Android.Wearable\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Wearable.Views.WearableListView+IOnOverScrollListenerImplementor, Xamarin.Android.Wearable", WearableListView_OnOverScrollListenerImplementor.class, __md_methods);
	}


	public WearableListView_OnOverScrollListenerImplementor ()
	{
		super ();
		if (getClass () == WearableListView_OnOverScrollListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Wearable.Views.WearableListView+IOnOverScrollListenerImplementor, Xamarin.Android.Wearable", "", this, new java.lang.Object[] {  });
	}


	public void onOverScroll ()
	{
		n_onOverScroll ();
	}

	private native void n_onOverScroll ();

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
