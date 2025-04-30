using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Base.Validations;

namespace Xeovis.UI.Models
{
    public class ColumnModel
    {
        public string Header { get; set; }
        public List<object> Values { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsVisible { get; set; }
        public bool CanSort { get; set; }
        public bool CanReorder { get; set; }
        public bool CanResize { get; set; }
        public List<object> Options { get; set; }
        public bool HasOptions => Options != null && Options.Count > 0;
        public Guid Id { get; set; }
        public int DataCount => Values != null ? Values.Count : 0;
        public bool HasData => DataCount > 0;
        public Type ValueType => HasData ? Values[0].GetType() : typeof(string);
        public List<IValidationRule<string>> ValidationRules { get; set; }

        public ColumnModel()
        {
            Id = Guid.NewGuid();
            Values = new List<object>();
            Options = new List<object>();
            Header = "COLUMN";
            IsReadOnly = false;
            IsVisible = true;
            CanSort = true;
            CanReorder = true;
            CanResize = true;
            ValidationRules = new List<IValidationRule<string>>();
        }

        public ColumnModel(string header, List<object> values) : this()
        {
            Header = header;
            Values = new List<object>(values);
        }

        public ColumnModel(string header, List<string> values) : this()
        {
            Header = header;
            Values = new List<object>(values);
        }

        public ColumnModel(string header, List<double> values) : this()
        {
            Header = header;
            Values = new List<object>(values.Cast<object>());
        }

        public ColumnModel(string header, List<int> values) : this()
        {
            Header = header;
            Values = new List<object>(values.Cast<object>());
        }

        public ColumnModel(string header, List<bool> values) : this()
        {
            Header = header;
            Values = new List<object>(values.Cast<object>());
        }

        public ColumnModel(string header, List<DateTime> values) : this()
        {
            Header = header;
            Values = new List<object>(values.Cast<object>());
        }
        public override string ToString()
        {
            return Header;
        }
    }
}
