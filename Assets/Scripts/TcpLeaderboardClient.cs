using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TcpLeaderboardClient : MonoBehaviour 
{
	#region private members
	private TcpClient socketConnection;
	private static Thread clientReceiveThread = null;
	private NameManager nameManager;
	private Timer timeManager;
	Byte[] bytes;
	[SerializeField] Text leaderboardText;
	[SerializeField] GameObject Leaderboard;
	#endregion

	void Awake ()
	{
		nameManager = FindObjectOfType<NameManager>();
		timeManager = FindObjectOfType<Timer>();
		clientReceiveThread = null;
	}

	void Update ()
	{
		if (Input.GetKeyUp(KeyCode.Alpha3))
		{
			SendMessage(0);
		}
		if (Input.GetKeyUp(KeyCode.Alpha4))
		{
			SendMessage(1);
		}
		if(clientReceiveThread == null)
		{
			clientReceiveThread = new Thread(ConnectToTcpServer);
			clientReceiveThread.Start();
		}
	}

	string CreateMessage()
	{
		return "add|" + nameManager.playerName + "|" + timeManager.GetTimeAsString();
	}

	string GetLeaderboard()
	{
		return "get";
	}

	private void ConnectToTcpServer ()
	{
		try
		{
			socketConnection = new TcpClient("localhost", 8052);
			bytes = new Byte[2048];
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}

	private void ListenForData() 
	{
		try
		{
			Debug.Log(1);
			using (NetworkStream stream = socketConnection.GetStream())
			{
				int length;

				length = stream.Read(bytes, 0, bytes.Length);
				var incommingData = new byte[length];
				Array.Copy(bytes, 0, incommingData, 0, length);
				string serverMessage = Encoding.ASCII.GetString(incommingData);
				string[] splitArray =  serverMessage.Split(char.Parse("|"));
				int index = 0;
				var message = "Leaderboard: \n";
				foreach ( var line in splitArray)
				{
					index++;
					message += index + ") ";
					message += line;
					message += "\n";
				}
				leaderboardText.text = message;
				Leaderboard.SetActive(true);
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}

	public void SendMessage(int type)
	{
		clientReceiveThread.Join();
		clientReceiveThread = null;
		if (socketConnection == null)
		{
			return;
		}
		try {
			NetworkStream stream = socketConnection.GetStream();
			if (stream.CanWrite)
			{
				string clientMessage;
				if(type == 0)
				{
					clientMessage = CreateMessage();
				}
				else
				{
					clientMessage = GetLeaderboard();

				}
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
				if(type == 1)
				{
					ListenForData();
				}
			}
		}
		catch (SocketException socketException) 
		{
			Debug.Log("Socket exception: " + socketException);
		}
	} 
}