using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
namespace WEB_253505_AZAROV.UI.TagHelpers;
[HtmlTargetElement("pager")]
public class Pager : TagHelper
{
    private readonly LinkGenerator _linkGenerator;
    private readonly HttpContext _httpContext;
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string Category { get; set; } =  "";
    public bool Admin { get; set; } = false;
    public Pager(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContext = httpContextAccessor.HttpContext!;
    }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "nav";
        var ulTag = new TagBuilder("ul");
        ulTag.AddCssClass(Admin ? "pagination justify-content-center" : "pagination offset-sm-2 justify-content-center");
        var prevAvaibale = CurrentPage > 1;
        var nextAvaibale = CurrentPage < TotalPages;
        var prevLiTag = new TagBuilder("li");
        prevLiTag.AddCssClass(prevAvaibale ? "page-item" : "page-item disabled");
        var aPrevTag = new TagBuilder("a");
        aPrevTag.AddCssClass("page-link");
        aPrevTag.Attributes["aria-label"] = "Previous";
        if (prevAvaibale)
        {
            var prevUrl = GetPageUrl(CurrentPage - 1);
            aPrevTag.Attributes["href"] = prevUrl;
            aPrevTag.Attributes["data-ajax-url"] = prevUrl;
            aPrevTag.Attributes["data-ajax-method"] = "GET";
        }
        aPrevTag.InnerHtml.AppendHtml("&laquo;");
        prevLiTag.InnerHtml.AppendHtml(aPrevTag);
        ulTag.InnerHtml.AppendHtml(prevLiTag);
        for (int i = 1; i <= TotalPages; i++)
        {
            var liTag = new TagBuilder("li");
            liTag.AddCssClass("page-item");
            if (CurrentPage == i)
            {
                liTag.AddCssClass("active");
            }
            var pageUrl = GetPageUrl(i);
            var aTag = new TagBuilder("a");
            aTag.AddCssClass("page-link");
            aTag.Attributes["href"] = pageUrl;
            aTag.Attributes["data-ajax-url"] = pageUrl;
            aTag.Attributes["data-ajax-method"] = "GET";
            aTag.InnerHtml.AppendHtml(i.ToString());
            liTag.InnerHtml.AppendHtml(aTag);
            ulTag.InnerHtml.AppendHtml(liTag);
        }
        var aNextLiTag = new TagBuilder("li");
        aNextLiTag.AddCssClass(nextAvaibale ? "page-item" : "page-item disabled");
        var aNextTag = new TagBuilder("a");
        aNextTag.AddCssClass("page-link");
        aNextTag.Attributes["aria-label"] = "Next";
        if (nextAvaibale)
        {
            var nextUrl = GetPageUrl(CurrentPage + 1);
            aNextTag.Attributes["href"] = nextUrl;
            aNextTag.Attributes["data-ajax-url"] = nextUrl;
            aNextTag.Attributes["data-ajax-method"] = "GET";
        }
        aNextTag.InnerHtml.AppendHtml("&raquo;");
        aNextLiTag.InnerHtml.AppendHtml(aNextTag);
        ulTag.InnerHtml.AppendHtml(aNextLiTag);
        output.Content.AppendHtml(ulTag);
    }
    private string GetPageUrl(int pageNumber)
    {
        if (Admin)
        {
            var res = _linkGenerator!.GetPathByPage(_httpContext, page: "/Index",values: new 
              { 
                  area = "Admin", 
                  pageNo = pageNumber 
              });
            return res!;
        }
        else
        {
            var res = _linkGenerator.GetPathByAction(_httpContext,
                          action: "Index",
                          controller: "Product",
                          values: new 
                          { 
                              pageNo = pageNumber,
                              category = Category
                          });
            return res!;
        }
    }
}