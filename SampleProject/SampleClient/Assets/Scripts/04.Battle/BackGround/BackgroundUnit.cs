using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundUnit : MonoBehaviour
{
    [HideInInspector] public BackgroundUnit up;
    [HideInInspector] public BackgroundUnit upRight;
    [HideInInspector] public BackgroundUnit right;
    [HideInInspector] public BackgroundUnit downRight;
    [HideInInspector] public BackgroundUnit down;
    [HideInInspector] public BackgroundUnit downLeft;
    [HideInInspector] public BackgroundUnit left;
    [HideInInspector] public BackgroundUnit upLeft;

    private int _IndexLeft = 0;
    public int IndexLeft => _IndexLeft;
    private int _IndexRight = 0;
    public int IndexRight => _IndexRight;
    private int _IndexUp = 0;
    public int IndexUp => _IndexUp;
    private int _IndexDown = 0;
    public int IndexDown => _IndexDown;

    private int _Index = 0;
    public int Index => _Index;
    private Vector2 originalPosition;

    private Vector2 _Pos = Vector2.zero;
    public Vector2 Pos => _Pos;

    public int RowNumber;
    
    public int ColumnNumber;

    void Start()
    {
        //var tex1 = GetComponent<MeshRenderer>().material.GetTexture(0);
        //var tex2d = GetComponent<MeshRenderer>().materials[0].mainTexture as Texture2D;

        //for (int i = 0; i < tex2d.width; i++)
        //{
        //    for (int j = 0; j < tex2d.height; j++)
        //    {
        //        tex2d.SetPixel(i, j, Color.white);
        //    }
        //}

        //tex2d.Apply();

        // Store the original position of the panel
        originalPosition = transform.position;
    }

    public void Init(int index)
    {
        _Index = index;

        int row = _Index / RowNumber;
        int col = _Index % ColumnNumber;

        _IndexLeft = GetWrappedIndex(row, col - 1);
        _IndexRight = GetWrappedIndex(row, col + 1);
        _IndexUp = GetWrappedIndex(row + 1, col);
        _IndexDown = GetWrappedIndex(row - 1, col);

        // Store the panel position for reference
        _Pos = new Vector2(col * 10, row * 10);

        transform.localPosition = _Pos;

    }

    private int GetWrappedIndex(int row, int col)
    {
        // Wrap the row and column indices around the grid size (3x3 grid)
        row = (row + RowNumber) % RowNumber;
        col = (col + ColumnNumber) % ColumnNumber;
        return row * ColumnNumber + col;
    }

    public void MoveToPosition(Vector3 newPosition)
    {
        _Pos = newPosition;
        transform.localPosition = _Pos;
    }

    public void ResetPosition()
    {
        transform.localPosition = originalPosition;
    }
}