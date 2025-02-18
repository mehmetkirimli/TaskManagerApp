using FluentValidation;
using TaskManagerApp.Entity;

namespace TaskManagerApp.Validator
{
    public class TaskValidator : AbstractValidator<TaskData>
    {
        public TaskValidator() 
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        }
    }
}
