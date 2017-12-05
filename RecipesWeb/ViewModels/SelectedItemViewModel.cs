using RecipesCore.Models;

namespace RecipesWeb.ViewModels
{
    public class SelectedItemViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool Selected { get; set; }
    }
}