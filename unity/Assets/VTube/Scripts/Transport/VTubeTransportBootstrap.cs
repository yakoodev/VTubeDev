using UnityEngine;

namespace VTube.Transport
{
    public static class VTubeTransportBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            var go = new GameObject("VTube.Transport");
            Object.DontDestroyOnLoad(go);
            go.AddComponent<MmfFrameWriter>();
            go.AddComponent<PipeCommandClient>();
        }
    }
}
