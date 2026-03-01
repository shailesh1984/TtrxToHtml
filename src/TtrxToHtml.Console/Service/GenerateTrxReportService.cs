using Microsoft.Extensions.Logging;

namespace TtrxToHtml.Console.Service
{
    public class GenerateTrxReportService : IGenerateTrxReportService
    {
        private static readonly string CONTENTS_FOLDER = "Contents";
        private readonly RazorLightEngine _razorEngine;
        private readonly ILogger<GenerateTrxReportService> _logger;

        public GenerateTrxReportService(RazorLightEngine razorEngine, ILogger<GenerateTrxReportService> logger)
        {
            _razorEngine = razorEngine ?? throw new ArgumentNullException(nameof(razorEngine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Convert trx to html
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="commandLineInterfaceValues"></param>
        /// <returns></returns>
        public async Task<TrxReport> ConvertTrxToHtmlAsync(AppSettings appSettings, CommandLineInterfaceValues commandLineInterfaceValues)
        {
            if (appSettings == null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }

            if (commandLineInterfaceValues == null)
            {
                throw new ArgumentNullException(nameof(commandLineInterfaceValues));
            }

            string trxDirPath = commandLineInterfaceValues.TrxPath!;
            string fileExt = Path.GetExtension(trxDirPath);

            List<string> trxFiles = new List<string>();
            if (string.IsNullOrEmpty(fileExt))
            {
                trxFiles = PathHelper.GetAllFilesBySpecifiedDirPath(trxDirPath);
            }
            else
            {
                trxFiles.Add(trxDirPath);
                trxDirPath = Path.GetDirectoryName(trxDirPath) ?? trxDirPath;
            }

            if (trxFiles.Count == 0)
            {
                _logger.LogWarning("No trx files found in path: {Path}", trxDirPath);
                throw new FileNotFoundException("No trx files found.", trxDirPath);
            }

            _logger.LogInformation("Processing {Count} trx file(s) into html format.", trxFiles.Count);

            string json;
            try
            {
                json = TrxHelper.CombineAllTrxFilesToOneTrx(trxFiles);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to combine trx files.");
                throw;
            }

            JsonData? testResult = JsonConvert.DeserializeObject<JsonData>(json);
            if (testResult == null)
            {
                _logger.LogError("Failed to deserialize combined trx json into JsonData.");
                throw new InvalidOperationException("Deserialization returned null.");
            }

            string html;
            try
            {
                html = await _razorEngine.CompileRenderAsync(appSettings.CshtmlFileName, testResult);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Razor template rendering failed for template {Template}.", appSettings.CshtmlFileName);
                throw;
            }

            TrxReport trxReport = new TrxReport
            {
                Html = html,
                Path = trxDirPath
            };

            return trxReport;
        }

        /// <summary>
        /// Generate trx report in html
        /// </summary>
        /// <param name="trxReport"></param>
        /// <param name="appSettings"></param>
        /// <param name="commandLineInterfaceValues"></param>
        /// <returns></returns>
        public async Task<string> CreateHtmlAsync(TrxReport trxReport, AppSettings appSettings, CommandLineInterfaceValues commandLineInterfaceValues)
        {
            if (trxReport == null)
            {
                throw new ArgumentNullException(nameof(trxReport));
            }

            if (appSettings == null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }

            if (commandLineInterfaceValues == null)
            {
                throw new ArgumentNullException(nameof(commandLineInterfaceValues));
            }

            string dateTime = DateTime.Now.ToString(appSettings.DateTimeFormat);
            string outputPath = string.IsNullOrEmpty(commandLineInterfaceValues.OutPutFilePath) ? trxReport.Path! : commandLineInterfaceValues.OutPutFilePath;
            string directoryPath = Path.Combine(outputPath, appSettings.HtmlReportDirectoryFolder!);

            DirectoryHelper.CreateDirectory(directoryPath);

            string rawReportFileName = string.IsNullOrEmpty(commandLineInterfaceValues.HtmlFileName) ? appSettings.TestReportFileName : string.Concat(commandLineInterfaceValues.HtmlFileName, "_");

            // sanitize file name to remove invalid chars
            string sanitizedFileName;
            {
                char[] invalidChars = Path.GetInvalidFileNameChars();
                string temp = string.Concat(rawReportFileName, dateTime);
                sanitizedFileName = new string(temp.Where(c => Array.IndexOf(invalidChars, c) < 0).ToArray());
            }

            string testReportFile = Path.Combine(directoryPath, sanitizedFileName + appSettings.OutputFileExt);

            string sourceContentsPath = Path.Combine(AppContext.BaseDirectory, CONTENTS_FOLDER);
            DirectoryInfo sourceContentsDir = new DirectoryInfo(sourceContentsPath);
            if (sourceContentsDir.Exists)
            {
                try
                {
                    TrxHelper.DeepCopy(sourceContentsDir, directoryPath, CONTENTS_FOLDER);
                }
                catch (Exception exception)
                {
                    _logger.LogWarning(exception, "Failed to copy contents folder. Continuing; CSS/JS may be missing in report.");
                }
            }
            else
            {
                _logger.LogWarning("Contents folder not found at {ContentsPath}. Skipping copy.", sourceContentsPath);
            }

            await File.WriteAllTextAsync(testReportFile, trxReport.Html);

            _logger.LogInformation("Test report written to {Path}", testReportFile);

            return testReportFile;
        }
    }
}
