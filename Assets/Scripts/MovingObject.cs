using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float MoveTime = 0.0f;
    public LayerMask BlockingLayer;

    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb2d;
    private float _inverseMoveTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rb2d = GetComponent<Rigidbody2D>();
       
    }

    protected bool Move(float xDir, float yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        _boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, BlockingLayer);
        _boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    protected virtual void AttemptMove<T>(float xDir, float yDir) where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
        {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();
        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }


    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(_rb2d.position, end, 60 * Time.deltaTime);
            _rb2d.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }

    protected abstract void OnCantMove<T>(T component)
         where T : Component;

}
