using System;
using System.Xml.Linq;

namespace Piovra.EfCore.Extensions {
    public class XmlProperty : NotificationObject {
        public XmlProperty(string propertyName, XElement raw, Action<string> callback) {
            PropertyName = propertyName;
            Raw = raw;            
            Raw.Changed += OnChanged;
            Callback = callback;
        }

        public string PropertyName { get; }
        public XElement Raw { get; set; }        
        public Action<string> Callback { get; }

        void OnChanged(object sender, XObjectChangeEventArgs e) {
            RaisePropertyChanged(PropertyName);
            Callback?.Invoke(PropertyName);
        }

        public override string ToString() => Raw.ToString();        
    }
}
