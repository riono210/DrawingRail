using UnityEngine;
using System.Collections;

namespace Kakera
{
    public class PickerController : MonoBehaviour
    {
        [SerializeField]
        private Unimgpicker imagePicker;

        [SerializeField]
        //private MeshRenderer imageRenderer;
        private SpriteRenderer imageRenderer;

        void Awake()
        {
            imagePicker.Completed += (string path) =>
            {
                StartCoroutine(LoadImage(path, imageRenderer));
            };
        }

        public void OnPressShowPicker()
        {
            imagePicker.Show("Select Image", "unimgpicker", 1024);
        }

        private IEnumerator LoadImage(string path, /*MeshRenderer*/ SpriteRenderer output)
        {
            var url = "file://" + path;
            var www = new WWW(url);
            yield return www;

            var texture = www.texture;
            Sprite texture_sprite = Sprite.Create(texture, new Rect(0, 0, 1500, 1500), Vector2.zero);
            if (texture_sprite == null)
            {
                Debug.LogError("Failed to load texture url:" + url);
            }

            output.sprite = texture_sprite;
        }
    }
}