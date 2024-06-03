using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PooledObjectPolicy : IPooledObjectPolicy<Socket>
{
    string _host;
    int _port;

    public PooledObjectPolicy(string host, int port)
    {
        _host = host;
        _port = port;
    }

    public Socket Create()
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(_host, _port);
        return socket;
    }

    public bool Return(Socket obj)
    {
        return true;
    }
}


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void Test_MemoryPool()
    {
        var buff = CMemoryPool<byte>.Shared.Rent(56);
        var bytes = buff.Memory.ToArray();
        buff.Dispose();
    }

    void Test_Extension()
    {
        IPooledObjectPolicy<Socket> policy = new PooledObjectPolicy("localhost", 39_999);
        
        // maximumRetained 값을 지정하지 않으면, 기본값은 Environment.ProcessorCount * 2
        DefaultObjectPool<Socket> pool = new DefaultObjectPool<Socket>(policy, 10);

        List<Socket> sockets = new List<Socket>();
        for (int i = 0; i < 11; i++)
        {
            sockets.Add(pool.Get());
        }

        //Console.WriteLine();
        sockets.ForEach((elem) => pool.Return(elem));
        sockets.Clear();

        for (int i = 0; i < 11; i++)
        {
            sockets.Add(pool.Get());
        }

        //Console.WriteLine("press any key to exit...");
        //Console.ReadLine();
    }

    void Test_Extension2()
    {
        IPooledObjectPolicy<Socket> policy = new PooledObjectPolicy("localhost", 39_999);

        var provider = new DefaultObjectPoolProvider();
        provider.MaximumRetained = 1;

        ObjectPool<Socket> pool = provider.Create(policy);
        {
            Socket socket1 = pool.Get();
            Socket socket2 = pool.Get();

            var b1 = socket1.Connected; //true
            var b2 = socket2.Connected; //true

            pool.Return(socket1);
            pool.Return(socket2);

            b1 = socket1.Connected; //true
            b2 = socket2.Connected; //false

            (pool as IDisposable).Dispose();

            b1 = socket1.Connected; //false
            b2 = socket2.Connected; //false
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onclick_GameStart()
    {
        Debug.Log("미니 게임 스타트");
    }
}
