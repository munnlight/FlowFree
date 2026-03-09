using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DragState
{
    Yes, No
}

public class InputManager : MonoBehaviour
{
    public GridManager gridManager;
    public LevelLoader levelLoader;
    public GameManager gameManager;
    private DragState isDragging = DragState.No;
    private Dot dragDot;
    private int posX, posY;
    private Dictionary<Color, List<Cell>> path = new();
    private List<Color> connected = new();
    private bool levelCleared = false;

    void Update()
    {
        Vector2 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D click = Physics2D.OverlapPoint(world);
            if (click == null) return;
            Dot dot = click.GetComponent<Dot>();

            if (dot != null)
            {
                Cell cell = GetCellByPosition(dot);
                Color color = dot.GetComponent<SpriteRenderer>().color;
                SetColor(cell, color);

                isDragging = DragState.Yes;
                dragDot = dot;
            }
            else
            {
                Cell cell = click.GetComponent<Cell>();

                if (cell != null && cell.occupied)
                {
                    isDragging = DragState.Yes;
                    dragDot = GetDotByPosition(path[cell.color][0]);

                    if (dragDot == null)
                    {
                        Debug.Log(cell.gridPosition);
                    }
                }
            }
        }

        if (isDragging == DragState.Yes)
        {
            posX = Mathf.RoundToInt(world.x);
            posY = Mathf.RoundToInt(world.y);

            if (Mathf.Abs(posX) > GridManager.width / 2 || Mathf.Abs(posY) > GridManager.height / 2) return;

            Collider2D loc = Physics2D.OverlapPoint(new Vector2(posX, posY));
            Cell cell = loc.GetComponent<Cell>();

            if (cell != null)
            {
                Color color = dragDot.GetComponent<SpriteRenderer>().color;
                SetColor(cell, color);
            }
            else
            {
                Dot dot = loc.GetComponent<Dot>();

                if (dot != null)
                {
                    Cell dotCell = GetCellByPosition(dot);
                    Color dotColor = dot.GetComponent<SpriteRenderer>().color;
                    SetColor(dotCell, dotColor);

                    if (!dragDot.Equals(dot) && dotColor == dragDot.GetComponent<SpriteRenderer>().color)
                    {
                        Color addColor = dragDot.GetComponent<SpriteRenderer>().color;
                        if (!connected.Contains(addColor))
                        {
                            connected.Add(addColor);
                            isDragging = DragState.No;
                            Debug.Log("NUmber " + connected.Count + dragDot.gridPosition + dot.gridPosition);
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = DragState.No;
        }

        if (CheckWin() && !levelCleared)
        {
            levelCleared = true;
            Debug.Log("YOu wind");
            int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
            PlayerPrefs.Save();

            gameManager.ShowNextButton();
            // gameManager.WinLevel();
        }
    }

    public Cell GetCellByPosition(Dot dot)
    {
        return gridManager.GetCell(dot.gridPosition.x, dot.gridPosition.y);
    }

    public Dot GetDotByPosition(Cell cell)
    {
        return levelLoader.GetDot(cell.gridPosition.x, cell.gridPosition.y);
    }

    public void SetColor(Cell cell, Color color)
    {
        if (!path.ContainsKey(color))
            path[color] = new();

        if (cell.occupied) RemovePath(cell, color);

        path[color].Add(cell);
        cell.occupied = true;
        cell.color = color;
        SpriteRenderer render = cell.GetComponent<SpriteRenderer>();
        render.color = color;
    }

    public void RemovePath(Cell cell, Color color)
    {
        Color removeColor = cell.GetComponent<SpriteRenderer>().color;
        // Debug.Log("Remove orob" + removeColor + color);
        if (color != removeColor)
        {
            foreach (Cell c in path[removeColor])
            {
                SpriteRenderer render = c.GetComponent<SpriteRenderer>();
                render.color = Color.black;
                c.occupied = false;
            }

            path[cell.color].Clear();
        }
        else
        {
            for (int i = path[removeColor].Count - 1; i >= 0; i--)
            {
                Cell c = path[color][i];
                if (c.Equals(cell)) break;
                // Debug.Log("IISHEE");
                SpriteRenderer render = c.GetComponent<SpriteRenderer>();
                render.color = Color.black;
                c.occupied = false;
                path[color].Remove(c);
            }
        }


        if (connected.Contains(removeColor))
        {
            Debug.Log("Number before removing " + connected.Count);
            connected.Remove(removeColor);
            Debug.Log("Number after removing " + connected.Count);
        }
    }

    public bool CheckWin()
    {
        return GridManager.left - connected.Count == 0;
    }
}