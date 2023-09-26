using HandyCook.Application.Data;

namespace HandyCook.Application.Pages
{
    public partial class Index
    {
        public IEnumerable<Recipe> Recipes { get; set; } = new HashSet<Recipe>();

        protected override async Task OnInitializedAsync()
        {
            if (Recipes is null || Recipes.Count() is 0)
            {
                Recipes = ctx.Recipes;
            }

            await base.OnInitializedAsync();
        }
    }
}
