using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RosemountDiagnosticsV2.View_Models;

namespace RosemountDiagnosticsV2.ViewComponents
{
    public class CreateGuageViewComponent : ViewComponent
    {
        private readonly IRecipeLimitRepository _RecipeLimitRepository;

        public CreateGuageViewComponent(IRecipeLimitRepository recipeLimitRepository)
        {
            _RecipeLimitRepository = recipeLimitRepository;
        }
        public IViewComponentResult Invoke(RecipeTypes recipeType, string title, double value, LimitType limitType, int height, int width)
        {
            CreateGuageViewModel guageViewModel = new CreateGuageViewModel()
            {
                ActualValue = value,
                Title = title,
                RecipeLimit = _RecipeLimitRepository.GetLimitInfo(recipeType, limitType),
                Height = height,
                Width = width
            };

            return View(guageViewModel);
        }


    }
}
