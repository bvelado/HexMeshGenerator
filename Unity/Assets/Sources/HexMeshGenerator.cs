using UnityEngine;
using System;

public class HexMeshGenerator : IMeshGenerator
{

    private float _size;
    public float Size
    {
        get
        {
            return _size;
        }
    }

    #region Private variables

    private float _height;
    protected float Height
    {
        get
        {
            return _height;
        }
    }

    private float _verticalDistance;
    private float VerticalDistance
    {
        get
        {
            return _verticalDistance;
        }
    }

    private float _width;
    private float Width
    {
        get
        {
            return _width;
        }
    }

    private float _horizontalDistance;
    private float HorizontalDistance
    {
        get
        {
            return _horizontalDistance;
        }
    }

#endregion

    public HexMeshGenerator(int hexagonSize)
    {
        _size = hexagonSize;
        _height = _size * 2;
        _verticalDistance = _height * 0.75f;
        _width = Mathf.Sqrt(3) / 2 * _height;
        _horizontalDistance = _width;
    }

    public Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();

        return mesh;
    }

    public Vector3[] GenerateVertices(int mapWidth, int mapHeight)
    {
        // Number of rows with an odd index
        int numberOfOddRows = mapHeight / 2;

        // Number of rows with an even index
        int numberOfEvenRows = Mathf.RoundToInt((mapHeight / 2) +0.5f);

        // For each row, south west and south vertices are registered
        int verticesPerLine = (mapWidth * 2) + 1;

        // number of hexagons + ( numbers of lines + 1 (top line) X the number of vertices needed per lines ) + 1 additional vertice per odd row + 1 additional vertice per even row (without first and last line )
        int verticesLength = (mapWidth * mapHeight) + ((mapHeight + 1) * verticesPerLine) + numberOfOddRows + (numberOfEvenRows - 2);

        Debug.Log(verticesLength);

        Vector3[] vertices = new Vector3[verticesLength];

        int i = 0;

        #region Regular rows

        for (int y = 0; y < mapHeight; y++)
        {
            // North west vertice oh the hexagon below
            if (IsOdd(y))
            {
                vertices[i] = new Vector3(-0.5f * Width, 0, (y * VerticalDistance) - (0.5f * Height));
                ++i;
            }

            // Hexagon south west, south and center vetices
            for (int x = 0; x < mapWidth; x++)
            {
                vertices[i] = new Vector3((x * HorizontalDistance) - (0.5f * Width), 0, (y * VerticalDistance) - (0.25f * Height));
                if (IsOdd(y))
                    vertices[i].x += 0.5f * HorizontalDistance;
                ++i;

                vertices[i] = new Vector3(x * HorizontalDistance, 0, (y * VerticalDistance) - (0.5f * Height));
                if (IsOdd(y))
                    vertices[i].x += 0.5f * HorizontalDistance;
                ++i;

                vertices[i] = new Vector3(x * HorizontalDistance, 0, (y * VerticalDistance));
                if (IsOdd(y))
                    vertices[i].x += 0.5f * HorizontalDistance;
                ++i;
            }

            // South east vertice oh the last hexagon
            vertices[i] = new Vector3(((mapWidth-1) * HorizontalDistance) + (0.5f * Width), 0, (y * VerticalDistance) - (0.25f * Height));
            if (IsOdd(y))
                vertices[i].x += 0.5f * HorizontalDistance;
            ++i;

            // North east vertice of the hexagon below
            if (y > 0 && !IsOdd(y))
            {
                vertices[i] = new Vector3(((mapWidth-1) * HorizontalDistance) + Width, 0, (y * VerticalDistance) - (0.5f * Height));
                ++i;
            }
        }

        #endregion

        #region Last top row

        // North west vertice oh the hexagon below
        vertices[i] = new Vector3((IsOdd(mapHeight) ? -0.5f * Width : 0), 0, (mapHeight * VerticalDistance) - (0.5f * Height));
        ++i;

        // Hexagon south west, south and center vetices
        for (int x = 0; x < mapWidth; x++)
        {
            //vertices[i] = new Vector3((x * HorizontalDistance), 0, (mapHeight * VerticalDistance) - (0.5f * Height));
            //if (IsOdd(mapHeight))
            //    vertices[i].x += 0.5f * Width;
            //++i;

            vertices[i] = new Vector3((x * HorizontalDistance), 0, (mapHeight * VerticalDistance) - (0.25f * Height));
            if (!IsOdd(mapHeight))
                vertices[i].x += 0.5f * HorizontalDistance;
            ++i;

            vertices[i] = new Vector3((x * HorizontalDistance), 0, (mapHeight * VerticalDistance) - (0.5f * Height));
            if (!IsOdd(mapHeight))
                vertices[i].x += HorizontalDistance;
            else
                vertices[i].x += 0.5f * HorizontalDistance;
            ++i;
        }

        #endregion

        Debug.Log(i);

        return vertices;
    }

    //private Vector3 GetHexCorner(Vector3 center, int index)
    //{
    //    var angle_degree = 60 * index + 30;
    //    var angle_radian = Mathf.PI / 180 * angle_degree;
    //    return new Vector3(center.x + Size * Mathf.Cos(angle_radian), 0, center.z + Size * Mathf.Sin(angle_radian));
    //}

    private bool IsOdd(int i)
    {
        return i % 2 != 0;
    }
}
