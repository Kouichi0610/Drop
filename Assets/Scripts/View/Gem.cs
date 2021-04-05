using UnityEngine;
using System.Collections;

namespace View {
    public class Gem : MonoBehaviour
    {
        public delegate void OnErase(Gem gem);

        public Model.Gems GemType { get; private set; }

        [SerializeField]
        private Material[] materials;

        OnErase erase = null;

        public void Initialize(Model.Gems gemType, OnErase erase) {
            this.erase = erase;
            GemType = gemType;
            var r = GetComponent<MeshRenderer>();
            r.material = materials[GemType.Id];
            gameObject.SetActive(true);
        }

        public IEnumerator Erase(System.Action onComplete) {
            yield return new WaitForEndOfFrame();
            erase(this);
            onComplete();
        }

        public IEnumerator DropTo(int y, System.Action onComplete) {
            while (transform.position.y > y) {
                yield return new WaitForEndOfFrame();
                var next = transform.position;
                next.y -= 0.1f;
                transform.position = next;
            }
            {
                var next = transform.position;
                next.y = y;
                transform.position = next;
            }
            onComplete();
        }


        // Start is called before the first frame update
        void Start()
        {
        }
    }
}

