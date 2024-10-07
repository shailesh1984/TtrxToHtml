﻿namespace TtrxToHtml.Console;

public static class TrxHelper
{
    public static string CombineAllTrxFilesToOneTrx(string sourceDirectory)
    {
        var trxFiles = Directory.EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories)
               .Where(s => s.EndsWith(".trx"))
               .ToList();

        XDocument doc = new();
        XElement rootElement = new("TestResults");

        foreach (var trxFile in trxFiles)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(trxFile);
            var xElem = XElement.Parse(xmlDoc.LastChild.OuterXml);

            rootElement.Add(new XElement(xElem));
        }
        doc.Add(rootElement);

        XmlDocument singleTrxXdoc = new();
        singleTrxXdoc.Load(doc.CreateReader());

        foreach (XmlNode node in singleTrxXdoc)
        {
            if (node.NodeType == XmlNodeType.XmlDeclaration)
            {
                singleTrxXdoc.RemoveChild(node);
            }
        }

        string json = JsonConvert.SerializeXmlNode(singleTrxXdoc, Newtonsoft.Json.Formatting.Indented);

        //File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "ConvertedTrxToJson.json"), json);

        return json;
    }
}
