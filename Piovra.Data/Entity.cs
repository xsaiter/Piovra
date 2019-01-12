using System;
using System.Collections.Generic;
using System.Text;

namespace Piovra.Data {
    public interface IEntity<TIdentity> where TIdentity : IEquatable<TIdentity> {
        TIdentity Id { get; set; }
    }
}
