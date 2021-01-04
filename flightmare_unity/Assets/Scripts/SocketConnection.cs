using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;


public class SocketConnection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //string[] hosts = new string[] { "172.17.0.1" };
        //string[] hosts = new string[] { "127.0.0.1" };
        string[] hosts = new string[] { "192.168.1.157" };
        getSocket(hosts);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static Socket ConnectSocket(string server, int port)
    {
        print("connecting socket at server: " + server + " and port " + port);

        Socket s = null;
        IPHostEntry hostEntry = null;

        // Get host related information.
        hostEntry = Dns.GetHostEntry(server);

        // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
        // an exception that occurs when the host IP Address is not compatible with the address family
        // (typical in the IPv6 case).
        foreach (IPAddress address in hostEntry.AddressList)
        {
            IPEndPoint ipe = new IPEndPoint(address, port);
            Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            print("connection starting with address " + address);

            tempSocket.Connect(ipe);

            print("connection attempt established");


            if (tempSocket.Connected)
            {
                s = tempSocket;
                print("connection succesful");
                break;
            }
            else
            {
                print("connection not succesful");
                continue;
            }
        }
        return s;
    }

    // This method requests the home page content for the specified server.
    private static string SocketSendReceive(string server, int port)
    {

        string request = "GET / HTTP/1.1\r\nHost: " + server +
            "\r\nConnection: Close\r\n\r\n";
        Byte[] bytesSent = Encoding.ASCII.GetBytes(request);
        Byte[] bytesReceived = new Byte[256];
        string page = "";

        print("sending request at " + request);


        // Create a socket connection with the specified server and port.
        using (Socket s = ConnectSocket(server, port))
        {

            if (s == null)
                return ("Connection failed");

            // Send request to the server.
            s.Send(bytesSent, bytesSent.Length, 0);

            // Receive the server home page content.
            int bytes = 0;
            page = "Default HTML page on " + server + ":\r\n";

            // The following will block until the page is transmitted.
            do
            {
                bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
                page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
            }
            while (bytes > 0);
        }

        return page;
    }

    public static void getSocket(string[] args)
    {
        string host;
        int port = 80;

        if (args.Length == 0)
            // If no server name is passed as argument to this program,
            // use the current host name as the default.
            host = Dns.GetHostName();
        else
            host = args[0];

        string result = SocketSendReceive(host, port);
        Console.WriteLine(result);
    }


}
