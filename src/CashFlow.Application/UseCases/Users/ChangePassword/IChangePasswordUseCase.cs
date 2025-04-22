using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Users.ChangePassword;

public interface IChangePasswordUseCase
{
    public Task Execute(RequestChangePasswordJson request);
}
