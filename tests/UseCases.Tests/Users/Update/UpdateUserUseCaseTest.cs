﻿using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Tests.Users.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();

        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var user = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains("Name cannot be empty."));
    }

    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains("E-mail is already in use."));
    }

    private UpdateUserUseCase CreateUseCase(User user, string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var loggedUser = LoggedUserBuilder.Build(user);
        var readRepository = new UserReadOnlyRepositoryBuilder();

        if (string.IsNullOrWhiteSpace(email) == false)
        {
            readRepository.ExistsActiveUserWithEmail(email);
        }

        return new UpdateUserUseCase(loggedUser, updateRepository, readRepository.Build(), unitOfWork);
    }
}
