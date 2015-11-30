# stackoverflow-PushStreamContent
Sample project for testing PushStreamContent.

    - Views/Home/Index.cshtml
      Contains simple html page where JS will print data sent from server
      
    - wwwroot/site.js
      EventSource logic: change here point to the other api endpoint
      
    - Controllers/HomeController.cs
      Contains 2 api endpoints, one for simple use of HttpContext.Response.Body stream and one using the FileResultFromStream there wrap a PushStreamContent

## Problem
`api/eventstream` it close the stream after it return the api action method and not possible to write to it later.

`api/eventstream_2` will not flush data to the stream on `Flush()`. Buffer it and sent it when buffer is full.