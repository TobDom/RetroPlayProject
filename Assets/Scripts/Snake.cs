using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax; //czas pomiedzy wykonaniem kolejnego ruchu
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<Vector2Int> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI pointsText;
    private bool ifAbleToMove;

    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private void Awake()
    {
        gridPosition = new Vector2Int(10, 10); //miejsce startu Snake
        gridMoveTimerMax = 0.3f; // ruch co 0,3 sekundy (1f = 1 sekunda)
        gridMoveTimer = gridMoveTimerMax; //ciagly ruch
        gridMoveDirection = new Vector2Int(1, 0); //domy�lnie ruch snake zacznie si� w prawo po ropocz�ciu gry, dzi�ki temu nie b�dzie sta� w miejscu zanim gracz wska�e Snake kierunek

        snakeMovePositionList = new List<Vector2Int>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>(); //tworze liste o typie SnakeBodyPart
        ifAbleToMove = true;
    }

    private void Update()
    {
        HandleInput();
        HandleGridMovement();
        pointsText.text = "Score: " + levelGrid.Points;
    }

    private void HandleInput()
    {
        if (!ifAbleToMove)
        {
            gridMoveDirection.x = 0;
            gridMoveDirection.y = 0;
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) //ruch w gore
        {
            if (gridMoveDirection.y != -1) //nie mozemy poruszac sie w dol, jesli obecnie idziemy w gore, sami by�my spowodowali kolizje z wlasnym ogonem
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = +1;
            }

        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) //ruch w dol
        {
            if (gridMoveDirection.y != +1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) //ruch w lewo
        {
            if (gridMoveDirection.x != +1)
            {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
            }

        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) //ruch w prawo
        {
            if (gridMoveDirection.x != -1)
            {
                gridMoveDirection.x = +1;
                gridMoveDirection.y = 0;
            }

        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime; //czas od poprzedniego sprawdzenia ruchu gracza
        if (gridMoveTimer >= gridMoveTimerMax) //czy od ostatniego ruchu min�o wystarczaj�co du�o czasu.
        {
            gridMoveTimer -= gridMoveTimerMax; //kod b�dzie si� odpala� co 1 sekunde w celu sprawdzenia dalszego ruchu
            

            snakeMovePositionList.Insert(0, gridPosition); //dodaje bie��c� pozycj� w�a na pocz�tek jego listy ruch�w

            gridPosition += gridMoveDirection; //aktualizuje pozycj� w�a na siatce na podstawie jego bie��cego kierunku ruchu

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood) //w mom�cie zjedzenia jab�ka w�� ro�nie
            {
                snakeBodySize++;
                CreateSnakeBodyPart();
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1) //sprawdzamy liste, w momencie kiedy wielkosc snake jest wi�ksza lub r�wna, odjemienimy 1
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            /* foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    Debug.Log("Dead", transform.position);
                }
                  
            } */



            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90); //-90 wynika z domy�lnych ustawie� unity, dzi�ki wprowadzeniu tego g�owa b�dzie si� poprawnie obraca� po skeceniu

            UpdateSnakeBodyParts();

         

            CheckColisionItself();
        }

        /* public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        } */


    }
  
    private void CheckColisionItself()
    {
        for (int i = 1; i < snakeMovePositionList.Count; i++)
        {
            if (gridPosition == snakeMovePositionList[i])
            {
                StartCoroutine(LoseTheGame());
            }
        }
    }

    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetGridPosition(snakeMovePositionList[i]);
        }
    }

    private float GetAngleFromVector(Vector2Int dir) //dzi�ki tej funckji g�owa Snake, b�dzie si� obraca� w kierunku obecengo ruchu po skr�caniu
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public List<Vector2Int> GetFullSnakeGridPositionList() //funkcja sprawdza nam aktuln� pozycj� naszego ca�ego snake i j� nam zwraca. Jest nam to potrzebne, �eby nie spawnowa� jab�ka w miejscu snake
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>()
        {
            gridPosition
        };
        gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }


    private class SnakeBodyPart
    {
        private Vector2Int gridPosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex) //Tworzy nam "miejsce" w unity,�eby pod�o�yc grafik� do cia�a snake w typie SpriteRenderer
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
            transform = snakeBodyGameObject.transform;
        }


        public void SetGridPosition(Vector2Int gridPosition)
        {
            this.gridPosition = gridPosition;
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //funckja wbudowana w unity 
    {
        if(collision.tag == "Wall")
        {
            StartCoroutine(LoseTheGame());
        }
    }


    private IEnumerator LoseTheGame()
    {
        Debug.Log("Game Over");
        ifAbleToMove = false;
        gameOverText.text = "GameOver\n Your score is: " + levelGrid.Points;
        yield return new WaitForSeconds(3);
        Debug.Log("Wait over");
        SceneManager.LoadScene("StartScene");
    }
}


