using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO.Compression;

namespace WWPlatform.Util
{
    class Program
    {
        static void Main(string[] args)
        {
            //int idkey = Math.Abs("http://kejiao.cntv.cn/bjjt/classpage/video/20110801/100997.shtml".GetHashCode());
            //Console.ReadLine();
            //return;
            //ISpider spider = new FeatureSpider();
            ////spider.Crawl();
            ////(spider as IFeature).Parse();
            ////Console.WriteLine("---------------------");

            //spider = new WebcastSpider();
            ////spider.Crawl();
            ////Console.WriteLine("---------------------");

            //(spider as IWebcast).ParseTag();
            ISpider spider = new BjjtSpider();
            spider.Crawl();
            Console.ReadLine();
        }
    }

    interface ISpider
    {
        void Crawl();
    }

    interface IWebcast
    {
        void Fixup();

        void ParseTag();
    }

    interface IFeature
    {
        void Parse();
    }

    /// <summary>
    /// 爬虫类
    /// </summary>
    abstract class Spider : ISpider
    {
        protected static readonly string connectionstring = ConfigurationManager.ConnectionStrings["WWPlatform"].ConnectionString;

        void ISpider.Crawl()
        {
            this.Crawl();
        }

        protected abstract void Crawl();
    }

    class FeatureSpider : Spider, IFeature
    {
        const string vod = "http://tansuo.cntv.cn/vod/bjjt/index.shtml";
        protected override void Crawl()
        {
            WebRequest request = WebRequest.Create(vod);
            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
            {
                string str = reader.ReadToEnd();
                Regex regex = new Regex("title_array\\(\"(?<title>.*)\",\"(?<image>.*)\",\"(?<refurl>.*)\",\"(?<brief>.*)\",\"(?<source>.*)\",\"(?<category>.*)\"\\)");
                MatchCollection matches = regex.Matches(str);
                foreach (Match m in regex.Matches(str))
                {
                    Console.WriteLine(m.Groups["title"]);

                    string refurl = m.Groups["refurl"].Value;
                    int idkey = Math.Abs(refurl.GetHashCode());
                    string sql = string.Format(@"if not exists(select * from feature where idkey={0})"
                        + " begin"
                        + " insert into feature(idkey,title,catalogid,lectuerid,image,refurl,category,brief,dynastyid) values({0},'{1}',{2},{3},'{4}','{5}','{6}','{7}',{8})"
                        + " end",
                        idkey,
                        m.Groups["title"].Value,
                        1,
                        1,
                        m.Groups["image"].Value,
                        refurl,
                        m.Groups["category"].Value,
                        m.Groups["brief"].Value,
                        1
                        );
                    using (SqlConnection conn = new SqlConnection(connectionstring))
                    {
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.CommandType = CommandType.Text;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        void IFeature.Parse()
        {
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter("select category from feature", conn);
                conn.Open();
                adapter.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string category = dr.Field<string>("category");
                    string[] array = category.Substring(category.IndexOf("/") + 1).Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var subtype in array)
                    {
                        SqlCommand cmd = new SqlCommand(string.Format("if not exists(select 1 from subtype where title='{0}') begin insert into subtype(title) values('{0}') end", subtype));
                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }

    class WebcastSpider : Spider, IWebcast
    {
        Semaphore fixupsemphore = new Semaphore(20, 20);
        protected override void Crawl()
        {
            DataSet dataset = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("select a.* from feature a left join webcast b on a.idkey=b.featureid where b.featureid is null", conn);
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataset);
            }

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                string url = row.Field<string>("refurl");
                int idref = row.Field<int>("idkey");

                try
                {
                    WebRequest request = WebRequest.Create(url);
                    request.Timeout = 60 * 1000;
                    WebResponse response = request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
                    {
                        string str = reader.ReadToEnd();

                        // 注意:正则表达式前面要加上new,不然会匹配到function video_c_array
                        Regex regex = new Regex(@"new video_c_array\('(?<title>.*)','(?<image>.*)','(?<timelong>.*)','(?<refurl>.*)','(?<excerpt>.*)','(?<source>.*)','(?<catagory>.*)'\)");
                        MatchCollection matches = regex.Matches(str);
                        foreach (Match m in matches)
                        {
                            string timelong = m.Groups["timelong"].Value;
                            string refurl = m.Groups["refurl"].Value;
                            string image = m.Groups["image"].Value;
                            int idkey;
                            DateTime createdon = DateTime.Today;

                            //通过分析抓取到的数据，发现refurl可能为空，要特殊处理
                            if (string.IsNullOrWhiteSpace(refurl))
                            {
                                idkey = Math.Abs(image.GetHashCode());
                                createdon = DateTime.Today;
                            }
                            else
                            {
                                idkey = Math.Abs(refurl.GetHashCode());
                                string[] ports = refurl.Split('/');
                                try
                                {
                                    createdon = DateTime.ParseExact(ports[ports.Length - 2], new string[] { "yyyyMMdd" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                                }
                                catch
                                {
                                    createdon = DateTime.Today;
                                }
                            }

                            using (SqlConnection conn = new SqlConnection(connectionstring))
                            {

                                string sql = string.Format(@"if not exists(select * from webcast where idkey={0})"
                                    + " begin"
                                    + " insert into webcast(idkey,visit,aired,title,[image],excerpt,featureid,tags,refurl,coreid,timelong)"
                                    + " values({0},{1},'{2}','{3}','{4}','{5}',{6},'{7}','{8}','{9}','{10}')"
                                    + " end",
                                    idkey,
                                    new Random(unchecked((int)DateTime.Now.Ticks)).Next(1, 100000),
                                    createdon,
                                    m.Groups["title"].Value,
                                    image,
                                    m.Groups["excerpt"].Value,
                                    idref,
                                    "",
                                    refurl,
                                    "",
                                    timelong.Substring(timelong.Length - 5)
                                    );

                                SqlCommand cmd = new SqlCommand(sql, conn);
                                conn.Open();
                                cmd.ExecuteNonQuery();
                            }

                            Thread.Sleep(100);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //记录下发生错误
                    Console.WriteLine(url);
                    Console.WriteLine(ex.Message);
                }
            }
        }

        void IWebcast.Fixup()
        {
            DataSet dataset = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("select idkey,refurl,[title] from webcast where coreid='' and refurl !=''", conn);
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataset);
            }

            if (dataset.Tables.Count == 0 || dataset.Tables[0].Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                fixupsemphore.WaitOne();
                new Thread(delegate(object obj)
                {
                    int idkey = (int)obj.GetType().GetProperty("idkey").GetValue(obj, null);
                    string refurl = (string)obj.GetType().GetProperty("refurl").GetValue(obj, null);
                    string title = (string)obj.GetType().GetProperty("title").GetValue(obj, null);
                    try
                    {

                        string tags = string.Empty;
                        WebRequest request = WebRequest.Create(refurl);
                        request.Timeout = 60 * 1000;
                        string coreid = string.Empty;
                        string videoid = string.Empty;

                        using (StreamReader reader2 = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.Default))
                        {
                            string text = reader2.ReadToEnd();
                            //tag
                            Regex regex2 = new Regex("var video_tag = \"(?<tags>.*)\";");
                            Match match2 = regex2.Match(text);
                            tags = match2.Groups["tags"].Value.Replace(" ", ",");

                            //coreid
                            regex2 = new Regex("var multiVariate = \"(?<variate>.*)\"");
                            match2 = regex2.Match(text);
                            string[] array = match2.Groups["variate"].Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            coreid = array[1];
                            videoid = array[2];
                        }
                        using (SqlConnection conn = new SqlConnection(connectionstring))
                        {
                            SqlCommand cmd = new SqlCommand(string.Format("update webcast set tags='{0}',coreid='{1}',videoid='{2}' where idkey={3}", tags, coreid, videoid, idkey), conn);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("idkey:{0} title:{1} url:{2}-->error:{3}", idkey, title, refurl, ex.Message);
                    }
                    finally
                    {
                        Console.WriteLine(title);
                        fixupsemphore.Release();
                    }
                }).Start(new
                {
                    refurl = row.Field<string>("refurl"),
                    idkey = row.Field<int>("idkey"),
                    title = row.Field<string>("title")
                });
            }
        }

        void IWebcast.ParseTag()
        {
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter("select tags from webcast", conn);
                conn.Open();
                adapter.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string tags = dr.Field<string>("tags");
                    if (string.IsNullOrWhiteSpace(tags))
                    {
                        continue;
                    }
                    string[] array = tags.Substring(tags.IndexOf("/") + 1).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tag in array)
                    {
                        if (tag.Length > 10)
                        {
                            continue;
                        }
                        SqlCommand cmd = new SqlCommand(string.Format("if not exists(select 1 from tag where title='{0}') begin insert into tag(title) values('{0}') end", tag));
                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }

    class FictionSpider : Spider
    {
        void CrawlBook()
        {
            //string url = "http://bookapp.book.qq.com/cgi-bin/lz_index?p={0}&type=sort&key=15";
            Regex regex = new Regex("<a[^>]*?href=([\"\"'\\s]?)(?<refurl>[^\"\"'\\s>]+)[^>]*?><img[^>]*?src=([\"\"'\\s]?)(?<cover2>[^\"\"'\\s>]+)[^>]*?></a>");
            for (int p = 1; p <= 30; p++)
            {
                string url = string.Format("http://bookapp.book.qq.com/cgi-bin/lz_index?p={0}&type=sort&key=15", p);
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 60 * 1000;
                WebResponse response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {
                    string str = reader.ReadToEnd();
                    MatchCollection matches = regex.Matches(str);
                    foreach (Match m in matches)
                    {
                        string refurl = m.Groups["refurl"].Value;
                        if (!refurl.StartsWith("http://book.qq.com/s/book"))
                        {
                            continue;
                        }
                        string cover2 = m.Groups["cover2"].Value;
                        int idkey = Math.Abs(refurl.GetHashCode());
                        string sql = string.Format("insert into fiction.book(idkey,cover2,refurl) values({0},'{1}','{2}')", idkey, cover2, refurl);
                        using (SqlConnection conn = new SqlConnection(connectionstring))
                        {
                            SqlCommand cmd = new SqlCommand(sql, conn);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        void CrawlSection()
        {
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("select * from fiction.book where title is null", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                conn.Open();
                adapter.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string url = dr.Field<string>("refurl");
                    WebRequest request = WebRequest.Create(url);
                    request.Timeout = 60 * 1000;
                    WebResponse response = request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
                    {
                        string str = reader.ReadToEnd();
                        Regex regex = new Regex(@"(?is)(?<=<div\s*id=""bookBrief""\s*>)\s*<h1[^>]*?>((?:(?!</?h1).)*)</h1>.*?<p[^>]*?>((?:(?!</?p).)*).*?splitAuthor\(['""]([^'""]+)['""]\)");
                        Match match = regex.Match(str);
                        string booktitle = match.Groups[1].Value;
                        string brief = match.Groups[2].Value;
                        string author = match.Groups[3].Value;

                        regex = new Regex(@"<div\b(?:(?!id=).)*id=(['""]?)bookCover[^>]*><img[^>]*?src=([""'\s]?)(?<cover3>[^""'\s>]+)[^>]*?></div>");
                        match = regex.Match(str);
                        string cover3 = match.Groups["cover3"].Value;

                        cmd.CommandText = string.Format("update fiction.book set title=N'{0}',cover3='{1}',brief=N'{2}',author=N'{3}' where idkey={4}",
                            booktitle,
                            cover3,
                            brief,
                            author,
                            dr.Field<int>("idkey")
                            );
                        cmd.ExecuteNonQuery();

                        regex = new Regex(@"(?is)(?<=<div\s*class=""tit"">)(?:(?!<div\s*class=""tit"").)*");
                        foreach (Match m in regex.Matches(str))
                        {
                            Regex r = new Regex(@"(?is)<span>(.*?)</span>(.*?)</div>(.*?)<p>(.*?)</p>");
                            Match t = r.Match(m.Value);
                            if (string.IsNullOrWhiteSpace(t.Groups[1].Value))
                            {
                                continue;
                            }

                            short seq = short.Parse(t.Groups[1].Value);
                            string title = t.Groups[2].Value;
                            brief = t.Groups[4].Value;
                            int sectionid = Math.Abs((booktitle + title).GetHashCode());
                            cmd.CommandText = string.Format("insert into fiction.section(idkey,title,brief,seq,bookid) values({0},N'{1}',N'{2}',{3},{4})",
                                sectionid,
                                title.Replace("'", "''"),
                                brief.Replace("'", "''"),
                                seq,
                                dr.Field<int>("idkey")
                                );
                            cmd.ExecuteNonQuery();

                            r = new Regex(@"(?is)<a[^>]*?href=""javascript:opennew\(['""]([^'""]+)['""]\)""\s*>第(?<seq>\d+)节：(?<tit>.*?)</a>");
                            foreach (Match t1 in r.Matches(m.Value))
                            {
                                seq = short.Parse(t1.Groups["seq"].Value);
                                title = t1.Groups["tit"].Value;
                                url = t1.Groups[1].Value;
                                string text = null;
                                WebRequest request1 = WebRequest.Create(url);
                                request.Timeout = 60 * 1000;
                                WebResponse response1 = request1.GetResponse();
                                Regex regex1 = new Regex(@"(?is)<div\b(?:(?!id=).)*id=(['""]?)content\1[^>]*>((?><div[^>]*>(?<o>)|</div>(?<-o>)|(?:(?!</?div\b).)*)*(?(o)(?!)))</div>");
                                using (StreamReader reader1 = new StreamReader(response1.GetResponseStream(), Encoding.Default))
                                {
                                    string str1 = reader1.ReadToEnd();
                                    Match m1 = regex1.Match(str1);
                                    if (m1.Groups != null && m1.Groups.Count >= 3)
                                    {
                                        text = m1.Groups[2].Value;
                                    }
                                    else
                                    {
                                        text = string.Empty;
                                    }
                                }
                                cmd.CommandText = string.Format("insert into fiction.chapter(idkey,bookid,title,sectionid,[text],seq,refurl) values({0},{1},N'{2}',{3},N'{4}',{5},'{6}')",
                                    Math.Abs((dr.Field<string>("refurl") + url).GetHashCode()),
                                    dr.Field<int>("idkey"),
                                    title.Replace("'", "''"),
                                    sectionid,
                                    text.Replace("'", "''"),
                                    seq,
                                    url);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        protected override void Crawl()
        {
            //CrawlBook();
            CrawlSection();
            return;
        }
    }

    class BjjtSpider : Spider
    {
        protected override void Crawl()
        {
            DateTime compiled = new DateTime(2011, 6, 10);
            for (int p = 361; p <= 361; p++)
            {
                string url = string.Format("http://www.baijiajiangtan.org/biji/dasuifengyun/{0}.html", p);
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 60 * 1000;
                WebResponse response = request.GetResponse();
                Regex regex = new Regex(@"<p.*?>(?<text>.*)</p>");
                string text = string.Empty;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {
                    string str = reader.ReadToEnd();
                    MatchCollection matches = regex.Matches(str);
                    foreach (Match m in matches)
                    {
                        if (m.Groups["text"].Value.StartsWith("（"))
                        {
                            text = text + m.Groups["text"].Value.Replace("（", "<h1>").Replace("）", "</h1>");
                        }
                        else
                        {
                            text = text
                                + "<p>"
                                + m.Groups["text"].Value
                                + "</p>";
                        }
                    }
                }
                compiled = compiled.AddDays(1);
                int idkey = Math.Abs(url.GetHashCode());
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = string.Format("insert into script(idkey,[text],compiled,visit) values({0},N'{1}','{2}',{3})", idkey, text.Replace("<p></p><h1>编辑：huyong</h1>",""), compiled, 353);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
