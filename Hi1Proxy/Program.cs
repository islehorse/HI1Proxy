using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Hi1Proxy
{
    
    class Program
    {
        static Socket hi1Mitm = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static MapForm mpForm;
        static bool isWaiting = true;
        static void AcceptConnection(ConsoleColor recv, ConsoleColor resp, bool gameServer)
        {
            Socket hi1MitmClient = hi1Mitm.Accept();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Client: " + hi1MitmClient.RemoteEndPoint);
          
            Socket realHi1Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.ForegroundColor = ConsoleColor.White;
            realHi1Server.Connect(IPAddress.Parse("216.98.148.63"), 443);
            Console.WriteLine("Server: "+realHi1Server.RemoteEndPoint);

            Thread ClientToServer = new Thread(() =>
            {

                MemoryStream ms = new MemoryStream();

                while (true)
                {
                    try
                    {
                        if (gameServer)
                        {
                            HI1Protocal.Hi1Client = hi1MitmClient;
                            HI1Protocal.HI1Server = realHi1Server;
                        }
                        

                        while (hi1MitmClient.Available >= 1)
                        {
                            byte[] buffer = new byte[hi1MitmClient.Available];
                            hi1MitmClient.Receive(buffer);


                            foreach (Byte b in buffer)
                            {
                                ms.WriteByte(b);
                                if (b == 0x00)
                                {
                                    ms.Seek(0x00, SeekOrigin.Begin);
                                    byte[] sendTo = ms.ToArray();
                                    if (HI1Protocal.ParseClientRequestPacket(sendTo))
                                    {
                                        realHi1Server.Send(sendTo);
                                        Console.ForegroundColor = recv;
                                    }
                                    ms.Close();
                                    ms = new MemoryStream();
                                }
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                        hi1MitmClient.Close();
                        realHi1Server.Close();
                        break;
                    }
                }
            });



            Thread ServerToClient = new Thread(() =>
            {

                MemoryStream ms = new MemoryStream();

                while (true)
                {

                    try
                    {
                        if (gameServer)
                        {
                            HI1Protocal.Hi1Client = hi1MitmClient;
                            HI1Protocal.HI1Server = realHi1Server;
                        }

                        if (realHi1Server.Available >= 1)
                        {
                            byte[] buffer = new byte[realHi1Server.Available];
                            realHi1Server.Receive(buffer);

                            foreach(Byte b in buffer)
                            {
                                ms.WriteByte(b);
                                if (b == 0x00)
                                {
                                    ms.Seek(0x00, SeekOrigin.Begin);
                                    byte[] respondWith = ms.ToArray();

                                    if (HI1Protocal.ParseHi1ResponsePackets(respondWith))
                                    {
                                        hi1MitmClient.Send(respondWith);
                                    }
                                    Console.ForegroundColor = recv;
                                    ms.Close();
                                    ms = new MemoryStream();
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                        hi1MitmClient.Close();
                        realHi1Server.Close();
                        break;
                    }

                }
            });

            ClientToServer.Start();
            ServerToClient.Start();
            if (gameServer == false)
            {
                isWaiting = false;
            }
            while (ClientToServer.IsAlive == true) { };
        }
        static void Main(string[] args)
        {


            if(File.Exists("MapData.bmp"))
            {
                Bitmap nMapData = new Bitmap("MapData.bmp");
                for(int x = 0; x < nMapData.Width; x++)
                {
                    for(int y = 0; y < nMapData.Height; y++)
                    {
                        HI1Protocal.MapData.SetPixel(x, y, nMapData.GetPixel(x, y));
                    }
                }
                nMapData.Dispose();
            }
            if (File.Exists("oMapData.bmp"))
            {
                Bitmap nMapData = new Bitmap("oMapData.bmp");
                for (int x = 0; x < nMapData.Width; x++)
                {
                    for (int y = 0; y < nMapData.Height; y++)
                    {
                        HI1Protocal.oMapData.SetPixel(x, y, nMapData.GetPixel(x, y));
                    }
                }
                nMapData.Dispose();
            }
            /*
            HI1Protocal.RebuildMapPngFromDb();*/
            

            int PORT = 443;
            string IP = "0.0.0.0";
            
            hi1Mitm = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress hostIP = IPAddress.Parse(IP);
            IPEndPoint ep = new IPEndPoint(hostIP, PORT);
            hi1Mitm.Bind(ep);
            hi1Mitm.Listen(10000);
            /*
            new Thread(() =>
            {
                try
                {
                    mpForm = new MapForm();
                     mpForm.ShowDialog();
                }
                catch(Exception)
                { 
                    
                }
            }).Start();*/

            Thread AcceptConnection1 = new Thread(() =>
            {
                AcceptConnection(ConsoleColor.Yellow, ConsoleColor.Magenta, false);
            });
            AcceptConnection1.Start();

            Thread AcceptConnection2 = new Thread(() =>
            {
                Console.WriteLine("Waiting for 1st connection.");
                while (isWaiting) { }
                Console.WriteLine("Accepting connections!");
                AcceptConnection(ConsoleColor.Cyan, ConsoleColor.Green, true);
            });
            AcceptConnection2.Start();
        }
    }
}
