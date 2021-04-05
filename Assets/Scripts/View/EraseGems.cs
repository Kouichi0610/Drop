using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Model;

namespace View {
    /// <summary>
    /// ジェムの消去
    /// </summary>
    public class EraseGems : MonoBehaviour
    {
        public IObservable<Unit> OnErased { get { return subject.AsObservable(); } }
        public Subject<Unit> subject = new Subject<Unit>();

        IEnumerator coroutine = null;

        public void Initialize(Transform gemsRoot, IFieldState field) {
            UnityEngine.Debug.Log("Erase.");
            coroutine = main(gemsRoot, field);
            StartCoroutine(coroutine);
        }

        IEnumerator main(Transform gemsRoot, IFieldState field) {
            yield return eraseGems(gemsRoot, field);
        }

        IEnumerator eraseGems(Transform gemsRoot, IFieldState field) {
            var eraseCount = 0;
            var count = 0;
            System.Action callback = () => count++;
            var gems = gemsRoot.GetComponentsInChildren<Gem>();
            foreach (var gem in gems) {
                var x = (int)(gem.transform.position.x);
                var y = (int)(gem.transform.position.y);

                if (!field.HasGem(x, y)) {
                    eraseCount++;
                    StartCoroutine(gem.Erase(callback));
                }
            }
            yield return new WaitUntil(() => count == eraseCount);
            yield return new WaitForSeconds(0.2f);
            Debug.Log("To Drop.");
            yield return null;
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
