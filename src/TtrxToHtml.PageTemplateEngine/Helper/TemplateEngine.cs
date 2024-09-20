using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.Language;
using System.IO;
using System.Reflection;

namespace TtrxToHtml.PageTemplateEngine.Helper;

public sealed class TemplateEngine
{
    private const string TemplateFolderName = "Templates";
    private readonly Dictionary<string, RazorCompiledItem> _razorCompiledItems = new();
    private readonly HtmlResourceFileProvider _htmlResourceFileProvider = new();

    public TemplateEngine()
    {
        //var thisAssembly = Assembly.GetExecutingAssembly();
        //var viewAssembly = RelatedAssemblyAttribute.GetRelatedAssemblies(thisAssembly, false).Single();
        //var razorCompiledItems = new RazorCompiledItemLoader().LoadItems(viewAssembly);

        //foreach (var item in razorCompiledItems)
        //{
        //    _razorCompiledItems.Add(item.Identifier, item);
        //}

        //var localPath = Server.MapPath("~/Views/Demos/SomePartialView.cshtml");

        string filePath = Path.GetFullPath(@"Templates\JsonData.cshtml");

        //var ll = Path.Combine(PathHelpers.CurrentOutPutDirectory, @"Templates\JsonData.cshtml");

    }


    public async Task<string> RenderTemplateAsync<TModel>(TModel model)
    {
        var templateNamePrefix = model.GetType().Name;
        EnsureAllTemplatesExist(TemplateFolderName, templateNamePrefix);

        using var stringWriter = new StringWriter();
        await _htmlResourceFileProvider.LoadResource(TemplateFolderName, templateNamePrefix, ResourceType.Header, stringWriter);
        await stringWriter.WriteAsync(await RenderTemplateAsync(TemplateFolderName, templateNamePrefix, model));
        await _htmlResourceFileProvider.LoadResource(TemplateFolderName, templateNamePrefix, ResourceType.Footer, stringWriter);

        stringWriter.Flush();
        return stringWriter.ToString();
    }

    private void EnsureAllTemplatesExist(string templateFolderName, string templateNamePrefix)
    {
        var razorTemplate = GetRazorTemplateName(templateFolderName, templateNamePrefix);

        var errorMessages = new StringBuilder();
        if (!_razorCompiledItems.TryGetValue(razorTemplate, out var razorCompiledItem))
        {
            errorMessages.AppendLine($"The Razor Template file: {razorTemplate}, was not found.");
        }

        errorMessages.AppendLine(_htmlResourceFileProvider.ValidateDefaultResources(templateFolderName));

        if (errorMessages.Length > 2)
        {
            throw new RazorTemplateNotFoundException(errorMessages.ToString());
        }
    }

    private async Task<string> RenderTemplateAsync<TModel>(string templateFolderName, string templateName, TModel model)
    {
        var razorTemplate = GetRazorTemplateName(templateFolderName, templateName);
        var razorCompiledItem = _razorCompiledItems[razorTemplate];
        return await GetRenderedOutput(razorCompiledItem, model);
    }

    private static string GetRazorTemplateName(string templateFolderName, string templateName)
    {
        return $"/{templateFolderName}/{templateName}.cshtml";
    }

    private static async Task<string> GetRenderedOutput<TModel>(RazorCompiledItem razorCompiledItem, TModel model)
    {
        using var stringWriter = new StringWriter();
        var razorPage = GetRazorPageInstance(razorCompiledItem, model, stringWriter);
        await razorPage.ExecuteAsync();
        return stringWriter.ToString();
    }

    private static RazorPage GetRazorPageInstance<TModel>(RazorCompiledItem razorCompiledItem, TModel model, TextWriter textWriter)
    {
        var razorPage = (RazorPage<TModel>)Activator.CreateInstance(razorCompiledItem.Type);

        razorPage.ViewData = new ViewDataDictionary<TModel>(
            new EmptyModelMetadataProvider(),
            new ModelStateDictionary())
        {
            Model = model
        };

        razorPage.ViewContext = new ViewContext
        {
            Writer = textWriter
        };

        razorPage.HtmlEncoder = HtmlEncoder.Default;
        return razorPage;
    }
}
