﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Kakera {
    public class PickerController : MonoBehaviour {
        [SerializeField]
        private Unimgpicker imagePicker;

        [SerializeField]
        private MeshRenderer imageRenderer;
        //private SpriteRenderer imageRenderer;

        //private Image imageRenderer;

        [SerializeField] private Texture defaultTexture;
        void Awake () {
            // デフォルトで電車のスプライトを用意する
            imageRenderer.material.mainTexture = defaultTexture;
            RailCreateManager.Instance.SelectTrain = defaultTexture;

            imagePicker.Completed += (string path) => {
                StartCoroutine (LoadImage (path, imageRenderer));
            };
        }

        public void OnPressShowPicker () {
            imagePicker.Show ("Select Image", "unimgpicker", 1024);
        }

        private IEnumerator LoadImage (string path, MeshRenderer output) {
            var url = "file://" + path;
            var www = new WWW (url);
            yield return www;

            var texture = www.texture;
            //Sprite texture_sprite = Sprite.Create (texture, new Rect (0, 0, 500, 500), Vector2.zero);
            if (texture == null) {
                Debug.LogError ("Failed to load texture url:" + url);
            }

            // output.sprite = texture_sprite;
            output.material.mainTexture = texture;

            //RailCreateManager.csのSelectTrainに入れることでどこのクラスでも取れる
            RailCreateManager.Instance.SelectTrain = texture;
        }
    }
}