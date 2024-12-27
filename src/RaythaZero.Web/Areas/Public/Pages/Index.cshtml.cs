namespace RaythaZero.Web.Areas.Public.Pages;

public class IndexModel : BasePublicPageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}