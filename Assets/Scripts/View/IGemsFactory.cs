using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View {
    public interface IGemsFactory {
        Gem Rent(Model.Gems gemType);
        void Release(Gem gem);
    }
}
