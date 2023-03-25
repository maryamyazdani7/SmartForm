
using Microsoft.AspNetCore.Html;

namespace SmartForm
{
    public class FormBuilder<TModel> : SmartFormBuilder<TModel>
    {
        public List<Component> Inputs { get; set; }
        private string _Name { get; set; } = "smart-forms-" + (new Random()).Next(10000, 99999);

        public FormBuilder()
        {
            Inputs = new List<Component>();
        }

        public FormBuilder<TModel> AddFormItem(List<Component> inputs)
        {
            Inputs.AddRange(inputs);

            return this;
        }
        public FormBuilder<TModel> Name(string name)
        {
            _Name = name;
            return this;
        }

        public override HtmlString Build()
        {
            var components = "";
            foreach (var widget in Inputs.Select((x, i) => new { Value = x, Index = i }))
            {
                components += widget.Value.Build();
            }
            var html = $@"<div class=""smart-form-container"" Id=""{_Name}"">{components}</div>";
            return new HtmlString(html);
        }
    }

}
