using UnityEngine;

public class XGridLine : XMonoBehavior
{
    Material lineMaterial;
    protected override void OnAwake()
    {
        if (!lineMaterial)
        {
            lineMaterial = new Material(Shader.Find("Particles/Alpha Blended"));
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            bShowGrid = !bShowGrid;
        }
    }

    private bool bShowGrid = false;
    private int height = 16;
    private void OnPostRender()
    {
        if (bShowGrid)
            ShowGrid();

    }

    private void ShowGrid()
    {
        GL.PushMatrix();
        lineMaterial.SetPass(0);
        GL.LoadOrtho();

        GL.Begin(GL.LINES);
        GL.Color(Color.gray);
        int width = Screen.width * height / Screen.height;
        for (int i = 0; i < height; i++)
        {
            GL.Vertex3(0, (float)i / height, 0);
            GL.Vertex3(1, (float)i / height, 0);
        }

        for (int i = 0; i < width; i++)
        {
            GL.Vertex3((float)i / width, 0, 0);
            GL.Vertex3((float)i / width, 1, 0);
        }

        GL.End();
        GL.PopMatrix();
    }
}
