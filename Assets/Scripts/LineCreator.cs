using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    [SerializeField]
    private Line linePrefab;

    [SerializeField]
    private float minDistance;

    private Line currentLine;
    private Stack<Line> undoStack;

    private void Start()
    {
        undoStack = new();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            StartDraw();
        else if (Input.GetKeyUp(KeyCode.Mouse0))
            StopDraw();
        else if (currentLine)
            Draw();
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z))
            Undo();
    }

    private void StartDraw()
    {
        currentLine = Instantiate(linePrefab);
    }

    private void Draw()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentLine.UpdateLine(mousePos, minDistance);
    }

    private void StopDraw()
    {
        undoStack.Push(currentLine);
        currentLine = null;
    }

    private void Undo()
    {
        if (undoStack.Count == 0) return;

        Line lastLine = undoStack.Pop();
        Destroy(lastLine.gameObject);
    }
}
