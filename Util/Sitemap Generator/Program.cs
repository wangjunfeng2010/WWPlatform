using System.Configuration;
using System.Data.SqlClient;
using System.Xml;
using WWPlatform.Core.Utilities;

namespace WWPlatform.SitemapGenerator
{
    class Program
    {
        static readonly string connectionstring = ConfigurationManager.ConnectionStrings["WWPlatform"].ConnectionString;

        static void Main(string[] args)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode xmlNode;
            xmlNode = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlNode);

            XmlNode root = xmlDoc.CreateElement("urlset");
            xmlDoc.AppendChild(root);

            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("select idkey from feature", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    XmlNode url = xmlDoc.CreateNode(XmlNodeType.Element, "url", null);
                    root.AppendChild(url);

                    xmlNode = xmlDoc.CreateNode(XmlNodeType.Element, "loc", null);
                    xmlNode.InnerText = string.Format("http://www.culture-visions.com/f/{0}.html", Base64.Encode(reader.GetInt32(0).ToString()));
                    url.AppendChild(xmlNode);

                    xmlNode = xmlDoc.CreateNode(XmlNodeType.Element, "lastmod", null);
                    xmlNode.InnerText = "2011-07-25";
                    url.AppendChild(xmlNode);

                    xmlNode = xmlDoc.CreateNode(XmlNodeType.Element, "priority", null);
                    xmlNode.InnerText = "0.5";
                    url.AppendChild(xmlNode);
                }

                reader.Close();

                cmd = new SqlCommand("select idkey,aired from webcast order by aired desc", conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    XmlNode url = xmlDoc.CreateNode(XmlNodeType.Element, "url", null);
                    root.AppendChild(url);

                    xmlNode = xmlDoc.CreateNode(XmlNodeType.Element, "loc", null);
                    xmlNode.InnerText = string.Format("http://www.culture-visions.com/w/{0}.html", Base64.Encode(reader.GetInt32(0).ToString()));
                    url.AppendChild(xmlNode);

                    xmlNode = xmlDoc.CreateNode(XmlNodeType.Element, "lastmod", null);
                    xmlNode.InnerText = reader.GetDateTime(1).ToString("yyyy-MM-dd");
                    url.AppendChild(xmlNode);

                    xmlNode = xmlDoc.CreateNode(XmlNodeType.Element, "priority", null);
                    xmlNode.InnerText = "0.8";
                    url.AppendChild(xmlNode);
                }
            }

            xmlDoc.Save(@"E:\sitemap.xml");
        }
    }
}
