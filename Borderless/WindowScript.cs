﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowScript : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public int lockedFrameRate = 60;
    
    [Space]
    public Vector2Int defaultWindowSize;
    public Vector2Int borderSize;

    private Vector2 _deltaValue = Vector2.zero;
    private bool _maximized;

    private void Awake()
    {
        Application.targetFrameRate = lockedFrameRate;
        Application.runInBackground = false;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(3f);
        #if !UNITY_EDITOR
            ResetWindowSize();
        #endif
    }

    public void OnBorderBtnClick()
    {
        if (BorderlessWindow.framed)
            return;

        BorderlessWindow.SetFramedWindow();        
        BorderlessWindow.MoveWindowPos(Vector2Int.zero, Screen.width + borderSize.x, Screen.height + borderSize.y); // Compensating the border offset.
    }

    public void OnNoBorderBtnClick()
    {
        if (!BorderlessWindow.framed)
            return;

        BorderlessWindow.SetFramelessWindow();
        BorderlessWindow.MoveWindowPos(Vector2Int.zero, Screen.width - borderSize.x, Screen.height - borderSize.y);
    }

    public void ResetWindowSize()
    {
        BorderlessWindow.MoveWindowPos(Vector2Int.zero, defaultWindowSize.x, defaultWindowSize.y);
    }

    public void OnCloseBtnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Application.Quit();
    }

    public void OnMinimizeBtnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
        BorderlessWindow.MinimizeWindow();
    }

    public void OnMaximizeBtnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (_maximized)
            BorderlessWindow.RestoreWindow();
        else
            BorderlessWindow.MaximizeWindow();

        _maximized = !_maximized;
    }

    public void OnDrag(PointerEventData data)
    {
        if (BorderlessWindow.framed)
            return;

        _deltaValue += data.delta;
        if (data.dragging)
        {
            BorderlessWindow.MoveWindowPos(_deltaValue, Screen.width, Screen.height);
        }
    }

    public void OnPointerClick(PointerEventData eventData) => ResetWindowSize();
}
