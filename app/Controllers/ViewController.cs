using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("view")]
    public class ViewController : ControllerBase
    {
        [HttpGet("{formatId}/{profile}")]
        public ContentResult Get(string formatId, string profile)
        {
            var html = $@"<!doctype html>
<html>
<head>
  <meta charset=""utf-8"" />
  <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
  <title>VTube Viewer</title>
  <style>
    html, body {{ margin: 0; padding: 0; background: #111; color: #ddd; font-family: sans-serif; }}
    .meta {{ position: absolute; top: 8px; left: 8px; font-size: 12px; opacity: 0.8; }}
    img {{ display: block; width: 100vw; height: 100vh; object-fit: contain; }}
  </style>
</head>
<body>
  <div class=""meta"">formatId={formatId} | profile={profile}</div>
  <img src=""/stream/{formatId}/{profile}.mjpg"" alt=""stream"" />
</body>
</html>";

            return new ContentResult
            {
                Content = html,
                ContentType = "text/html",
                StatusCode = 200
            };
        }
    }
}
