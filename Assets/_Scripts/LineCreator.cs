using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    [Header("This class currently handles all types of input from the user.")]
    [Space(1f)]

    [Header("Lines")]
    [SerializeField] private Line linePrefab;
    [SerializeField] private float minDistance;
    private Line currentLine;
    private Stack<Line> undoStack;

    [Header("Player")]
    [SerializeField] private Rigidbody2D playerBody;
    private Vector3 playerStartPos;
    private float playerStartRot;
    private PlayerStats playerStats;

    [Header("Camera")]
    [SerializeField] private LerpPosition cameraLerp;
    [SerializeField] private Camera cam;
    private Vector3 cameraStartPos;
    private Vector3 mouseAnchor;

    private void Start()
    {
        playerStats = playerBody.GetComponent<PlayerStats>();
        Cursor.lockState = CursorLockMode.Confined;
        undoStack = new();

        if (!cam) cam = Camera.main;
        if (!cameraLerp) cameraLerp = cam.GetComponent<LerpPosition>();
        if (!playerBody) playerBody = GetComponent<Rigidbody2D>();
        if (!linePrefab) Debug.LogWarning("NO LINE PREFAB WTF!!!");
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
            UndoLine();
        else if (Input.GetKeyDown(KeyCode.Space))
            StartStopSimulation();
        else if (Input.GetKeyDown(KeyCode.Mouse1))
            StartDragCamera();
        else if (Input.GetKey(KeyCode.Mouse1))
            DragCamera();
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

    private void UndoLine()
    {
        if (undoStack.Count == 0) return;

        Line lastLine = undoStack.Pop();
        Destroy(lastLine.gameObject);
    }

    private void StartStopSimulation()
    {
        playerBody.simulated = !playerBody.simulated;

        if (playerBody.simulated)
        {
            // Start
            playerStartPos = playerBody.transform.position;
            playerStartRot = playerBody.transform.eulerAngles.z;
            cameraLerp.ToFollow = playerBody.transform;
            cameraStartPos = cameraLerp.transform.position;
            cameraLerp.enabled = true;
            playerStats.Restart();
        }
        else
        {
            // Stop
            playerBody.transform.position = playerStartPos;
            playerBody.transform.eulerAngles = new Vector3(0, 0, playerStartRot);
            playerBody.velocity = Vector2.zero;
            playerBody.angularVelocity = 0f;
            cameraLerp.transform.position = cameraStartPos;
            cameraLerp.enabled = false;
        }
    }

    private void StartDragCamera()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseAnchor = mousePos;
    }

    private void DragCamera()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 diff = mouseAnchor - mousePos;
        cam.transform.position += diff;
        //mouseAnchor -= diff;
        mouseAnchor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
