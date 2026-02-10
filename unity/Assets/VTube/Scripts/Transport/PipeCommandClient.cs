using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using UnityEngine;

namespace VTube.Transport
{
    public class PipeCommandClient : MonoBehaviour
    {
        private readonly ConcurrentQueue<string> _logQueue = new();
        private Thread _thread;
        private volatile bool _running;

        private void Start()
        {
            _running = true;
            _thread = new Thread(Run)
            {
                IsBackground = true,
                Name = "VTube-PipeCommandClient"
            };
            _thread.Start();
        }

        private void Update()
        {
            while (_logQueue.TryDequeue(out var message))
            {
                Debug.Log(message);
            }
        }

        private void Run()
        {
            while (_running)
            {
                try
                {
                    using var client = new NamedPipeClientStream(".", TransportConstants.CommandPipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
                    client.Connect(2000);
                    using var reader = new StreamReader(client, Encoding.UTF8, false, 4096, leaveOpen: true);
                    using var writer = new StreamWriter(client, Encoding.UTF8, 4096, leaveOpen: true)
                    {
                        AutoFlush = true
                    };

                    _logQueue.Enqueue("[Pipe] Connected to app command pipe");

                    while (_running && client.IsConnected)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        var request = JsonUtility.FromJson<SceneCommandRequest>(line);
                        var requestId = request?.GetRequestId() ?? "";
                        var commandType = request?.command?.type ?? "";
                        _logQueue.Enqueue($"[Pipe] SceneCommand requestId={requestId} type={commandType}");

                        var ack = new CommandAck
                        {
                            status = "accepted",
                            requestId = requestId,
                            error = null
                        };
                        var ackJson = JsonUtility.ToJson(ack);
                        writer.WriteLine(ackJson);
                    }
                }
                catch (Exception ex)
                {
                    _logQueue.Enqueue($"[Pipe] Connection error: {ex.Message}");
                    Thread.Sleep(1000);
                }
            }
        }

        private void OnDestroy()
        {
            _running = false;
            if (_thread != null && _thread.IsAlive)
            {
                _thread.Join(500);
            }
        }
    }

    [Serializable]
    public class SceneCommandRequest
    {
        public string requestId;
        public string source;
        public string timestampUtc;
        public RequestContext context;
        public SceneCommand command;

        public string GetRequestId()
        {
            if (!string.IsNullOrWhiteSpace(context?.requestId))
            {
                return context.requestId;
            }

            return requestId;
        }
    }

    [Serializable]
    public class RequestContext
    {
        public string requestId;
        public string source;
        public string timestampUtc;
    }

    [Serializable]
    public class SceneCommand
    {
        public string type;
    }

    [Serializable]
    public class CommandAck
    {
        public string status;
        public string requestId;
        public string error;
    }
}
