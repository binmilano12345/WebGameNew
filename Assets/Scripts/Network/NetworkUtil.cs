using System.IO;
using System.Threading;
using System;
using UnityEngine;
using System.Net.Sockets;
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
            //Debug.Log("Send: " + msg.command);

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