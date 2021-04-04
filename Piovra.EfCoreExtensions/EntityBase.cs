using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Piovra.EfCoreExtensions {
    public class EntityBase : NotificationObject {
        protected XmlProperty GetXmlProperty(XElement xml, [CallerMemberName] string propertyName = null) {
            return new XmlProperty(propertyName, xml, _ => RaisePropertyChanged(_));
        }

        protected void SetXmlProperty(ref XmlProperty field, XmlProperty value, [CallerMemberName] string propertyName = null) {
            SetProperty(ref field, value, propertyName);
        }

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
            if (!Equals(field, value)) {
                field = value;
                RaisePropertyChanged(propertyName);
            }
        }
    }
}
