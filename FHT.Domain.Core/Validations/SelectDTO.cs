namespace FHT.Domain.Core.Validations
{
    public class SelectDTO
    {
        public SelectDTO()
        {
        }

        public SelectDTO(string label, string value)
            : this()
        {
            Label = label;
            Value = value;
        }

        public SelectDTO(string label, string value, bool selected)
            : this(label, value)
        {
            Selected = selected;
        }

        public SelectDTO(string label, string value, bool selected, bool disabled)
            : this(label, value, selected)
        {
            Disabled = disabled;
        }

        public bool Disabled
        {
            get;
            set;
        }

        public bool Selected
        {
            get;
            set;
        }

        public string Label
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }
    }
}
