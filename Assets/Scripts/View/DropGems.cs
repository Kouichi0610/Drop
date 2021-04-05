using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Rules;
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
        FieldSize size;

        List<Gem> gems = new List<Gem>();


        public void SetUp(IGemsFactory factory, FieldSize size) {
            this.factory = factory;
            this.startX = size.Width/2;
            this.startY = size.Height;
            this.size = size;
        }

        public void Initialize(params Gems[] gems) {
            int i = 0;
            foreach (var gem in gems) {
                var g = factory.Rent(gem);
                g.transform.SetParent(this.transform);
                g.transform.name = "Gem_" + i;
                g.transform.position = new Vector3(0, i, 0);
                this.gems.Add(g);
                i++;
            }
            transform.position = new Vector3(startX, startY, 0);
            StartCoroutine(Drop());
        }

        IEnumerator Drop() {
            // TODO:落下判定
            while (!IsGround()) {
                yield return new WaitForSeconds(1);
                var current = transform.position;
                var next = new Vector3(current.x, current.y-1, current.z);
                transform.position = next;
            }
            yield return null;
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                Left();
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                Right();
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                DecideDrop();
            } else if (Input.GetKeyDown(KeyCode.Space)) {
                Rotate();
            }
        }

        // 設置判定
        bool IsGround() {
            return transform.position.y <= 0;
        }

        void DecideDrop() {
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
            var right = size.Width;
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

        void Start()
        {
            
        }
    }
}
