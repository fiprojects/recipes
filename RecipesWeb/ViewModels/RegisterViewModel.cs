using System.Collections.Generic;

namespace RecipesWeb.ViewModels
{
    public class RegisterViewModel
    {
        public List<SelectedItemViewModel> Ingredients { get; } = new List<SelectedItemViewModel>();
    }
}
