using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using WPF_HackersList.Classes;
using WPF_HackersList.DataBaseClasses;
using WPF_HackersList.DataBaseClasses.DataBaseMethods;
using WPF_HackersList.Models;

namespace WPF_HackersList.ViewModels
{
    public class UCHackersListViewModel : Screen
    {

        private BindableCollection<PersonModel> _people { get; set; }

        public BindableCollection<PersonModel> People
        {
            get { return _people; }
            set { _people = value; NotifyOfPropertyChange(() => People); }
        }

        private string _personName { get; set; }

        public string PersonName
        {
            get { return _personName; }
            set { _personName = value; NotifyOfPropertyChange(() => PersonName); }
        }

        private object _selectedItem;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; NotifyOfPropertyChange(() => SelectedItem); }
        }

        public void UpdatePeople()
        {
            IDataBaseGetMethods DataBaseGetMethods = DependencyResolver.Resolve<IDataBaseGetMethods>();
            IDataBaseUpdateMethods DataBaseUpdateMethods = DependencyResolver.Resolve<IDataBaseUpdateMethods>();

            if(People != null && SelectedItem != null)
                DataBaseUpdateMethods.UpdateChanges(People.ToList());

            People = new BindableCollection<PersonModel>(DataBaseGetMethods.GetPeopleList());
        }

        public void AddPerson()
        {
            if (String.IsNullOrWhiteSpace(PersonName))
            {
                MessageBox.Show("Поле с ником не заполенно корректно");
                return;
            }

            IDataBaseAddMethods DataBaseAddMethods = DependencyResolver.Resolve<IDataBaseAddMethods>();

            DataBaseAddMethods.AddPerson(PersonName);
            UpdatePeople();
        }

        public void RemovePerson()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Игрок не вибран в списке");
                return;
            }

            IDataBaseDeleteMethods DataBaseDeleteMethods = DependencyResolver.Resolve<IDataBaseDeleteMethods>();

            DataBaseDeleteMethods.DeletePerson(((PersonModel)SelectedItem).Id);
            SelectedItem = null;

            UpdatePeople();
        }
    }
}
