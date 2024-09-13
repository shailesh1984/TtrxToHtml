using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;
using TtrxToHtml.Console;

public class Program
{
    private static void Main(string[] args)
    {
        var sourceDirectory = @"C:\Personal\Work\TtrxToHtml\src\TtrxToHtml.Console";
        string json = TrxHelper.CombineAllTrxFilesToOneTrx(sourceDirectory);
    }
}

// To convert JSON text contained in string json into an XML node
//XmlDocument doc = JsonConvert.DeserializeXmlNode(json);


