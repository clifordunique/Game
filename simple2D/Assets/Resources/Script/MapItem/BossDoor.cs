using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [SerializeField]
    GameObject boss;
    private bool isOpen;

    SpriteRenderer spriteRenderer;
    Color fading = new Color(0, 0, 0, 0.2f);
    float fadingInterval = 0.05f;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (boss == null)
        {

        }
    }

    public void SetOpen(bool state)
    {
        if (state)
        {
            StartCoroutine(Opening());
        }
        else
        {
            StartCoroutine(Closing());
        }
    }

    IEnumerator Opening()
    {
        while (spriteRenderer.color.a > 0)
        {
            spriteRenderer.color -= fading;
            yield return new WaitForSeconds(fadingInterval);
        }
        /*animator.Play("open");
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);*/
        gameObject.layer = LayerMask.NameToLayer("Enemy_dying");
        isOpen = true;
    }
    IEnumerator Closing()
    {
        while (spriteRenderer.color.a < 1)
        {
            spriteRenderer.color += fading;
            yield return new WaitForSeconds(fadingInterval);
        }
        /*animator.Play("close");
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);*/
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
        isOpen = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isOpen && boss == null)
        {
            SetOpen(true);
        }
    }
}
