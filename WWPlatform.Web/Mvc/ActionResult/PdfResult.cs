using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace WWPlatform.Web.Mvc
{
    public class PdfResult : ActionResult
    {
        private string fileName;
        private Stream stream;

        public PdfResult(string fileName, Stream stream)
        {
            this.fileName = fileName;
            this.stream = stream;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("controller context");

            HttpResponseBase response = context.HttpContext.Response;
            switch (context.HttpContext.Request.Browser.Browser)
            {
                //Firefox
                case "IE":
                    response.AddHeader("Content-Disposition", string.Format("attachment;filename=\"{0}.pdf\"", HttpUtility.UrlEncode(fileName)));
                    break;
                //IE and others
                case "Firefox":
                    response.AddHeader("Content-Disposition", string.Format("attachment;filename*=\"{0}.pdf\"", HttpUtility.UrlEncode(fileName)));
                    break;
            }
            response.ContentType = "application/octet-stream";
            byte[] buffer = new byte[stream.Length];
            while (true)
            {
                int read = this.stream.Read(buffer, 0, buffer.Length);
                if (read == 0)
                { break; }

                response.OutputStream.Write(buffer, 0, read);
            }
            response.OutputStream.Flush();
            response.OutputStream.Close();
            response.Flush();
            response.End();
        }
    }
}