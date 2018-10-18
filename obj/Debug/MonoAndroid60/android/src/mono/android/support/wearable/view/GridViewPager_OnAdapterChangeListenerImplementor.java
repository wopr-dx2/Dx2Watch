package mono.android.support.wearable.view;


public class GridViewPager_OnAdapterChangeListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.wearable.view.GridViewPager.OnAdapterChangeListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onAdapterChanged:(Landroid/support/wearable/view/GridPagerAdapter;Landroid/support/wearable/view/GridPagerAdapter;)V:GetOnAdapterChanged_Landroid_support_wearable_view_GridPagerAdapter_Landroid_support_wearable_view_GridPagerAdapter_Handler:Android.Support.Wearable.Views.GridViewPager/IOnAdapterChangeListenerInvoker, Xamarin.Android.Wearable\n" +
			"n_onDataSetChanged:()V:GetOnDataSetChangedHandler:Android.Support.Wearable.Views.GridViewPager/IOnAdapterChangeListenerInvoker, Xamarin.Android.Wearable\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Wearable.Views.GridViewPager+IOnAdapterChangeListenerImplementor, Xamarin.Android.Wearable", GridViewPager_OnAdapterChangeListenerImplementor.class, __md_methods);
	}


	public GridViewPager_OnAdapterChangeListenerImplementor ()
	{
		super ();
		if (getClass () == GridViewPager_OnAdapterChangeListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Wearable.Views.GridViewPager+IOnAdapterChangeListenerImplementor, Xamarin.Android.Wearable", "", this, new java.lang.Object[] {  });
	}


	public void onAdapterChanged (android.support.wearable.view.GridPagerAdapter p0, android.support.wearable.view.GridPagerAdapter p1)
	{
		n_onAdapterChanged (p0, p1);
	}

	private native void n_onAdapterChanged (android.support.wearable.view.GridPagerAdapter p0, android.support.wearable.view.GridPagerAdapter p1);


	public void onDataSetChanged ()
	{
		n_onDataSetChanged ();
	}

	private native void n_onDataSetChanged ();

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
