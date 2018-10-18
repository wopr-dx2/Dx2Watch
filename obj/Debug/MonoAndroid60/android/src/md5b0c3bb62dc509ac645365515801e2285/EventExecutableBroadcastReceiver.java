package md5b0c3bb62dc509ac645365515801e2285;


public class EventExecutableBroadcastReceiver
	extends md5b0c3bb62dc509ac645365515801e2285.RegistrationSwitchableBroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("Chronoir_net.Chronica.WatchfaceExtension.EventExecutableBroadcastReceiver, Chronoir_net.Chronica.WatchfaceExtension", EventExecutableBroadcastReceiver.class, __md_methods);
	}


	public EventExecutableBroadcastReceiver ()
	{
		super ();
		if (getClass () == EventExecutableBroadcastReceiver.class)
			mono.android.TypeManager.Activate ("Chronoir_net.Chronica.WatchfaceExtension.EventExecutableBroadcastReceiver, Chronoir_net.Chronica.WatchfaceExtension", "", this, new java.lang.Object[] {  });
	}

	public EventExecutableBroadcastReceiver (java.lang.String p0)
	{
		super ();
		if (getClass () == EventExecutableBroadcastReceiver.class)
			mono.android.TypeManager.Activate ("Chronoir_net.Chronica.WatchfaceExtension.EventExecutableBroadcastReceiver, Chronoir_net.Chronica.WatchfaceExtension", "System.String, mscorlib", this, new java.lang.Object[] { p0 });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

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
