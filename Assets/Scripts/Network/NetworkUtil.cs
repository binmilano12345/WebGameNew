//#if STATE_0
//using System.Threading;
//using System;
//using UnityEngine;
//using System.Collections;
//using System.Net.Sockets;
//using System.Runtime.InteropServices;
//using UnityEngine.SceneManagement;
//using AppConfig;

//public class NetworkUtil : MonoBehaviour {
//    private MessageHandler messageHandler;
//    public bool connected;
//    protected static NetworkUtil instance;
//    private Message message;
//    WebSocket w_socket = null;
//    ManualResetEvent _clientDone = new ManualResetEvent(false);
//    SocketAsyncEventArgs EventArgSend;
//    SocketAsyncEventArgs EventArgRead;
//    SocketAsyncEventArgs EventArgConnect;
//    protected Thread connectThread;
//    protected System.Threading.Thread receiveThread;
//    private int maxRetry = 1;

//    void Awake() {
//        if (instance == null) {
//            //If I am the first instance, make me the Singleton
//            instance = this;
//            DontDestroyOnLoad(this);
//        } else {
//            //If a Singleton already exists and you find
//            //another reference in scene, destroy it!
//            if (this != instance)
//                Destroy(this.gameObject);
//        }
//    }
//    string m_url = "";
//    public IEnumerator Start() {
//        //        if (m_url.Equals("")) {
//        //            WWW www = new WWW("http://choibaidoithuong.org/config");
//        //            yield return www;
//        //            m_url = www.text;
//        //        }
//        //#if UNITY_WEBGL
//        //        Application.ExternalCall("StartLoad");
//        //#endif
//        yield return StartCoroutine(doConnect());
//        yield return StartCoroutine(threadReceiveMSG());

//        if (connected)
//            SendData.onGetPhoneCSKH();
//    }

//    public static NetworkUtil GI() {
//        if (instance == null) {
//            instance = new NetworkUtil();
//        }
//        return instance;
//    }

//    public bool isConnected() {
//        return connected;
//    }

//    public void registerHandler(MessageHandler messageHandler) {
//        this.messageHandler = messageHandler;
//    }

//    public IEnumerator doConnect() {
//        if (!connected) {
//            m_url = "ws://" + GameConfig.IP + ":" + GameConfig.PORT;
//            w_socket = new WebSocket(new Uri(m_url));
//            yield return StartCoroutine(w_socket.Connect());
//            connected = true;
//        }
//    }


//    public void sendMessage(Message msg) {
//        try {
//            byte[] bytes = msg.toByteArray();
//            //#if UNITY_EDITOR
//            Debug.Log("Send : " + msg.command);
//            //#endif
//            w_socket.Send(bytes);
//        } catch (Exception ex) {
//            Debug.LogException(ex);
//        }
//    }

//    private void processMsgFromData(sbyte[] data, int range) {
//        sbyte command = 0;
//        int count = 0;
//        int size = 0;
//        try {
//            if (range <= 0)
//                return;
//            Message msg;
//            do {
//                command = data[count];
//                count++;
//                sbyte a1 = data[count];
//                count++;
//                sbyte a2 = data[count];
//                count++;
//                size = ((a1 & 0xff) << 8) | (a2 & 0xff);
//                byte[] subdata = new byte[size];
////#if UNITY_EDITOR
//                Debug.Log("Read == " + command);
////#endif
//                Buffer.BlockCopy(data, count, subdata, 0, size);
//                count += size;
//                msg = new Message(command, subdata);
//                messageHandler.processMessage(msg);
//            } while (count < range);
//        } catch (Exception ex) {
//            Debug.LogException(ex);
//            messageHandler.onDisconnected();
//        }
//    }

//    public void close() {
//#if UNITY_EDITOR
//        Debug.Log("Close current socket!");
//#endif
//        cleanNetwork();
//    }

//    public void cleanNetwork() {
//        try {
//            connected = false;
//            if (w_socket != null) {
//                try {
//                    w_socket.Close();
//                } catch (SocketException ex) {
//                    Debug.LogException(ex);
//                }

//            }
//            if (EventArgRead != null) {
//                EventArgRead.Dispose();
//            }
//            if (EventArgSend != null) {
//                EventArgSend.Dispose();
//            }
//            if (EventArgConnect != null) {
//                EventArgConnect.Dispose();
//            }
//            maxRetry = 1;
//            connectThread = null;
//            _clientDone.Close();
//            _clientDone = new ManualResetEvent(false);
//        } catch (Exception e) {
//            Debug.LogException(e);
//        } finally {
//            if (connectThread != null && connectThread.IsAlive) {
//                connectThread.Abort();
//            }
//        }
//    }

//    public void resume(bool pausestatus) {

//    }
//    public IEnumerator threadReceiveMSG() {
//        while (connected) {
//            try {
//                byte[] data = w_socket.Recv();
//                if (data != null) {
//                    sbyte[] sdata = new sbyte[data.Length];
//                    for (int i = 0; i < data.Length; i++) {
//                        if (data[0] > 127) {
//                            sdata[0] = (sbyte)(data[0] - 256);
//                        }
//                        sdata[i] = (sbyte)data[i];
//                    }
//                    processMsgFromData(sdata, sdata.Length);
//                }
//            } catch (Exception e) {
//                Debug.LogException(e);
//            }
//            if (w_socket.error != null) {
//                messageHandler.onDisconnected();
//                break;
//            }
//            yield return 0;
//        }
//        cleanNetwork();
//    }

//    void OnApplicationQuit() {
//        close();
//    }
//}
//#endif
//#if STATE_1
/*
using System.IO;
using System.Threading;
using System;
using UnityEngine;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Net.Sockets;
using AppConfig;

public class NetworkUtil {
	private MessageHandler messageHandler;
	public bool connected;
	protected static NetworkUtil instance;
	private Message message;
	Socket _socket = null;
	ManualResetEvent _clientDone = new ManualResetEvent(false);
	SocketAsyncEventArgs EventArgSend;
	SocketAsyncEventArgs EventArgRead;
	SocketAsyncEventArgs EventArgConnect;
	protected Thread connectThread;
	private int maxRetry = 1;

	public static NetworkUtil GI() {
		if (instance == null) {
			instance = new NetworkUtil();
		}
		return instance;
	}

	public bool isConnected() {

		return connected;
	}

	public void registerHandler(MessageHandler messageHandler) {
		this.messageHandler = messageHandler;
	}

	public void connect(Message message) {
		if (!connected) {
			if (connectThread != null) {
				if (connectThread.IsAlive) {
					return;
				}
			}
			this.message = message;
			connectThread = new Thread(new ThreadStart(runConnect));
			connectThread.Start();
		} else {
			if (message != null) {
				sendMessage(message);
			}
		}
	}

	private void runConnect() {
		try {
			EndPoint hostEntry = new IPEndPoint(Dns.GetHostAddresses(GameConfig.IP)[0], GameConfig.PORT);
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			EventArgSend = new SocketAsyncEventArgs();
			EventArgRead = new SocketAsyncEventArgs();
			EventArgConnect = new SocketAsyncEventArgs();
			EventArgRead.Completed += new EventHandler<SocketAsyncEventArgs>(Read_Completed);
			EventArgConnect.Completed += new EventHandler<SocketAsyncEventArgs>(Connect_Completed);
			EventArgConnect.RemoteEndPoint = hostEntry;
			byte[] readBuffer = new byte[64 * 1024];
			EventArgRead.RemoteEndPoint = hostEntry;
			EventArgRead.SetBuffer(readBuffer, 0, readBuffer.Length);
			EventArgRead.UserToken = _socket;

			EventArgSend.RemoteEndPoint = hostEntry;
			EventArgSend.UserToken = null;
		} catch (Exception ex) {
			Debug.Log("Unable to connect to internet!" + ex);
			return;
		}
		Connect();
	}

	public void Connect() {
		Debug.Log("========try connect=========== : " + GameConfig.IP);
		_socket.ConnectAsync(EventArgConnect);
		_clientDone.WaitOne(5000);
		if (!connected && maxRetry < 4) {
			maxRetry++;
			Connect();
		} else if (!connected && maxRetry >= 4) {
			close();
		}
	}
	private void Connect_Completed(object sender, SocketAsyncEventArgs e) {
		switch (e.LastOperation) {
		case SocketAsyncOperation.Connect:
			ProcessConnect(e);
			break;
		}
	}
	private void Read_Completed(object sender, SocketAsyncEventArgs e) {
		switch (e.LastOperation) {
		case SocketAsyncOperation.Receive:
			ProcessReceive(e);
			break;
		}
	}
	private void ProcessConnect(SocketAsyncEventArgs e) {
		if (e.SocketError == SocketError.Success) {
			connected = true;
			maxRetry = 1;
			_clientDone.Set();
			messageHandler.onConnectOk();
			_socket.NoDelay = true;
			//_socket.ReceiveTimeout = 60000;
			_socket.ReceiveAsync(EventArgRead);
			if (message != null) {
				sendMessage(message);
			}
			Debug.LogError("Connected " + connected);
		}
	}

	private void ProcessReceive(SocketAsyncEventArgs e) {
		//Debug.LogError(e.BytesTransferred + " " + (_socket == null));
		if (e.BytesTransferred > 0) {
			if (e.SocketError == SocketError.Success) {
				// Retrieve the data from the buffer
				processMsgFromData(e.Buffer, e.BytesTransferred);
				byte[] readBuffer = new byte[64 * 1024];
				e.SetBuffer(readBuffer, 0, readBuffer.Length);
				_socket.ReceiveAsync(e);
			} else if (e.SocketError == SocketError.ConnectionAborted
				|| e.SocketError == SocketError.ConnectionReset
				|| e.SocketError == SocketError.TimedOut) {
				if (connected) {
					if (messageHandler != null) {
						messageHandler.onDisconnected();
					}
					cleanNetwork();
				}
			}
		} else {
			if (connected) {
				if (messageHandler != null) {
					messageHandler.onDisconnected();
				}
				cleanNetwork();
			}
		}
	}

	public void sendMessage(Message msg) {
		try {
			byte[] bytes = msg.toByteArray();
			EventArgSend.SetBuffer(bytes, 0, bytes.Length);
			Debug.Log("Send : " + msg.command);
			_socket.SendAsync(EventArgSend);
		} catch (Exception ex) {
			Debug.LogException(ex);
		}
	}

	private void processMsgFromData(byte[] data, int range) {
		//List<Message> listMsg = new List<Message>();
		sbyte command = 0;
		int count = 0;
		int size = 0;
		try {
			if (range <= 0)
				return;
			Message msg;
			do {
				command = (sbyte)data[count];
				count++;
				sbyte a1 = (sbyte)data[count];
				count++;
				sbyte a2 = (sbyte)data[count];
				count++;
				size = ((a1 & 0xff) << 8) | (a2 & 0xff);
				byte[] subdata = new byte[size];
				Debug.Log("Read == " + command);
				Buffer.BlockCopy(data, count, subdata, 0, size);
				count += size;
				msg = new Message(command, subdata);
				//listMsg.Add(msg);
				messageHandler.processMessage(msg);
				Thread.Sleep(70);
			} while (count < range);
		} catch (Exception ex) {
			Debug.LogException(ex);
		}
	}

	public void close() {
		Debug.Log("Close current socket!");
		cleanNetwork();
	}

	public void cleanNetwork() {
		try {
			connected = false;
			if (_socket != null) {
				try {
					_socket.Close();
				} catch (SocketException ex) {
					Debug.LogException(ex);
				}

			}
			if (EventArgRead != null) {
				EventArgRead.Dispose();
			}
			if (EventArgSend != null) {
				EventArgSend.Dispose();
			}
			if (EventArgConnect != null) {
				EventArgConnect.Dispose();
			}
			maxRetry = 1;
			connectThread = null;
			_clientDone.Close();
			_clientDone = new ManualResetEvent(false);
		} catch (Exception e) {
			Debug.LogException(e);
		} finally {
			if (connectThread != null && connectThread.IsAlive) {
				connectThread.Abort();
			}
		}
	}

	public void resume(bool pausestatus) {
		//if (pausestatus) {

		//}
		//else {
		//    if (GameControl.instance.currenStage != GameControl.instance.login) {
		//        GameControl.instance.setStage(GameControl.instance.login);
		//        GameControl.instance.disableAllDialog();
		//        close();
		//        if (!BaseInfo.gI().username.Equals("")) {
		//            GameControl.instance.login.doLogin(BaseInfo.gI().username, BaseInfo.gI().pass);
		//        }
		//    }
		//}

	}
}
*/

using System.IO;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using AppConfig;

public class NetworkUtil {
    private MessageHandler messageHandler;
    public bool connected;
    protected static NetworkUtil instance;
    private Message message;
    Socket _socket = null;
    ManualResetEvent _clientDone = new ManualResetEvent(false);
    protected Thread connectThread;
    protected Thread pingThread;
    private int maxRetry = 1;

    public static NetworkUtil GI() {
        if (instance == null) {
            instance = new NetworkUtil();
        }
        return instance;

    }

    public bool isConnected() {

        return connected;
    }

    public void registerHandler(MessageHandler messageHandler) {
        this.messageHandler = messageHandler;
    }

    public void connect(Message message) {
        if (!connected) {
            if (connectThread != null) {
                if (connectThread.IsAlive) {
                    return;
                }
            }
            this.message = message;
            connectThread = new Thread(new ThreadStart(runConnect));
            connectThread.Start();

        } else {
            if (message != null) {
                sendMessage(message);
            }

        }
    }

    Stream stream;
    TcpClient client;

    private void runConnect() {
        try {
            client = new TcpClient();
            client.Connect(GameConfig.IP, GameConfig.PORT);

            Connect();
            if (connected) {
                stream = client.GetStream();
                stream.WriteTimeout = 5;
                connectThread = new Thread(new ThreadStart(threadReceiveMSG));
                connectThread.Start();
                if (message != null)
                    sendMessage(message);
                //pingThread = new Thread(new ThreadStart(ping));
                //pingThread.Start();
            } else {
                if (messageHandler != null) {
                    messageHandler.onDisconnected();
                }
                close();
            }
        } catch (Exception ex) {
            Debug.Log("Unable to connect to internet!" + ex);
            close();
            return;
        }
        //Connect();
    }

    public void Connect() {
        Debug.Log("========try connect=========== : " + GameConfig.IP);
        while (!connected) {
            if (!connected && maxRetry < 100) {
                maxRetry++;
                connected = client.Connected;
                Thread.Sleep(100);
            } else
                break;
        }


    }

    public void threadReceiveMSG() {
        while (connected) {

            stream = client.GetStream();
            if (client.Available > 0) {
                try {

                    byte[] data = new byte[client.Available];

                    int bytesread = stream.Read(data, 0, data.Length);
                    byte[] sdata = new byte[data.Length];
                    for (int i = 0; i < data.Length; i++) {
                        if (data[0] > 127) {
                            sdata[0] = (byte)(data[0] - 256);
                        }
                        sdata[i] = (byte)data[i];
                    }
                    processMsgFromData(sdata, data.Length);
                } catch (Exception e) {
                    if (messageHandler != null) {
                        messageHandler.onDisconnected();
                    }
                    cleanNetwork();
                }
            } else {
                bool b = true;
                if ((client.Client.Poll(0, SelectMode.SelectWrite)) && (!client.Client.Poll(0, SelectMode.SelectError))) {
                    byte[] buffer = new byte[1];
                    if (client.Client.Receive(buffer, SocketFlags.Peek) == 0) {
                        b = false;
                    } else {
                        b = true;
                    }
                } else {
                    b = false;
                }
                if (!b) {
                    // Client disconnected
                    if (messageHandler != null) {
                        messageHandler.onDisconnected();
                    }
                    cleanNetwork();
                }
            }
        }
    }

    private byte[] msgNotFull = null;
    private sbyte commandNotFull;
    private int sizeNotFull = 0;

    private void processMsgFromData(byte[] data, int range) {
        //List<Message> listMsg = new List<Message>();
        sbyte command = 0;
        int count = 0;
        int size = 0;
        try {
            if (range <= 0)
                return;
            Message msg;
            if (msgNotFull == null) {
                do {
                    command = (sbyte)data[count];
                    Debug.Log("Read: " + command);
                    count++;
                    sbyte a1 = (sbyte)data[count];
                    count++;
                    sbyte a2 = (sbyte)data[count];
                    count++;
                    size = ((a1 & 0xff) << 8) | (a2 & 0xff);
                    if (size > data.Length - count) {
                        byte[] subdata = new byte[data.Length - count];
                        //						for (int i = count; i < data.Length; i++) {
                        //							subdata [i-count] = data [i];
                        //						}
                        Buffer.BlockCopy(data, count, subdata, 0, subdata.Length);
                        count += size;
                        msgNotFull = subdata;
                        commandNotFull = command;
                        sizeNotFull = size;
                    } else {
                        byte[] subdata = new byte[size];
                        //						for (int i = count; i < data.Length; i++) {
                        //							subdata [i-count] = data [i];
                        //						}
                        Buffer.BlockCopy(data, count, subdata, 0, size);
                        count += size;
                        msg = new Message(command, subdata);
                        messageHandler.processMessage(msg);
                    }

                    Thread.Sleep(70);
                } while (count < range);
            } else {
                if (sizeNotFull > data.Length + msgNotFull.Length) {
                    byte[] subdata = new byte[data.Length + msgNotFull.Length];

                    int sizeB = msgNotFull.Length;
                    for (int i = 0; i < sizeB; i++) {
                        subdata[i] = msgNotFull[i];
                    }
                    for (int i = sizeB; i < subdata.Length; i++) {
                        subdata[i] = data[i - sizeB];
                    }
                    //					Buffer.BlockCopy (msgNotFull, 0, subdata, 0, msgNotFull.Length);
                    //					Buffer.BlockCopy (data, 0, subdata, 0, data.Length);
                    msgNotFull = subdata;
                } else {
                    byte[] subdata = new byte[sizeNotFull];
                    int sizeB = msgNotFull.Length;
                    for (int i = 0; i < sizeB; i++) {
                        subdata[i] = msgNotFull[i];
                    }
                    for (int i = sizeB; i < sizeNotFull; i++) {
                        subdata[i] = data[i - sizeB];
                    }
                    msg = new Message(commandNotFull, subdata);
                    messageHandler.processMessage(msg);
                    msgNotFull = null;
                }


            }
        } catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public void sendMessage(Message msg) {
        try {
            byte[] b = msg.toByteArray();
            stream.Write(b, 0, b.Length);
            Debug.Log("Send: " + msg.command);

        } catch (Exception ex) {
            Debug.LogException(ex);
            if (messageHandler != null)
                messageHandler.onDisconnected();
            close();
        }
    }



    public void close() {
        Debug.Log("Close current socket!");
        cleanNetwork();
    }

    public void cleanNetwork() {
        try {
            connected = false;
            if (client != null)
                client.Close();
            if (_socket != null) {
                try {
                    _socket.Close();
                } catch (SocketException ex) {
                    Debug.LogException(ex);
                }

            }

            maxRetry = 1;
            connectThread = null;
        } catch (Exception e) {
            Debug.LogException(e);
        } finally {
            if (connectThread != null && connectThread.IsAlive) {
                connectThread.Abort();
            }
        }
    }

    public void resume(bool pausestatus) {
        //if (pausestatus) {

        //}
        //else {
        //    if (GameControl.instance.currenStage != GameControl.instance.login) {
        //        GameControl.instance.setStage(GameControl.instance.login);
        //        GameControl.instance.disableAllDialog();
        //        close();
        //        if (!BaseInfo.gI().username.Equals("")) {
        //            GameControl.instance.login.doLogin(BaseInfo.gI().username, BaseInfo.gI().pass);
        //        }
        //    }
        //}
    }
}