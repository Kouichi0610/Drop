using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using System.Linq;
using System;
using UniRx;

namespace View {
    /// <summary>
    /// 落とした場所決定後の落下処理
    /// </summary>
    public class FallGems : MonoBehaviour
    {
        public void Initialize(IEnumerable<Gem> gems, IFieldState field) {
            StartCoroutine(WaitFall(gems, field));
        }

        IEnumerator WaitFall(IEnumerable<Gem> gems, IFieldState field) {
            var count = 0;
            System.Action callback = () => count++;

            var sortedGems = gems.OrderBy(g => g.transform.position.y);

            var prevX = 99;
            foreach (var g in sortedGems) {
                g.transform.SetParent(this.transform);
                var line = (int)(g.transform.position.x);
                var y = field.PiledHeight(line);
                if (line == prevX) y++;
                StartCoroutine(g.DropTo(y, callback));
                prevX = line;
            }
            yield return new WaitUntil(() => count == gems.Count());
            Debug.Log("Complete.");
        }
        
        void Start()
        {
        }

        void Update()
        {
        }
    }
}

