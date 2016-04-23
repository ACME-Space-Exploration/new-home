using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class PositionBasedSortingOrder : MonoBehaviour
{    
    private SpriteRenderer _spriteRenderer;

    void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

	void Update () {
	    _spriteRenderer.sortingOrder = Mathf.CeilToInt(transform.position.y * -1000);
    }
}
