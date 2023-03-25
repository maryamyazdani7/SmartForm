using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartForm.Form
{
    public class FormResultData
    {
        public string Id { get; set; }
        public List<ComponentData> ComponentsData { get; set; }
    }
    public class ComponentData
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public bool Required { get; set; }
    }
}
