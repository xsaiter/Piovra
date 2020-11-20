using System.Xml.Linq;

namespace Piovra.EfCore.Extensions {
    public class XmlProperty : NotificationObject {
        public XmlProperty(string propertyName, XElement raw) {
            PropertyName = propertyName;
            Raw = raw;            
            Raw.Changed += OnChanged;
        }

        public XElement Raw { get; set; }
        public string PropertyName { get; }        

        void OnChanged(object sender, XObjectChangeEventArgs e) {
            RaiseProprtyChanged(PropertyName);
        }        
    }
}
