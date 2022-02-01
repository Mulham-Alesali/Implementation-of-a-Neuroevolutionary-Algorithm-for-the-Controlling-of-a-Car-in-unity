using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : Graphic
{

    public Vector2Int gridSize;
    public UIGridRenderer grid;

    public List<Vector2> points;

    float width;
    float height;

    float unitWidth;
    float unitHeight;

    public float thickness = 10f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {

        vh.Clear();


        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / (float)gridSize.x;
        unitHeight = height / (float)gridSize.y;

        if(points.Count < 2)
        {
            return;
        }

        float angle = 0;
        for(int i = 0; i < points.Count; i++)
        {
            Vector2 point = points[i];
            if(i < points.Count - 1)
            {
                angle = GetAngle(points[i], points[i + 1]) + 45f;
            }
            DrawVerticesForPoint(point, vh,angle);
        }


        for(int i = 0; i < points.Count - 1; i++)
        {
            int index = i * 2;
            vh.AddTriangle(index + 0, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index + 0);

        }

    }

    public float GetAngle(Vector2 me, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - me.y, target.x - me.x) * (180 / Mathf.PI));
    }

    void DrawVerticesForPoint(Vector2 point,VertexHelper vh, float angle)
    {
        point -= new Vector2(gridSize.x / 2, gridSize.y / 2);
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;
        vertex.position = Quaternion.Euler(0,0,angle) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);

        vh.AddVert(vertex);

        
    }

    public void Update()
    {
        

        if(grid != null)
        {
            if(gridSize != grid.gridSize)
            {
                gridSize = grid.gridSize;
                var x2 = GameObject.Find("x2").GetComponent<Text>();
                var x3 = GameObject.Find("x3").GetComponent<Text>();
                var x4 = GameObject.Find("x4").GetComponent<Text>();
                var x5 = GameObject.Find("x5").GetComponent<Text>();
                var x6 = GameObject.Find("x6").GetComponent<Text>();
                x2.text = (gridSize.x / 5 * 1) + "";
                x3.text = (gridSize.x / 5 * 2) + "";
                x4.text = (gridSize.x / 5 * 3) + "";
                x5.text = (gridSize.x / 5 * 4) + "";
                x6.text = (gridSize.x / 5 * 5) + "";
            }

        }
     SetVerticesDirty();
    }

    internal void ResetUILineRenderer()
    {
        points.Clear();
        GetComponent<AverageFitnessDrawing>()?.Reset();
        GetComponent<FitnessDrawing>()?.Reset();
        SetVerticesDirty();
       
    }
}
