package mono.android.support.wearable.view;


public class WearableListView_OnCenterProximityListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.wearable.view.WearableListView.OnCenterProximityListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCenterPosition:(Z)V:GetOnCenterPosition_ZHandler:Android.Support.Wearable.Views.WearableListView/IOnCenterProximityListenerInvoker, Xamarin.Android.Wearable\n" +
			"n_onNonCenterPosition:(Z)V:GetOnNonCenterPosition_ZHandler:Android.Support.Wearable.Views.WearableListView/IOnCenterProximityListenerInvoker, Xamarin.Android.Wearable\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Wearable.Views.WearableListView+IOnCenterProximityListenerImplementor, Xamarin.Android.Wearable", WearableListView_OnCenterProximityListenerImplementor.class, __md_methods);
	}


	public WearableListView_OnCenterProximityListenerImplementor ()
	{
		super ();
		if (getClass () == WearableListView_OnCenterProximityListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Wearable.Views.WearableListView+IOnCenterProximityListenerImplementor, Xamarin.Android.Wearable", "", this, new java.lang.Object[] {  });
	}


	public void onCenterPosition (boolean p0)
	{
		n_onCenterPosition (p0);
	}

	private native void n_onCenterPosition (boolean p0);


	public void onNonCenterPosition (boolean p0)
	{
		n_onNonCenterPosition (p0);
	}

	private native void n_onNonCenterPosition (boolean p0);

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
