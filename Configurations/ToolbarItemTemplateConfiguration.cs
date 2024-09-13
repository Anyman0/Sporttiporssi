using Sporttiporssi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Configurations
{
    public class ToolbarItemTemplateConfiguration : DataTemplateSelector
    {
        public DataTemplate MainPageTemplate { get; set; }
        public DataTemplate GroupPageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is MainPage)
            {
                return MainPageTemplate;
            }
            else if (item is GroupPage)
            {
                return GroupPageTemplate;
            }
            else return MainPageTemplate;
        }
    }
}
