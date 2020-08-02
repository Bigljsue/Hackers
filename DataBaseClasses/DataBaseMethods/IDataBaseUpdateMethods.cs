using System;
using System.Collections.Generic;
using System.Text;
using WPF_HackersList.Models;

namespace WPF_HackersList.DataBaseClasses.DataBaseMethods
{
    public interface IDataBaseUpdateMethods
    {
        public void UpdateChanges(List<PersonModel> people);
    }
}
