using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UdpReceiver : MonoBehaviour
{
    public int listenPort = 12345;
    public string returnData;

    private UdpClient udpClient;
    private Thread receiveThread;
    // volatile keyword ensures that the isReceiving variable is accessed from main memory
    // and not from the thread's cache, which is important for thread safety.
    private volatile bool isReceiving = false;

    void Start()
    {
        udpClient = new UdpClient(listenPort);
        isReceiving = true;
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        Debug.Log("UDP Receiver started on port: " + listenPort);
    }

    private void ReceiveData()
    {
        while (isReceiving)
        {
            try
            {
                IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receiveBytes = udpClient.Receive(ref remoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);

                // IMPORTANT: You cannot call Unity API functions from this thread.
                // You must queue data and process it in the main thread's Update() method.
                //Debug.Log("Received UDP data: " + returnData);
            }
            catch (SocketException e)
            {
                // A SocketException will be thrown when the UdpClient is closed.
                // This is expected when stopping the receiver, so we can check our flag.
                if (isReceiving)
                {
                    Debug.LogError("SocketException: " + e.Message);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error in UDP reception: " + e.Message);
            }
        }
    }

    void OnApplicationQuit()
    {
        StopReceiving();
    }

    void OnDestroy()
    {
        StopReceiving();
    }

    private void StopReceiving()
    {
        if (isReceiving)
        {
            isReceiving = false;

            // Close the UdpClient. This will cause the blocking Receive() 
            // call to throw a SocketException and unblock the thread.
            if (udpClient != null)
            {
                udpClient.Close();
            }

            // Now, wait for the thread to actually finish.
            // This should be very fast since the thread is now unblocked.
            if (receiveThread != null && receiveThread.IsAlive)
            {
                receiveThread.Join();
            }

            // Dispose the client only after the thread has finished.
            // In the original code, `Dispose` was called before Join, which is not ideal.
            // However, Close() often calls Dispose() internally.
            if (udpClient != null)
            {
                udpClient.Dispose();
                udpClient = null;
            }

            Debug.Log("UDP Receiver stopped.");
        }
    }
}