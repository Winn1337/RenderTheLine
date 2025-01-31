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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            StartDraw();
        else if (Input.GetKeyUp(KeyCode.Mouse0))
            StopDraw();
        else if (currentLine)
            Draw();
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
        currentLine = null;
    }
}
