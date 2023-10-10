using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Events;
using OnlineShop.Domain.Events.Handlers;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;

namespace BookShop.Domain.Test
{
    public class AccountTests
    {
        [Fact]
        public async void Account_registered_event_notifies_user_by_email()
        {
            // Arrange
            var account = new Account(Guid.NewGuid(), "John", "John@john.com", "qwerty");

            var loggerMock = new Mock<ILogger<RegistrationNotification>>();

            var emailSenderMock = new Mock<IEmailSender>();


            // var emailSender = new FakeEmailSender();
            var logger = new Logger<RegistrationNotification>(new LoggerFactory());

            var handler = new RegistrationNotification(emailSenderMock.Object, loggerMock.Object);
            var @event = new AccountRegistered(account, DateTime.Now);

            // Act
            await handler.Handle(@event, default);

            // Assert
            // 1. Активация обработчика (триггер)
            handler.Should().BeAssignableTo<INotificationHandler<AccountRegistered>>();

            // 2. Факт вызова метода отправки сообщения
            emailSenderMock
                .Verify(it =>
                    it.SendEmailAsync(account.Email, It.IsAny<string>(), It.IsAny<string>(), default), Times.Once);



            // emailSender.counter.Should().Be(1);
        }

        [Fact]
        public async Task Register_Should_ThrowException_WhenNameIsNull()
        {
            // Arrange 
            var bogus = new Faker();
            var email = bogus.Internet.Email();
            var name = (string)null; // Имя равно null 
            var password = bogus.Internet.Password();

            var accountService = new AccountService(
                Mock.Of<IApplicationPasswordHasher>(),
                Mock.Of<IUnitOfWork>(),
                Mock.Of<ILogger<AccountService>>());

            // Act & Assert 
            await FluentActions.Invoking(async () => await accountService.Register(name, email, password, CancellationToken.None))
                .Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Register_Should_ThrowException_WhenEmailIsNull()
        {
            // Arrange 
            var bogus = new Faker();
            var email = (string)null; // Email равен null 
            var name = bogus.Person.FullName;
            var password = bogus.Internet.Password();

            var accountService = new AccountService(
                Mock.Of<IApplicationPasswordHasher>(),
                Mock.Of<IUnitOfWork>(),
                Mock.Of<ILogger<AccountService>>());

            // Act & Assert 
            await FluentActions.Invoking(async () => await accountService.Register(name, email, password, CancellationToken.None))
                .Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Register_Should_ThrowException_WhenPasswordIsNull()
        {
            // Arrange 
            var bogus = new Faker();
            var email = bogus.Internet.Email();
            var name = bogus.Person.FullName;
            var password = (string)null; // Пароль равен null 

            var accountService = new AccountService(
                Mock.Of<IApplicationPasswordHasher>(),
                Mock.Of<IUnitOfWork>(),
                Mock.Of<ILogger<AccountService>>());

            // Act & Assert 
            await FluentActions.Invoking(async () => await accountService.Register(name, email, password, CancellationToken.None))
                .Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Register_Should_RegisterNewAccount_WhenEmailIsUnique()
        {
            // Arrange 
            var bogus = new Faker();
            var email = bogus.Internet.Email();
            var name = bogus.Person.FullName;
            var password = bogus.Internet.Password();

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.AccountRepository.FindAccountByEmail(email, CancellationToken.None))
                .ReturnsAsync((Account)null);

            var accountService = new AccountService(
                Mock.Of<IApplicationPasswordHasher>(),
                uowMock.Object,
                Mock.Of<ILogger<AccountService>>());

            // Act 
            await accountService.Register(name, email, password, CancellationToken.None);

            // Assert 
            uowMock.Verify(u => u.AccountRepository.Add(It.IsAny<Account>(), CancellationToken.None));
            uowMock.Verify(u => u.CartRepository.Add(It.IsAny<Cart>(), CancellationToken.None));
            uowMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));
        }

        [Fact]
        public async Task Register_Should_ThrowException_WhenEmailIsNotUnique()
        {
            // Arrange 
            var bogus = new Faker();
            var email = bogus.Internet.Email();
            var name = bogus.Person.FullName;
            var password = bogus.Internet.Password();

            var existingAccount = new Account(Guid.NewGuid(), "John", "John@john.com", "qwerty"); ; // Существующий аккаунт 

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.AccountRepository.FindAccountByEmail(email, CancellationToken.None))
                .ReturnsAsync(existingAccount);

            var accountService = new AccountService(
                Mock.Of<IApplicationPasswordHasher>(),
                uowMock.Object,
                Mock.Of<ILogger<AccountService>>());

            // Act & Assert 
            await FluentActions.Invoking(async () => await accountService.Register(name, email, password, CancellationToken.None))
                .Should().ThrowAsync<EmailAlreadyExistsException>();
        }

    }
}
