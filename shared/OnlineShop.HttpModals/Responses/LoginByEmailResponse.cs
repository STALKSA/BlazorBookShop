namespace OnlineShop.HttpModals.Responses
{
    public record LoginByEmailResponse(Guid Id, string Name, Guid? ConfirmationCodeId);
    public record LoginByCodeResponse(Guid Id, string Name, string Token);

}
