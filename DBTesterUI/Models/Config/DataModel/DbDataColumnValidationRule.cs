using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace DBTesterUI.Models.Config.DataModel
{
    class DbDataColumnValidationRule: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var column = ((BindingGroup)value)?.Items[0] as DbDataColumn;
            if (column == null || !column.IsValid())
            {
                return new ValidationResult(false, null);
            }

            return ValidationResult.ValidResult;

        }
    }
}
