using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Infrastructure.Extentions
{
    public static class ControllerExtentions
    {
        public static IActionResult ViewOrNotFound(this Controller controller, object model)
        {
            if (model == null)
            {
                return controller.NotFound();
            }

            return controller.View(model);
        }
    }
}
