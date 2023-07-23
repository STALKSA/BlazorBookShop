using Microsoft.AspNetCore.Components;
using MudBlazor;




//namespace BlazorBookShop.Pages
//{
//    public partial class RegistrationPage
//    {
//        private RegisterRequest _model = new RegisterRequest();
//        private bool _registrationInProgress = false;

//        private async Task ProcessRegistration()
//        {
//            if (_registrationInProgress)
//            {
//                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
//                Snackbar.Add("Пожалуйста, подождите...", Severity.Info);
//                return;
//            }
//            _registrationInProgress = true;
//            try
//            {
//                await Client.Register(_model);
//                await DialogService.ShowMessageBox(
//                    "Успех!",
//                    $"Вы успешно зарегистрировались! Молодец!",
//                    yesText: "Ok!");
//                NavigationManager.NavigateTo("/catalog");
//            }
//            catch (MySuperShopApiException ex)
//            {
//                _registrationInProgress = false;
//                await DialogService.ShowMessageBox(
//                    "Ошибка!",
//                    $"Что-то пошло не так во время регистрации, а именно:\n{ex.Message}");
//            }
//            finally
//            {
//                _registrationInProgress = false;
//            }
//        }
//    }
//}
