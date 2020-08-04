using System;
using System.Collections.Generic;
using System.Text;
using WPF_HackersList.Models;

namespace WPF_HackersList.DataBaseClasses.DataBaseMethods
{
    public interface IDataBaseGetMethods
    {
        public List<PersonModel> GetPeopleList();

        public void GetDataFromOtherDataBase(string secondDataBaseFullPath);
    }
}
