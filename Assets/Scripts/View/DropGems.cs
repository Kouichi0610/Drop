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
            /*
            if (Input.GetButtonDown("Left")) {
                Left();
            } else if (Input.GetButtonDown("Right")) {
                Right();
            } else if (Input.GetButtonDown("Down")) {
                DecideDrop();
            } else if (Input.GetButtonDown("Space")) {
                Rotate();
            }
            */
        }


        // 設置判定
        bool IsGround() {
            return transform.position.y <= 0;
        }

        void DecideDrop() {
            Debug.Log("Decide.");
        }

        void Left() {
            var current = transform.position;
            var next = new Vector3(current.x-1, current.y, current.z);
            transform.position = next;
        }
        void Right() {
            var current = transform.position;
            var next = new Vector3(current.x+1, current.y, current.z);
            transform.position = next;
        }

        void Rotate() {
            Debug.Log("Rotate.");
        }

        void Start()
        {
            
        }
    }
}
