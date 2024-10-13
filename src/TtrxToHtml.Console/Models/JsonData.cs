namespace TtrxToHtml.Console.Models;

public partial class JsonData
{
    [JsonProperty("TestResults")]
    public required TestResults TestResults { get; set; }
}

public partial class TestResults
{
    [JsonProperty("TestRun")]
    [JsonConverter(typeof(SingleOrArrayConverter<TestRun>))]
    public required List<TestRun> TestRun { get; set; }
}

public partial class TestRun
{
    [JsonProperty("@id")]
    public Guid Id { get; set; }

    [JsonProperty("@name")]
    public string Name { get; set; }

    [JsonProperty("@runUser")]
    public string RunUser { get; set; }

    [JsonProperty("@xmlns")]
    public Uri Xmlns { get; set; }

    [JsonProperty("TestSettings")]
    public TestSettings TestSettings { get; set; }

    [JsonProperty("Times")]
    public Times Times { get; set; }

    [JsonProperty("ResultSummary")]
    public ResultSummary ResultSummary { get; set; }

    [JsonProperty("TestDefinitions")]
    public TestDefinitions TestDefinitions { get; set; }

    [JsonProperty("TestLists")]
    public TestLists TestLists { get; set; }

    [JsonProperty("TestEntries")]
    public TestEntries TestEntries { get; set; }

    [JsonProperty("Results")]
    public Results Results { get; set; }
}

public partial class ResultSummary
{
    [JsonProperty("@outcome")]
    public Outcome Outcome { get; set; }

    [JsonProperty("Counters")]
    public Counters Counters { get; set; }

    [JsonProperty("Output")]
    public ResultSummaryOutput Output { get; set; }
}

public partial class Counters
{
    [JsonProperty("@total")]
    public long Total { get; set; }

    [JsonProperty("@executed")]
    public long Executed { get; set; }

    [JsonProperty("@passed")]
    public long Passed { get; set; }

    [JsonProperty("@error")]
    public long Error { get; set; }

    [JsonProperty("@failed")]
    public long Failed { get; set; }

    [JsonProperty("@timeout")]
    public long Timeout { get; set; }

    [JsonProperty("@aborted")]
    public long Aborted { get; set; }

    [JsonProperty("@inconclusive")]
    public long Inconclusive { get; set; }

    [JsonProperty("@passedButRunAborted")]
    public long PassedButRunAborted { get; set; }

    [JsonProperty("@notRunnable")]
    public long NotRunnable { get; set; }

    [JsonProperty("@notExecuted")]
    public long NotExecuted { get; set; }

    [JsonProperty("@disconnected")]
    public long Disconnected { get; set; }

    [JsonProperty("@warning")]
    public long Warning { get; set; }

    [JsonProperty("@completed")]
    public long Completed { get; set; }

    [JsonProperty("@inProgress")]
    public long InProgress { get; set; }

    [JsonProperty("@pending")]
    public long Pending { get; set; }
}

public partial class ResultSummaryOutput
{
    [JsonProperty("StdOut")]
    public string StdOut { get; set; }
}

public partial class Results
{
    [JsonProperty("UnitTestResult")]
    public required UnitTestResult[] UnitTestResult { get; set; }
}

public partial class UnitTestResult
{
    [JsonProperty("@executionId")]
    public Guid ExecutionId { get; set; }

    [JsonProperty("@testId")]
    public Guid TestId { get; set; }

    [JsonProperty("@testName")]
    public string TestName { get; set; }

    [JsonProperty("@computerName")]
    public string ComputerName { get; set; }

    [JsonProperty("@duration")]
    public DateTimeOffset Duration { get; set; }

    [JsonProperty("@startTime")]
    public DateTimeOffset StartTime { get; set; }

    [JsonProperty("@endTime")]
    public DateTimeOffset EndTime { get; set; }

    [JsonProperty("@testType")]
    public Guid TestType { get; set; }

    [JsonProperty("@outcome")]
    public Outcome Outcome { get; set; }

    [JsonProperty("@testListId")]
    public Guid TestListId { get; set; }

    [JsonProperty("@relativeResultsDirectory")]
    public Guid RelativeResultsDirectory { get; set; }

    [JsonProperty(nameof(Output), NullValueHandling = NullValueHandling.Ignore)]
    public UnitTestResultOutput Output { get; set; }
}

public partial class UnitTestResultOutput
{
    [JsonProperty("ErrorInfo", NullValueHandling = NullValueHandling.Ignore)]
    public ErrorInfo ErrorInfo { get; set; }

    [JsonProperty("StdOut", NullValueHandling = NullValueHandling.Ignore)]
    public string StdOut { get; set; }
}

public partial class ErrorInfo
{
    [JsonProperty("Message")]
    public string Message { get; set; }

    [JsonProperty("StackTrace")]
    public string StackTrace { get; set; }
}

public partial class TestDefinitions
{
    [JsonProperty("UnitTest")]
    public UnitTest[] UnitTest { get; set; }
}

public partial class UnitTest
{
    [JsonProperty("@name")]
    public string Name { get; set; }

    [JsonProperty("@storage")]
    public string Storage { get; set; }

    [JsonProperty("@id")]
    public Guid Id { get; set; }

    [JsonProperty("Execution")]
    public UnitTestExecution Execution { get; set; }

    [JsonProperty("TestMethod")]
    public TestMethod TestMethod { get; set; }
}

public partial class UnitTestExecution
{
    [JsonProperty("@id")]
    public Guid Id { get; set; }
}

public partial class TestMethod
{
    [JsonProperty("@codeBase")]
    public string CodeBase { get; set; }

    [JsonProperty("@adapterTypeName")]
    public string AdapterTypeName { get; set; }

    [JsonProperty("@className")]
    public string ClassName { get; set; }

    [JsonProperty("@name")]
    public string Name { get; set; }
}

public partial class TestEntries
{
    [JsonProperty("TestEntry")]
    public TestEntry[] TestEntry { get; set; }
}

public partial class TestEntry
{
    [JsonProperty("@testId")]
    public Guid TestId { get; set; }

    [JsonProperty("@executionId")]
    public Guid ExecutionId { get; set; }

    [JsonProperty("@testListId")]
    public Guid TestListId { get; set; }
}

public partial class TestLists
{
    [JsonProperty("TestList")]
    public TestList[] TestList { get; set; }
}

public partial class TestList
{
    [JsonProperty("@name")]
    public string Name { get; set; }

    [JsonProperty("@id")]
    public Guid Id { get; set; }
}

public partial class TestSettings
{
    [JsonProperty("@name")]
    public string Name { get; set; }

    [JsonProperty("@id")]
    public Guid Id { get; set; }

    [JsonProperty("Description")]
    public string Description { get; set; }

    [JsonProperty("Deployment")]
    public Deployment Deployment { get; set; }

    [JsonProperty("Execution")]
    public TestSettingsExecution Execution { get; set; }
}

public partial class Deployment
{
    [JsonProperty("@enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("@runDeploymentRoot")]
    public string RunDeploymentRoot { get; set; }

    [JsonProperty("DeploymentItem")]
    public DeploymentItem[] DeploymentItem { get; set; }
}

public partial class DeploymentItem
{
    [JsonProperty("@filename")]
    public string Filename { get; set; }
}

public partial class TestSettingsExecution
{
    [JsonProperty("TestTypeSpecific")]
    public object TestTypeSpecific { get; set; }

    [JsonProperty("AgentRule")]
    public AgentRule AgentRule { get; set; }
}

public partial class AgentRule
{
    [JsonProperty("@name")]
    public string Name { get; set; }
}

public partial class Times
{
    [JsonProperty("@creation")]
    public DateTimeOffset Creation { get; set; }

    [JsonProperty("@queuing")]
    public DateTimeOffset Queuing { get; set; }

    [JsonProperty("@start")]
    public DateTimeOffset Start { get; set; }

    [JsonProperty("@finish")]
    public DateTimeOffset Finish { get; set; }
}

public enum Outcome { Failed, Passed, Warn, NotExecuted, Executed, Error, Timeout, Aborted, Inconclusive, PassedButRunAborted, NotRunnable, Disconnected, Completed, InProgress, Pending };

public class SingleOrArrayConverter<T> : JsonConverter
{
    private readonly IContractResolver resolver;

    public SingleOrArrayConverter() : this(JsonSerializer.CreateDefault().ContractResolver) { }

    public SingleOrArrayConverter(IContractResolver resolver)
    {
        ArgumentNullException.ThrowIfNull(resolver);
        this.resolver = resolver;
    }

    public override bool CanConvert(Type objecType)
    {
        return (objecType == typeof(List<T>));
    }

    public override object ReadJson(JsonReader reader, Type objecType, object existingValue,
        JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        if (token.Type == JTokenType.Array)
        {
            return token.ToObject<List<T>>();
        }
        return new List<T> { token.ToObject<T>() };
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

public static partial class JsonExtensions
{
    public static JsonReader MoveToContentAndAssert(this JsonReader reader)
    {
        if (reader == null)
            throw new ArgumentNullException();
        if (reader.TokenType == JsonToken.None)       // Skip past beginning of stream.
            reader.ReadAndAssert();
        while (reader.TokenType == JsonToken.Comment) // Skip past comments.
            reader.ReadAndAssert();
        return reader;
    }

    public static JsonReader ReadAndAssert(this JsonReader reader)
    {
        if (reader == null)
            throw new ArgumentNullException();
        if (!reader.Read())
            throw new JsonReaderException("Unexpected end of JSON stream.");
        return reader;
    }
}