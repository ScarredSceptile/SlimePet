using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [System.Serializable]
    public class StringUnityEvent : UnityEvent<string>
    {

    }

    [SerializeField]
    private StringUnityEvent _onCompleteEvent;

    [SerializeField]
    GameObject _particleSystem;

    [SerializeField]
    protected int seconds;
    public float timeLeft;
    private bool dragging;

    [SerializeField]
    protected string itemType;

    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 defaultPosition;

    void Awake()
    {
        defaultPosition = transform.position;
        timeLeft = seconds;
        dragging = true;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        defaultPosition = transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (dragging)
        {
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            transform.position = cursorPosition;
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        transform.position = defaultPosition;
        dragging = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Slime")
        {
            if (_particleSystem)
            {
                _particleSystem.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Slime")
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                _onCompleteEvent.Invoke(itemType);
                dragging = false;
                transform.position = defaultPosition;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Slime")
        {
            //Yes, I am this evil :)
            timeLeft = seconds;
            if (_particleSystem)
            {
                _particleSystem.GetComponent<ParticleSystem>().Stop();
            }
        }
    }
}
