using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Toolkit;

namespace View {
    public sealed class GemsFactory : MonoBehaviour, IGemsFactory
    {
        [SerializeField]
        Gem Prefab = null;

        GemsPool pool = null;

        Gem IGemsFactory.Rent(Model.Gems gemType) {
            var res = pool.Rent();
            res.Initialize(gemType, (Gem gem) => {
                gem.transform.SetParent(this.transform);
                pool.Return(gem);
            });
            return res;
        }

        // Start is called before the first frame update
        void Start()
        {
            this.pool = new GemsPool(Prefab);
        }

        private class GemsPool : ObjectPool<Gem> {
            private Gem prefab = null;
            public GemsPool(Gem prefab) {
                this.prefab = prefab;
            }
            protected override Gem CreateInstance()
            {
                var res = GameObject.Instantiate(prefab).GetComponent<Gem>();
                return res;
            }
        }
    }
}
