using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using WPF_HackersList.Classes;
using WPF_HackersList.DataBaseClasses.DataBaseMethods;

namespace WPF_HackersList.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {

        public void LoadHackersListPage()
        {
            ActivateItem(new UCHackersListViewModel());            
        }
    }
}
