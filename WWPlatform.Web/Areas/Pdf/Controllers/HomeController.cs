using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WWPlatform.Core.Utilities;
using WWPlatform.Model.CNTV;
using WWPlatform.Repositories.CNTV;
using WWPlatform.Web.Mvc.Extensions;

namespace WWPlatform.Web.Controllers.Pdf
{
    public class HomeController : BaseController
    {
        IWebcastRepository webcastRepository;
        IFeatureRepository featureRepository;

        public HomeController(IWebcastRepository webcastRepository, IFeatureRepository featureRepository)
        {
            this.webcastRepository = webcastRepository;
            this.featureRepository = featureRepository;
        }

        /// <summary>
        /// 生成pdf文件
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ActionResult Create(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return new EmptyResult();
            }

            string type = token.Substring(0, 1);
            int idkey;
            try
            {
                idkey = int.Parse(Base64.Decode(token.Substring(1)));
            }
            catch
            {
                return new EmptyResult();
            }

            PdfDocument doc;
            switch (type)
            {
                case "w":
                    Webcast w = webcastRepository.GetById(idkey);
                    if (w == null || w.Script == null)
                    {
                        return new EmptyResult();
                    }
                    doc = new WebcastDocument(w);
                    break;
                case "f":
                    Feature f = featureRepository.GetById(idkey);
                    if (f == null || f.Webcasts == null || f.Webcasts.All(t => t.Script == null))
                    {
                        return new EmptyResult();
                    }
                    doc = new FeatureDocument(f);
                    break;
                default:
                    return new EmptyResult();

            }

            return this.Pdf(doc.Title, doc.Stream);
        }
    }


    abstract class PdfDocument
    {
        private static BaseFont basefont = BaseFont.CreateFont(
                            Path.Combine(HttpRuntime.AppDomainAppPath, @"content\font\simhei.ttf"),
                            BaseFont.IDENTITY_H,
                            BaseFont.NOT_EMBEDDED);
        protected static Font font = new Font(basefont, 12f);
        protected static Regex regex = new Regex(@"<(?<tag>[^\s>]+)[^>]*>(?<cont>(.|\n)*?)</\k<tag>>");

        public string Title
        { get; protected set; }

        public Stream Stream
        {
            get { return this.Read(); }
        }

        /// <summary>
        /// 读取内容生成流
        /// </summary>
        /// <returns></returns>
        protected abstract Stream Read();

        protected class HeaderFooterPdfPageEventHelper : PdfPageEventHelper
        {
            PdfContentByte cb;
            PdfTemplate template;
            private const float fontsize = 8;

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                try
                {
                    cb = writer.DirectContent;
                    template = cb.CreateTemplate(50, 50);
                }
                catch (DocumentException de)
                { }
                catch (IOException io)
                { }
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
                string text = string.Format("第{0}页", writer.PageNumber);
                Rectangle rectangle = document.PageSize;
                cb.SetRGBColorFill(100, 100, 100);
                cb.BeginText();
                cb.SetFontAndSize(basefont, fontsize);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT
                    , "【文化中国】"
                    , rectangle.GetLeft(50)
                    , rectangle.GetTop(40)
                    , 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER
                    , "更多讲稿，请访问：http://www.culture-visions.com"
                    , rectangle.GetLeft(280)
                    , rectangle.GetBottom(40)
                    , 0);
                cb.SetTextMatrix(rectangle.GetRight(70), rectangle.GetBottom(40));
                cb.ShowText(text);
                cb.EndText();
            }

            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);
                //template.BeginText();
                //template.SetFontAndSize(basefont, fontsize);
                //template.SetTextMatrix(0, 0);
                //template.ShowText((writer.PageNumber - 1).ToString());
                //template.EndText();
            }

            public override void OnChapter(PdfWriter writer, Document document, float paragraphPosition, Paragraph title)
            {
                base.OnChapter(writer, document, paragraphPosition, title);
            }
        }
    }

    class WebcastDocument : PdfDocument
    {
        Webcast w;
        public WebcastDocument(Webcast w)
        {
            this.w = w;
            Title = string.Format("{0} {1}", w.Feature.Title, w.Title);
        }

        protected override Stream Read()
        {
            Document doc = new Document(PageSize.A4, 50, 50, 50, 50);
            doc.AddTitle(Title);
            doc.AddCreationDate();
            doc.AddCreator("iTextSharp");
            doc.AddAuthor("文化中国");
            doc.AddSubject(w.Excerpt);
            doc.AddKeywords(w.Tags);
            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                writer.SetEncryption(true, null, null, 0);
                writer.PageEvent = new HeaderFooterPdfPageEventHelper();

                doc.Open();

                Paragraph p = new Paragraph(Title, font);
                p.Alignment = 1;
                doc.Add(p);

                MatchCollection matches = regex.Matches(w.Script.Text);
                foreach (Match m in matches)
                {
                    Chunk chunk = new Chunk(m.Groups["cont"].Value);
                    switch (m.Groups["tag"].Value.ToLower())
                    {
                        case "h1":
                            font.Color = BaseColor.WHITE;
                            chunk.SetBackground(BaseColor.DARK_GRAY);
                            break;
                        case "p":
                            font.Color = BaseColor.BLACK;
                            break;
                    }
                    chunk.Font = font;
                    p = new Paragraph(chunk);
                    p.SetLeading(0.0f, 2.0f);
                    p.FirstLineIndent = 20f;
                    p.Alignment = 0;
                    doc.Add(p);
                }
                doc.Close();

                return new MemoryStream(stream.GetBuffer());
            }
        }
    }

    class FeatureDocument : PdfDocument
    {
        Feature f;
        public FeatureDocument(Feature f)
        {
            this.f = f;
            this.Title = f.Title;
        }

        protected override Stream Read()
        {
            Document doc = new Document(PageSize.A4, 50, 50, 50, 50);
            doc.AddTitle(Title);
            doc.AddCreationDate();
            doc.AddCreator("iTextSharp");
            doc.AddAuthor("文化中国");
            doc.AddSubject(f.Brief);
            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                writer.SetEncryption(true, null, null, PdfWriter.ALLOW_PRINTING);
                writer.ViewerPreferences = PdfWriter.PageModeUseOutlines;
                writer.PageEvent = new HeaderFooterPdfPageEventHelper();

                doc.Open();

                foreach (var w in f.Webcasts.Where(w => w.Script != null).OrderBy(w => w.Aired))
                {
                    Paragraph p = new Paragraph(w.Title, font);
                    p.Alignment = 1;
                    Chapter chapter = new Chapter(p, 1);
                    chapter.NumberDepth = 0;
                    chapter.BookmarkTitle = w.Title;
                    chapter.TriggerNewPage = true;
                    doc.Add(chapter);

                    MatchCollection matches = regex.Matches(w.Script.Text);
                    foreach (Match m in matches)
                    {
                        Chunk chunk = new Chunk(m.Groups["cont"].Value);
                        switch (m.Groups["tag"].Value.ToLower())
                        {
                            case "h1":
                                font.Color = BaseColor.WHITE;
                                chunk.SetBackground(BaseColor.DARK_GRAY);
                                break;
                            case "p":
                                font.Color = BaseColor.BLACK;
                                break;
                        }
                        chunk.Font = font;
                        p = new Paragraph(chunk);
                        p.SetLeading(0.0f, 2.0f);
                        p.FirstLineIndent = 20f;
                        p.Alignment = 0;
                        doc.Add(p);
                    }
                }

                doc.Close();
                return new MemoryStream(stream.GetBuffer());
            }
        }
    }
}