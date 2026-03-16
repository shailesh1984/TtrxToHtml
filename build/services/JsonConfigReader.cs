public static class JsonConfigReader
{
    public static CakeConfig LoadConfig(string filePath)
    {
        var json = System.IO.File.ReadAllText(filePath);
        return Newtonsoft.Json.JsonConvert.DeserializeObject<CakeConfig>(json);
    }
}