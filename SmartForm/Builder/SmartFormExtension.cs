using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartForm
{
    public static class SmartFormExtension
    {
        public static SmartFormHost<TModel> SmartForm<TModel>(this IHtmlHelper<TModel> helper)
        {
            return new SmartFormHost<TModel>(helper);
        }
    }
}
