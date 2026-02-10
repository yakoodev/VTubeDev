namespace VTube.Transport
{
    public static class TransportConstants
    {
        public const string CommandPipeName = "vtube_cmd_v1";
        public const string FramesMmfName = "vtube_frames_v1";
        public const string FramesSignalName = "vtube_frames_signal_v1";
        public const int HeaderSizeBytes = 2048;
        public const int MaxFrameBytes = 1920 * 1080 * 4;
        public const int DefaultWidth = 640;
        public const int DefaultHeight = 360;
        public const int DefaultTargetFps = 10;
    }
}
