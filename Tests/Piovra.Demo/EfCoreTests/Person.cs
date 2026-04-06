using Piovra.EfCoreExtensions;

namespace Piovra.Demo.EfCoreTests;

public class Person : EntityBase {
    public int Id { get; set; }

    int _age;
    public int Age {
        get => _age;
        set => SetProperty(ref _age, value);
    }

    XmlProperty? _info;
    public XmlProperty? Info {
        get => _info is null ? null : GetXmlProperty(_info.Raw);
        set => SetXmlProperty(ref _info, value);
    }
}
