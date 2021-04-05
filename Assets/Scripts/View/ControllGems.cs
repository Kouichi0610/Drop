using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Rules;
using UniRx;
using System;

namespace View {
    /// <summary>
    /// 落下中のジェム操作
    /// </summary>
    public class ControllGems : MonoBehaviour
    {
        IGemsFactory factory = null;
        int startX = 0;
        int startY = 0;
        IFieldState fieldState = null;

        IEnumerator dropCoroutine = null;
        bool keyActive = false;

        List<Gem> gems = new List<Gem>();

        public IObservable<List<Gem>> OnResult { get { return resultSubject.AsObservable(); } }

        Subject<List<Gem>> resultSubject = new Subject<List<Gem>>();

        public void SetUp(IGemsFactory factory, IFieldState field) {
            this.factory = factory;
            this.startX = field.Width/2;
            this.startY = field.Height;
            this.fieldState = field;
        }

        public void Initialize(params Gems[] gems) {
            transform.position = new Vector3(startX, startY, 0);
            transform.rotation = Quaternion.identity;
            int i = 0;
            foreach (var gem in gems) {
                var g = factory.Rent(gem);
                g.Initialize(gem);
                g.transform.SetParent(this.transform);
                g.transform.localPosition = new Vector3(0, i, 0);
                this.gems.Add(g);
                i++;
            }
            dropCoroutine = Drop();
            StartCoroutine(dropCoroutine);
        }

        IEnumerator Drop() {
            keyActive = true;
            while (!IsGround()) {
                yield return new WaitForSeconds(1);
                var current = transform.position;
                var next = new Vector3(current.x, current.y-1, current.z);
                transform.position = next;
            }
            DecideDrop();
        }

        void Update() {
            if (!keyActive) return;

            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                Left();
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                Right();
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                StopCoroutine(dropCoroutine);
                dropCoroutine = null;
                DecideDrop();
            } else if (Input.GetKeyDown(KeyCode.Space)) {
                Rotate();
            }
        }

        // 設置判定
        bool IsGround() {
            foreach (var g in gems) {
                var line = (int)g.transform.position.x;
                var y = fieldState.PiledHeight(line);

                if (transform.position.y <= y) {
                    return true;
                }
            }
            return false;
        }

        void DecideDrop() {
            keyActive = false;
            resultSubject.OnNext(gems);
            gems.Clear();
        }

        void Left() {
            var current = transform.position;
            var next = new Vector3(current.x-1, current.y, current.z);
            transform.position = next;
            HorizontalClip();
        }

        void Right() {
            var current = transform.position;
            var next = new Vector3(current.x+1, current.y, current.z);
            transform.position = next;
            HorizontalClip();
        }

        void Rotate() {
            var next = Quaternion.AngleAxis(-90, Vector3.forward);
            transform.rotation = transform.rotation * next;
            HorizontalClip();
        }

        void HorizontalClip() {
            var left = 0;
            var right = fieldState.Width-1;
            float append = 0;

            foreach (var g in gems) {
                var x = g.transform.position.x;
                if (x < left) {
                    append = left - x;
                } else if (x > right) {
                    append = right - x;
                }
            }
            var next = transform.position;
            next.x = next.x + append;
            transform.position = next;
        }
    }
}
