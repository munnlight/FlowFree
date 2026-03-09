using Unity.Collections;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject finishPanel;
    public GameObject dotPrefab;
    public int width;
    private Dot[] dots = new Dot[GridManager.width * GridManager.width];

    void Start()
    {
        int level = PlayerPrefs.GetInt("CurrentLevel", 1);

        // PlayerPrefs.SetInt("CurrentLevel", 1);
        // PlayerPrefs.Save();

        width = GridManager.width;
        Debug.Log("This: " + level);
        if (level > 5)
        {
            Debug.Log("iish");
            finishPanel.SetActive(true);
        }
        else
            LoadLevel("level" + level);

    }

    void LoadLevel(string levelName)
    {
        TextAsset json = Resources.Load<TextAsset>(levelName);
        LevelData level = JsonUtility.FromJson<LevelData>(json.text);
        GridManager.left = level.dotpair;

        foreach (DotData dot in level.dots)
        {
            Vector3 pos = new Vector3(dot.x - width / 2, dot.y - width / 2, -1);

            GameObject newDot = Instantiate(dotPrefab, pos, Quaternion.identity);

            Dot dotScript = newDot.GetComponent<Dot>();
            dotScript.gridPosition = new Vector2Int(dot.x, dot.y);
            dots[dot.x * width + dot.y] = dotScript;

            SetColor(newDot, dot.color);
        }
    }

    void SetColor(GameObject obj, string color)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        if (color == "red") sr.color = Color.red;
        if (color == "blue") sr.color = Color.blue;
        if (color == "green") sr.color = Color.green;
        if (color == "yellow") sr.color = Color.yellow;
        if (color == "orange") sr.color = Color.orange;
    }

    public Dot GetDot(int x, int y)
    {
        return dots[x * width + y];
    }
}