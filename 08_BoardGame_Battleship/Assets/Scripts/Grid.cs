
using TMPro;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject letterPrefab;

    const int gridLineCount = 11;

    private void Awake()
    {
        DrawGridLine();
        DrawGridLetter();
    }

    void DrawGridLine()
    {
        int half = Mathf.FloorToInt(gridLineCount * 0.5f);
        int start = -half;
        int end = gridLineCount - half;
        for (int i = start; i < end; i++)
        {
            GameObject line = Instantiate(linePrefab, transform);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(i, 1, -half));
            lineRenderer.SetPosition(1, new Vector3(i, 1, gridLineCount - half));
        }

        for (int i = start; i < end; i++)
        {
            GameObject line = Instantiate(linePrefab, transform);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(-1 - half, 1, i));
            lineRenderer.SetPosition(1, new Vector3(gridLineCount - 1 - half, 1, i));
        }
    }

    void DrawGridLetter()
    {
        int half = Mathf.FloorToInt(gridLineCount * 0.5f);
        int start = -half;
        int end = gridLineCount - 1 - half;
        for (int i = start; i < end; i++)
        {
            GameObject letter = Instantiate(letterPrefab, transform);
            letter.transform.position = new Vector3(i + 0.5f, 1, half + 0.5f );
            TextMeshPro text = letter.GetComponent<TextMeshPro>();
            char c = (char)(65 + i + half );
            text.text = c.ToString();
        }

        for (int i = start; i < end; i++)
        {
            GameObject letter = Instantiate(letterPrefab, transform);
            letter.transform.position = new Vector3(-half - 0.5f, 1, -i - 0.5f);
            TextMeshPro text = letter.GetComponent<TextMeshPro>();
            text.text = (i + 1 + half).ToString();
            if( i+half >= 9)
            {
                text.fontSize = 8;
            }
        }
    }
}
