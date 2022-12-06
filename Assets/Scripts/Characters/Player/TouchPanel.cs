using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private Vector2 _playerVectorOutput;
    private Touch _myTouch;
    private int _touchId;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            for(int i = 0; i < Input.touchCount; i++)
            {
                _myTouch = Input.GetTouch(i);
                if (_myTouch.fingerId == _touchId)
                {
                    if (_myTouch.phase != TouchPhase.Moved)
                    {
                        OutputVectorValue(Vector2.zero);
                    }
                }
            }
        }    
    }

    private void OutputVectorValue(Vector2 outputValue)
    {
        _playerVectorOutput = outputValue;
    }

    public Vector2 VectorOutput()
    {
        return _playerVectorOutput;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OutputVectorValue(Vector2.zero);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        _touchId = eventData.pointerId;
        _myTouch.fingerId = _touchId;
    }

    public void OnDrag(PointerEventData eventData)
    {
        OutputVectorValue(new Vector2(eventData.delta.normalized.x, eventData.delta.normalized.y)); 
    }
}
