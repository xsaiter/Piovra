using System;

namespace Piovra.Data {
    public interface IEntity<TIdentity> where TIdentity : IEquatable<TIdentity> {
        TIdentity Id { get; set; }
    }
}
