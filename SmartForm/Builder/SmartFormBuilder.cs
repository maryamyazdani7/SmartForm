
using Microsoft.AspNetCore.Html;

namespace SmartForm
{
    public abstract class SmartFormBuilder<TModel>
    {
        public abstract HtmlString Build();
    }
}
