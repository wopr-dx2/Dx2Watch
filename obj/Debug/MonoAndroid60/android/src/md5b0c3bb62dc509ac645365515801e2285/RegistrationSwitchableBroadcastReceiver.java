package md5b0c3bb62dc509ac645365515801e2285;


public abstract class RegistrationSwitchableBroadcastReceiver
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Chronoir_net.Chronica.WatchfaceExtension.RegistrationSwitchableBroadcastReceiver, Chronoir_net.Chronica.WatchfaceExtension", RegistrationSwitchableBroadcastReceiver.class, __md_methods);
	}


	public RegistrationSwitchableBroadcastReceiver ()
	{
		super ();
		if (getClass () == RegistrationSwitchableBroadcastReceiver.class)
			mono.android.TypeManager.Activate ("Chronoir_net.Chronica.WatchfaceExtension.RegistrationSwitchableBroadcastReceiver, Chronoir_net.Chronica.WatchfaceExtension", "", this, new java.lang.Object[] {  });
	}

	public RegistrationSwitchableBroadcastReceiver (java.lang.String p0)
	{
		super ();
		if (getClass () == RegistrationSwitchableBroadcastReceiver.class)
			mono.android.TypeManager.Activate ("Chronoir_net.Chronica.WatchfaceExtension.RegistrationSwitchableBroadcastReceiver, Chronoir_net.Chronica.WatchfaceExtension", "System.String, mscorlib", this, new java.lang.Object[] { p0 });
	}

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
