using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using WPF_HackersList.DataBaseClasses;
using WPF_HackersList.DataBaseClasses.DataBaseMethods;
using WPF_HackersList.Models;

namespace WPF_HackersList.ViewModels
{
    public class UCHackersListViewModel : Screen, IDeactivate
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
            set { _personName = value; NotifyOfPropertyChange(() => PersonName); TextBoxPersonNameChanged(); }
        }

        private object _selectedItem;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; NotifyOfPropertyChange(() => SelectedItem); }
        }

        private List<string> _filteredPeople;

        public List<string> FilteredPeople
        {
            get { return _filteredPeople; }
            set { _filteredPeople = value; NotifyOfPropertyChange(() => FilteredPeople); }
        }

        private int _filteredPeopleCount;

        public string FilteredPeopleCount
        {
            get { return $"Найдено: {_filteredPeopleCount}"; }
            set { _filteredPeopleCount = Convert.ToInt32(value); NotifyOfPropertyChange(() => FilteredPeopleCount); }
        }


        private bool _dropDownMenuOpened;

        public bool DropDownMenuOpened
        {
            get { return _dropDownMenuOpened; }
            set { _dropDownMenuOpened = value; NotifyOfPropertyChange(() => DropDownMenuOpened); }
        }

        public UCHackersListViewModel()
        {
            UpdateDataGrid();
        }

        public void UpdateDataGrid()
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
            UpdateDataGrid();
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

            UpdateDataGrid();
        }

        public void TextBoxPersonNameChanged()
        {
            if (People != null)
            {
                if (String.IsNullOrWhiteSpace(PersonName))
                {
                    FilteredPeople = null;
                    FilteredPeopleCount = null;
                }
                else
                {
                    FilteredPeople = People.Where(x => x.Name.Contains(PersonName, StringComparison.OrdinalIgnoreCase)).Select(x => x.Name).ToList();
                    FilteredPeopleCount = FilteredPeople.Count.ToString();
                }
            }       
        }
    }
}
