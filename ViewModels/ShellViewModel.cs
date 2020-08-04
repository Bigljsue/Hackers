using Caliburn.Micro;

namespace WPF_HackersList.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        public ShellViewModel()
        {
            ActivateItem(new UCHackersListViewModel());
        }

        public void LoadHackersListPage()
        {
            ActivateItem(new UCHackersListViewModel());            
        }

        public void LoadSettingsPage()
        {
            ActivateItem(new UCSettingsViewModel());
        }
    }
}
