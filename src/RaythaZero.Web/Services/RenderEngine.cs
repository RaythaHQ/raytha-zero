using Fluid;
using Fluid.Filters;
using Fluid.Values;
using MediatR;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Utils;
using System.Text.Json;

namespace RaythaZero.Web.Services;

public class RenderEngine : IRenderEngine
{
    private static readonly FluidParser _parser = new FluidParser(new FluidParserOptions { AllowFunctions = true });
    private readonly IRelativeUrlBuilder _relativeUrlBuilder;
    private readonly ICurrentOrganization _currentOrganization;
    private readonly IMediator _mediator;
    private readonly IFileStorageProvider _fileStorageProvider;

    public RenderEngine(
        IMediator mediator, 
        IRelativeUrlBuilder relativeUrlBuilder, 
        ICurrentOrganization currentOrganization, 
        IFileStorageProvider fileStorageProvider)
    {
        _relativeUrlBuilder = relativeUrlBuilder;
        _currentOrganization = currentOrganization;
        _mediator = mediator;
        _fileStorageProvider = fileStorageProvider;
    }

    public string RenderAsHtml(string source, object entity)
    {
        if (_parser.TryParse(source, out var template, out var error))
        {
            var options = new TemplateOptions();
            options.MemberAccessStrategy = new UnsafeMemberAccessStrategy();
            options.TimeZone = DateTimeExtensions.GetTimeZoneInfo(_currentOrganization.TimeZone);
            options.Filters.AddFilter("attachment_redirect_url", AttachmentRedirectUrl);
            options.Filters.AddFilter("attachment_public_url", AttachmentPublicUrl);
            options.Filters.AddFilter("organization_time", LocalDateFilter);
            options.Filters.AddFilter("json", JsonFilter);

            var context = new TemplateContext(entity, options);
            string renderedHtml = template.Render(context);
            return renderedHtml;
        }
        else
        {
            throw new Exception(error);
        }
    }

    public ValueTask<FluidValue> AttachmentRedirectUrl(FluidValue input, FilterArguments arguments, TemplateContext context)
    {
        return new StringValue(_relativeUrlBuilder.MediaRedirectToFileUrl(input.ToStringValue()));
    }

    public ValueTask<FluidValue> AttachmentPublicUrl(FluidValue input, FilterArguments arguments, TemplateContext context)
    {
        if (string.IsNullOrEmpty(input.ToStringValue()))
            return new StringValue(string.Empty);

        return new StringValue(_fileStorageProvider.GetDownloadUrlAsync(input.ToStringValue(), FileStorageUtility.GetDefaultExpiry()).Result);
    }

    public ValueTask<FluidValue> LocalDateFilter(FluidValue input, FilterArguments arguments, TemplateContext context)
    {
        var value = TimeZoneConverter(input, context);
        return ReferenceEquals(value, NilValue.Instance) ? value : MiscFilters.Date(value, arguments, context);
    }
    
    public static ValueTask<FluidValue> JsonFilter(FluidValue input, FilterArguments arguments, TemplateContext context)
    {
        return new StringValue(JsonSerializer.Serialize(input.ToObjectValue()));
    }

    private FluidValue TimeZoneConverter(FluidValue input, TemplateContext context)
    {
        if (!input.TryGetDateTimeInput(context, out var value))
        {
            return NilValue.Instance;
        }

        var utc = DateTime.SpecifyKind(value.DateTime, DateTimeKind.Utc);

        // Create new offset for UTC
        var localOffset = new DateTimeOffset(utc, TimeSpan.Zero);

        var result = TimeZoneInfo.ConvertTime(localOffset, context.TimeZone);
        return new DateTimeValue(result);
    }
}
