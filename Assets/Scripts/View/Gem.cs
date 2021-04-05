using UnityEngine;
using System.Collections;

namespace View {
    public class Gem : MonoBehaviour
    {
        public Model.Gems GemType { get; private set; }

        [SerializeField]
        private Material[] materials;

        public void Initialize(Model.Gems gemType) {
            GemType = gemType;
            var r = GetComponent<MeshRenderer>();
            r.material = materials[GemType.Id];
            gameObject.SetActive(true);
        }

        public IEnumerator Erase(System.Action onComplete) {
            yield return new WaitForSeconds(1);
            gameObject.SetActive(false);
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

