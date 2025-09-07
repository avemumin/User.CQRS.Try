using FluentValidation;
using User.Application.Commands;
using User.Application.Common.Interfaces;

namespace User.Application.Validators.UserValidators;

public class CreateUserValidator : AbstractValidator<CreateUser>
{
  public CreateUserValidator(IUserRepository repo)
  {
    RuleFor(x => x.Age)
      .GreaterThanOrEqualTo(5)
      .WithMessage("Musisz mieć co najmniej 5 lat.");

    RuleFor(x => x.Email)
      .NotEmpty()
      .EmailAddress();

    RuleFor(x => x.Email)
      .NotEmpty()
      .EmailAddress()
      .MustAsync(async (email, _) => !await repo.ExistsByEmailAsync(email))
      .WithMessage("Email już istnieje w bazie");
  }
}
