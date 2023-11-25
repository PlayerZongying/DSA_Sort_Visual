using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySorting : MonoBehaviour
{

    public static DisplaySorting instance;
    public int testTime = 10;
    public enum ArraySize : int
    {
        Ten = 10,
        Fifty = 50,
        Hundred = 100,
        FiveHundred = 500,
        Thousand = 1000,
        FiveThousand = 5000
    }

    public ArraySize arraySize;

    
    [Range(0f, 0.5f)]
    public float height = 0.5f;
    [Range(0f, 1f)]
    public float width = 1;

    public Material lineMaterial;
    
    [HideInInspector]
    public float[] arr;

    private LineRenderer[] lineRenderers;
    Camera mainCamera;
    private Vector3 bottomLeft;
    private Vector3 topRight;
    // Start is called before the first frame update

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        arr = GenerateArray();
        // PrintArray(arr);
        

        // Get the world positions of the four screen corners
        mainCamera = Camera.main;

        lineRenderers = new LineRenderer[5000];
        for (int i = 0; i < 5000; i++)
        {
            GameObject lineObject = new GameObject("LineRenderer_" + i);
            lineObject.transform.SetParent(transform);
            lineRenderers[i] = lineObject.AddComponent<LineRenderer>();
            lineRenderers[i].material = lineMaterial;
            // Set other properties of LineRenderer as needed
            lineRenderers[i].positionCount = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     RandomlySwap(arr);
        // }
        //
        // if (Input.GetMouseButtonDown(1))
        // {
        //     Sort(arr);
        // }

        // DrawArrayOnGizomos(arr);
        DrawArrayUsingLineRenderer(arr);
    }
    
    void OnGUI()
    {
        // DrawArrayOnGUI(arr);
    }

    float[] GenerateArray()
    {
        int size = (int)arraySize;
        float[] arr = new float[size];
        for (int i = 0; i < size; i++)
        {
            arr[i] = Mathf.Lerp(0, 1, (float)(i + 1) / size);
        }

        return arr;
    }

    public void RegenerateArray()
    {
        for (int i = (int)arraySize - 1; i < arr.Length; i++)
        {
            lineRenderers[i].SetPositions(new Vector3[]{Vector3.zero,Vector3.zero});
        }
        arr = GenerateArray();
    }

    void PrintArray(float[] arr)
    {
        foreach (var f in arr)
        {
            Debug.Log(f);
        }
    }
    
    public void OnSizeSelected(int index)
    {
        switch (index)
        {
            case 0:
                arraySize = ArraySize.Ten;
                break;
            case 1:
                arraySize = ArraySize.Fifty;
                break;
            case 2:
                arraySize = ArraySize.Hundred;
                break;
            case 3:
                arraySize = ArraySize.FiveHundred;
                break;
            case 4:
                arraySize = ArraySize.Thousand;
                break;
            case 5:
                arraySize = ArraySize.FiveThousand;
                break;
        }

        RegenerateArray();
    }

    // void DrawArrayOnGUI(float[] arr)
    // {
    //     for (int i = 0; i < arr.Length; i++)
    //     {
    //         // int screenWidth = main
    //         float x = ((float)(i + 1)/ arr.Length - 0.5f) * Screen.width * width + Screen.width * 0.5f;
    //         float y = arr[i] * Screen.height * height;
    //         Vector2 startPoint = new Vector2(x, 0);
    //         Vector2 endPoint = new Vector2(x, y);
    //         Color hsvColor = Color.HSVToRGB(arr[i], 0.75f, 1f);
    //         DrawLine(startPoint, endPoint, hsvColor, Screen.width * width/(float)arr.Length);
    //     }
    // }
    //
    // void DrawArrayOnGizomos(float[] arr)
    // {
    //     for (int i = 0; i < arr.Length; i++)
    //     {
    //         topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));
    //         bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
    //         float x = ((float)(i + 1)/ arr.Length - 0.5f) * (topRight.x - bottomLeft.x) * width;
    //         float y = arr[i]  * (topRight.y - bottomLeft.y)  * height;
    //         Vector3 startPoint = new Vector3(x, 0,0);
    //         Vector3 endPoint = new Vector3(x, y,0);
    //         Color hsvColor = Color.HSVToRGB(arr[i], 0.75f, 1f);
    //         Debug.DrawLine(startPoint, endPoint, hsvColor);
    //         // DrawLine(startPoint, endPoint, hsvColor, Screen.width * width/(float)arr.Length);
    //     }
    // }
    
    void DrawArrayUsingLineRenderer(float[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));
            bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            float x = ((float)(i + 0.5f)/ arr.Length - 0.5f) * (topRight.x - bottomLeft.x) * width;
            float y = arr[i]  * (topRight.y - bottomLeft.y)  * height;
            Vector3 startPoint = new Vector3(x, 0,0);
            Vector3 endPoint = new Vector3(x, y,0);
            Color hsvColor = Color.HSVToRGB(arr[i], 0.75f, 1f);
            // Debug.DrawLine(startPoint, endPoint, hsvColor);
            
            LineRenderer lineRenderer = lineRenderers[i];
            lineRenderer.startWidth = lineRenderer.endWidth = (topRight.x - bottomLeft.x) * width / arr.Length;
            lineRenderer.startColor = lineRenderer.endColor = hsvColor;
            lineRenderer.SetPositions(new Vector3[] { startPoint, endPoint });
        }
    }


    // void DrawLine(Vector2 start, Vector2 end, Color color, float width = 1f)
    // {
    //     GUI.color = color;
    //     GUI.skin.box.normal.background = Texture2D.whiteTexture;
    //
    //     Vector2 delta = end - start;
    //     float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
    //     float length = delta.magnitude;
    //
    //     GUIUtility.RotateAroundPivot(angle, start);
    //     GUI.Box(new Rect(start.x, start.y, length, width), GUIContent.none);
    //     GUIUtility.RotateAroundPivot(-angle, start);
    //     GUI.color = Color.white;
    // }


    void RandomlySwap(float[] array)
    {
        System.Random random = new System.Random();

        for (int i = array.Length - 1; i > 0; i--)
        {
            // Generate a random index to swap with
            int randomIndex = random.Next(0, i + 1);

            // Swap elements
            (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
        }
    }

    public void Randomize()
    {
        RandomlySwap(arr);
    }

    void Sort(float[] array)
    {
        System.Array.Sort(array);
    }
}