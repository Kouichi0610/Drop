using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using UniRx;

namespace View {
    /// <summary>
    /// 落下中のジェム、操作
    /// </summary>
    public class DropGems : MonoBehaviour
    {
        IGemsFactory factory = null;
        int startX = 0;
        int startY = 0;

        public void SetUp(IGemsFactory factory, Model.Rules.FieldSize size) {
            this.factory = factory;
            this.startX = size.Width/2;
            this.startY = size.Height;
        }

        public void Initialize(params Gems[] gems) {
            int i = 0;
            foreach (var gem in gems) {
                var g = factory.Rent(gem);
                g.transform.SetParent(this.transform);
                g.transform.name = "Gem_" + i;
                g.transform.position = new Vector3(0, i, 0);
                i++;
            }
            transform.position = new Vector3(startX, startY, 0);
            StartCoroutine(Drop());
        }

        IEnumerator Drop() {
            // TODO:落下判定
            while (transform.position.y > 0) {
                yield return new WaitForSeconds(1);
                var current = transform.position;
                var next = new Vector3(current.x, current.y-1, current.z);
                transform.position = next;
            }
            yield return null;
        }

        void Start()
        {
            
        }
    }
}
