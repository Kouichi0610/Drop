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
        public IObservable<DropGems> OnDropped { get { return subject.AsObservable(); } }
        Subject<DropGems> subject = new Subject<DropGems>();

        IEnumerator coroutine = null;

        public void Initialize(IEnumerable<Gem> gems, IFieldState field) {
            coroutine = WaitFall(gems, field);
            StartCoroutine(coroutine);
        }

        IEnumerator WaitFall(IEnumerable<Gem> gems, IFieldState field) {
            var count = 0;
            System.Action callback = () => count++;

            var sortedGems = gems.OrderBy(g => g.transform.position.y);

            var results = new List<DropGem>();

            var prevX = 99;
            foreach (var g in sortedGems) {
                g.transform.SetParent(this.transform);
                var line = (int)(g.transform.position.x);
                var y = field.PiledHeight(line);
                if (line == prevX) y++;
                StartCoroutine(g.DropTo(y, callback));
                prevX = line;
                results.Add(new DropGem(g.GemType, line));
            }
            yield return new WaitUntil(() => count >= gems.Count());
            yield return new WaitForSeconds(0.3f);

            StopCoroutine(coroutine);
            coroutine = null;

            subject.OnNext(new DropGems(results));
        }
    }
}

