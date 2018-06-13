using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DBTesterLib.Data;

namespace DBTesterUI.Models.Config.DataModel
{
    class DbDataColumn
    {
        public bool IsPrimary { get; set; }

        public bool IsNotPrimary => !IsPrimary;

        public Visibility DeleteVisibility => IsPrimary ? Visibility.Hidden : Visibility.Visible;

        public string PrimaryString => IsPrimary ? "[PK]" : "";

        public string Name { get; set; }

        public DataType Type { get; set; }

        public IEnumerable<DataType> DataTypes => Enum.GetValues(typeof(DataType)).Cast<DataType>();

        public bool IsValid()
        {
            return Name != null && Name.Trim().Length > 0;
        }
    }
}