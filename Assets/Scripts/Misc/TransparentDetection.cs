using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparentDetection : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTime = 0.4f;

    private SpriteRenderer spriteRenderer;
    private Tilemap tilemap;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();
    }

    //khi player di duoi tan la hoac cay
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<PlayerController>()) {
            //fade the tree
            if(spriteRenderer) 
            {
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, transparencyAmount));  //giai quyet duoc van de di duoi cay nhung chua giai quyet duoc van de di duoi la cay
            } 
            else if (tilemap) 
            {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, transparencyAmount)); //giai quyet duoc van de di duoi duoi la cay
            }
        }
    }

    //khi player khong di duoi tan la hoac cay
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<PlayerController>()) {
            //not fade the tree
            if(spriteRenderer)
            {
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, 1f));  //giai quyet duoc van de di duoi cay nhung chua giai quyet duoc van de di duoi la cay
            }
            else if(tilemap)
            {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, 1f));  //giai quyet duoc van de di duoi cay nhung chua giai quyet duoc van de di duoi la cay
            }
        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue, float targetTransparent) {
        float elapsedTime = 0;
        while(elapsedTime < fadeTime) 
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparent, elapsedTime / fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, /*spriteRenderer.color.a*/ newAlpha);
            yield return null;
        }
    }
    //kha nang cua C# la co the co 2 function giong ten
    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float targetTransparent) {
        float elapsedTime = 0;
        while(elapsedTime < fadeTime) 
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparent, elapsedTime / fadeTime);
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, newAlpha);
            yield return null;
        }
    }
}
