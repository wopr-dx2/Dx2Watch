package mono.android.support.wearable.view;


public class ObservableScrollView_OnScrollListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.wearable.view.ObservableScrollView.OnScrollListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onScroll:(F)V:GetOnScroll_FHandler:Android.Support.Wearable.Views.ObservableScrollView/IOnScrollListenerInvoker, Xamarin.Android.Wearable\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Wearable.Views.ObservableScrollView+IOnScrollListenerImplementor, Xamarin.Android.Wearable", ObservableScrollView_OnScrollListenerImplementor.class, __md_methods);
	}


	public ObservableScrollView_OnScrollListenerImplementor ()
	{
		super ();
		if (getClass () == ObservableScrollView_OnScrollListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Wearable.Views.ObservableScrollView+IOnScrollListenerImplementor, Xamarin.Android.Wearable", "", this, new java.lang.Object[] {  });
	}


	public void onScroll (float p0)
	{
		n_onScroll (p0);
	}

	private native void n_onScroll (float p0);

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
