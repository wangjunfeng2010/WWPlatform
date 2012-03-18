using System.IO;
using System.Web.Mvc;

namespace WWPlatform.Web.Mvc.Extensions
{
    public static class ControllerExtensions
    {
        public static PdfResult Pdf(this Controller controller, string filename, Stream stream)
        {
            return new PdfResult(filename, stream);
        }

        public static PdfResult Pdf(this Controller controller, string filename, byte[] bytes)
        {
            return new PdfResult(filename, new MemoryStream(bytes));
        }
    }
}