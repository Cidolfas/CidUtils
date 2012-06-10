using UnityEngine;
using System.Collections;

/// <summary>
/// The Networking Base is an abstract class that's designed to be the base
/// class of your main game manager class. Implement everything there, including
/// the UI where you can set the server info. Then use the functions inherited from
/// this class to set up the networking.
/// </summary>
public abstract class NetworkingBase : MonoBehaviour {
	
	// Server to join variables
	protected string m_joinServerIP = "127.0.0.1";
	protected string[] m_joinServerIPs = new string[0];
	protected int m_joinServerPort = 27182;
	protected string m_joinServerPassword = "";
	
	// Server to host variables
	protected string m_serverName = "Default Server Name";
	protected string m_serverComment = "";
	protected int m_serverPort = 27182;
	protected bool m_serverUseNAT = false;
	protected int m_serverPlayerLimit = 2;
	protected string m_serverPassword = "";
	protected bool m_serverDedicated = true;
	
	// Player information variables
	protected string m_playerName = "Name";
	
	// Master server information
	// Write your own values in your child class if they're different
	protected bool m_usesMasterServer = false;
	protected string m_masterServerGameName = "CidTest"; // Change this!
	protected HostData[] m_masterServerList = new HostData[0];
	private float m_masterServerMaxRefreshRate = 5f;
	private float m_masterServerNextRefreshTime = 0f;
	
	public void GetServerPrefs ()
	{
		// Join
		if (PlayerPrefs.GetString ("joinServerIP") != "")
			m_joinServerIP = PlayerPrefs.GetString ("joinServerIP");
		if (PlayerPrefs.GetInt ("joinServerPort") != 0)
			m_joinServerPort = PlayerPrefs.GetInt ("joinServerPort");
		
		// Host
		if (PlayerPrefs.GetString ("serverName") != "")
			m_serverName = PlayerPrefs.GetString ("serverName");
		if (PlayerPrefs.GetInt ("serverPort") != 0)
			m_serverPort = PlayerPrefs.GetInt ("serverPort");
		if (PlayerPrefs.GetInt ("serverLimit") != 0)
			m_serverPlayerLimit = PlayerPrefs.GetInt ("serverLimit");
		if (PlayerPrefs.GetInt ("serverNAT") != 0)
			m_serverUseNAT = true;
		if (PlayerPrefs.GetString ("serverPassword") != "")
			m_serverPassword = PlayerPrefs.GetString ("serverPassword");
		if (PlayerPrefs.GetInt ("serverDedicated") != 0)
			m_serverDedicated = true;
		
		// Player
		if (PlayerPrefs.GetString ("playerName") != "")
			m_playerName = PlayerPrefs.GetString ("playerName");
	}
	
	public void RequestServerList ()
	{
		if (CanUpdateSeverList ()) {
			m_masterServerNextRefreshTime = Time.time + m_masterServerMaxRefreshRate;
			MasterServer.RequestHostList (m_masterServerGameName);
		}
	}
	
	public bool CanUpdateSeverList ()
	{
		return Time.time > m_masterServerNextRefreshTime;
	}
	
	private IEnumerator GetAndWriteServerList ()
	{
		float startTime = Time.time;
		
		while (MasterServer.PollHostList().Length == 0) {
			if (Time.time > startTime + m_masterServerMaxRefreshRate)
				break;
			yield return null;
		}
		m_masterServerList = MasterServer.PollHostList ();
		
	}
	
	void OnFailedToConnectToMasterServer (NetworkConnectionError info)
	{
		Debug.Log ("Could not connect to master server: " + info);
	}
	
	void OnServerInitialized ()
	{
		
	}

	void OnMasterServerEvent (MasterServerEvent msEvent)
	{
		switch (msEvent) {
		case MasterServerEvent.HostListReceived:
			m_masterServerList = MasterServer.PollHostList ();
			break;
			
		default:
			Debug.Log (msEvent);
			break;
		}
	}
	
	public NetworkConnectionError StartServer ()
	{
		if (m_serverName == "") {
			m_serverName = "Server";
		}
		
		if (m_serverPassword != "") {
			Network.incomingPassword = m_serverPassword;
			PlayerPrefs.SetString ("serverPassword", m_serverPassword);
		} else {
			Network.incomingPassword = "";
		}
		
		NetworkConnectionError error;
		
		m_serverUseNAT = !Network.HavePublicAddress ();
		
		// Make sure to account for the server player if we're not running a dedicated server!
		int numConnections = (m_serverDedicated) ? m_serverPlayerLimit : m_serverPlayerLimit - 1;
		error = Network.InitializeServer (numConnections, m_serverPort, m_serverUseNAT);
		
		if (m_usesMasterServer) {
			MasterServer.RegisterHost (m_masterServerGameName, m_serverName, m_serverComment);
		}
		
		PlayerPrefs.SetString ("serverName", m_serverName);
		PlayerPrefs.SetInt ("serverPort", m_serverPort);
		PlayerPrefs.SetInt ("serverLimit", m_serverPlayerLimit);
		PlayerPrefs.SetInt ("serverNAT", (m_serverUseNAT) ? 1 : 0);
		PlayerPrefs.SetInt ("serverDedicated", (m_serverDedicated) ? 1 : 0);
		
		return error;
	}
	
	public void CloseServer ()
	{
		Network.Disconnect ();
		MasterServer.UnregisterHost ();
	}
	
	public NetworkConnectionError JoinServerByIP ()
	{
		if (m_playerName == "") {
			m_playerName = "Player";
		}
		
		NetworkConnectionError error;
		
		if (m_joinServerPassword != "")
			error = Network.Connect (m_joinServerIP, m_joinServerPort, m_joinServerPassword);
		else
			error = Network.Connect (m_joinServerIP, m_joinServerPort);
		
		PlayerPrefs.SetString ("joinServerIP", m_joinServerIP);
		PlayerPrefs.SetInt ("joinServerPort", m_joinServerPort);
		
		return error;
	}
	
	public NetworkConnectionError JoinServerByIPs ()
	{
		if (m_playerName == "") {
			m_playerName = "Player";
		}
		
		NetworkConnectionError error;
		
		if (m_joinServerPassword != "")
			error = Network.Connect (m_joinServerIPs, m_joinServerPort, m_joinServerPassword);
		else
			error = Network.Connect (m_joinServerIPs, m_joinServerPort);
		
		return error;
	}
	
	public void QuitServer ()
	{
		Network.Disconnect ();
	}
}
