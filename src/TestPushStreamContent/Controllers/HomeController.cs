using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TestPushStreamContent.Infrastructure;

namespace TestPushStreamContent.Controllers
{
    public class HomeController : Controller
    {
        private Timer _timer;
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Simple example using the HttpContext.Response.Body stream in action method
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/eventstream")]
        public Task GetEventStream()
        {
            MemoryStream ms = new MemoryStream();
            var streamWriter = new StreamWriter(ms);

            _timer = new Timer(_ =>
            {
                streamWriter.WriteLine("data: Time - " + DateTime.UtcNow + " " + RandomString(2000) + "\n\n");
                streamWriter.Flush();
            }, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));

            HttpContext.Response.ContentType = "text/event-stream";
            return ms.CopyToAsync(HttpContext.Response.Body);
        }

        /// <summary>
        /// Example using the FileResultFromStream there wrap a PushStreamContent
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/eventstream_2")]
        public FileResultFromStream GetEventStream_usingThePushContentStream()
        {
            return new FileResultFromStream(new PushStreamContent(new Action<Stream, HttpContent, TransportContext>(WriteToStream), "text/event-stream"), "text/event-stream");
        }

        private void WriteToStream(Stream outputStream, HttpContent headers = null, TransportContext context = null)
        {
            var streamWriter = new StreamWriter(outputStream);
            streamWriter.AutoFlush = true;

            _timer = new Timer(_ =>
            {
                try
                {
                    streamWriter.WriteLine("data: (Using the FileResultFromStream) Time - " + DateTime.UtcNow + " " + RandomString(2000)+ "\n\n");
                    streamWriter.Flush();
                }
                catch
                {
                    _timer.Dispose();
                }
               
            }, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
