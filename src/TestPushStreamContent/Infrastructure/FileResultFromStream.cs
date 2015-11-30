using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace TestPushStreamContent.Infrastructure
{
    public class FileResultFromStream : ActionResult
    {
        public FileResultFromStream(PushStreamContent stream, string contentType)
        {
            Stream = stream;
            ContentType = contentType;
        }

        public string ContentType { get; private set; }
        public PushStreamContent Stream { get; private set; }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = ContentType;
            await Stream.CopyToAsync(response.Body);
        }
    }
}