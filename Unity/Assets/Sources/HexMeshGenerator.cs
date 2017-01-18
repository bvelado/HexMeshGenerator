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

        var vertices = GenerateVertices(4,4);
        var triangles = GenerateTriangles(4,4);

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.Optimize();
        mesh.RecalculateNormals();

        return mesh;
    }

    public Vector3[] GenerateVertices(int mapWidth, int mapHeight)
    {
        // Number of rows with an odd index
        int numberOfOddRows = Mathf.RoundToInt(mapHeight / 2f);

        // Number of rows with an even index
        int numberOfEvenRows = Mathf.RoundToInt((mapHeight / 2f) + 0.6f);

        // For each row, south west and south vertices are registered
        int verticesPerLine = (mapWidth * 2) + 1;

        // number of hexagons + ( numbers of lines + 1 (top line) X the number of vertices needed per lines ) + 1 additional vertice per odd row + 1 additional vertice per even row (without first and last line )
        int verticesLength = (mapWidth * mapHeight) + ((mapHeight + 1) * verticesPerLine) + numberOfOddRows + (numberOfEvenRows - 2);

        /// IMPORTANT
        /// TODO
        /// Find why it sometimes need an additional vertice

        //verticesLength += 1;

        Vector3[] vertices = new Vector3[verticesLength];

        Debug.Log("Vertices length : " + verticesLength);

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

        Debug.Log("Vertices used : " + i);

        return vertices;
    }

    public int[] GenerateTriangles(int mapWidth, int mapHeight)
    {
        int[] triangles = new int[mapWidth * mapHeight * 6 * 3];

        int i = 0;
        for(int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                int oddEvenRowIndexModifier = y > 0 && !IsOdd(y) ? 0 : 2;
                int yIndex = y * ((mapWidth * 3) + oddEvenRowIndexModifier);

                // South west triangle
                triangles[i] = yIndex + (x * 3);
                triangles[i + 1] = yIndex + 2 + (x * 3);
                triangles[i + 2] = yIndex + 1 + (x * 3);

                // South east triangle
                triangles[i + 3] = yIndex + 1 + (x * 3);
                triangles[i + 4] = yIndex + 2 + (x * 3);
                triangles[i + 5] = yIndex + 3 + (x * 3);

                // East triangle
                triangles[i + 3] = yIndex + 1 + (x * 3);
                triangles[i + 4] = yIndex + 2 + (x * 3);
                triangles[i + 5] = yIndex + 3 + (x * 3);

                i += 6;
            }
        }

        Debug.Log(i);

        return triangles;
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
