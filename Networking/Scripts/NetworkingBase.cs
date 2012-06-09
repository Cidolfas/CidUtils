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
	protected int m_joinServerPort = 27182;
	protected string m_joinServerPassword = "";
	
	// Server to host variables
	protected string m_serverName = "Default Server Name";
	protected int m_serverPort = 27182;
	protected bool m_serverUseNAT = false;
	protected int m_serverPlayerLimit = 2;
	protected string m_serverPassword = "";
	
	// Player information variables
	protected string m_playerName = "Name";
	
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
		
		// Player
		if (PlayerPrefs.GetString ("playerName") != "")
			m_playerName = PlayerPrefs.GetString ("playerName");
	}
	
	public void StartServer ()
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
		
		Network.InitializeServer (m_serverPlayerLimit, m_serverPort, m_serverUseNAT);
		
		PlayerPrefs.SetString ("serverName", m_serverName);
		PlayerPrefs.SetInt ("serverPort", m_serverPort);
		PlayerPrefs.SetInt ("serverLimit", m_serverPlayerLimit);
		PlayerPrefs.SetInt ("serverNAT", (m_serverUseNAT) ? 1 : 0);
	}
	
	public void CloseServer ()
	{
		Network.Disconnect ();
	}
	
	public void JoinServerByIP ()
	{
		if (m_playerName == "") {
			m_playerName = "Player";
		}
		
		if (m_joinServerPassword != "")
			Network.Connect (m_joinServerIP, m_joinServerPort, m_joinServerPassword);
		else
			Network.Connect (m_joinServerIP, m_joinServerPort);
		
		PlayerPrefs.SetString ("joinServerIP", m_joinServerIP);
		PlayerPrefs.SetInt ("joinServerPort", m_joinServerPort);
	}
	
	public void QuitServer ()
	{
		Network.Disconnect ();
	}
}
