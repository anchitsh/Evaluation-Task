using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Unit unit;
    public Cube[] cubes;
    private int nextCube = 0;
    public AudioSource clickAS;
    public AudioSource teleportAS;
    public AudioSource cubeAppearAS;
    public Button movePlayerButton;
    public Button placeCubesButton;

    public Image transitionImage;

    public enum State
    {
        MovePlayer,
        PlaceCubes
    }

    public State state = State.MovePlayer;

    void Start()
    {
        instance = this;
        this.state = State.MovePlayer;
        StartCoroutine(FadeIn());
        movePlayerButton.Select();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                if (hit.collider.gameObject.tag == "Clickable")
                {
                    switch (this.state)
                    {
                        case State.MovePlayer:
                            if(this.unit.canMove == true)
                            {
                                this.unit.agent.enabled = true;
                                this.unit.agent.destination = hit.point;
                            }
                            break;
                        case State.PlaceCubes:
                            this.cubes[nextCube].transform.position = hit.point + new Vector3(0, 0.5f, 0);
                            this.cubes[nextCube].StopAllCoroutines();
                            this.cubes[nextCube].Spawn();
                            nextCube++;
                            if (nextCube > cubes.Length - 1)
                            {
                                nextCube = 0;
                            }
                            cubeAppearAS.Play();
                            break;
                    }
                }
                else if(hit.collider.gameObject.tag == "Cube")
                {
                    if(this.state == State.MovePlayer && this.unit.canMove == true)
                    {
                        this.unit.agent.enabled = true;
                        this.unit.agent.destination = hit.point;
                    }
                }
            }       
        }
        HotKeys();
    }

    public void ChangeStateMovePlayer()
    {
        clickAS.Play();
        this.state = State.MovePlayer;
    }

    public void ChangeStatePlaceCubes()
    {
        clickAS.Play();
        this.state = State.PlaceCubes;
    }

    public IEnumerator FadeIn()
    {
        transitionImage.gameObject.SetActive(true);
        transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 1);
        transitionImage.DOColor(new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 0), .5f);
        yield return new WaitForSeconds(.5f);
        transitionImage.gameObject.SetActive(false);
    }

    public IEnumerator Exit()
    {
        transitionImage.gameObject.SetActive(true);
        transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 0);
        transitionImage.DOColor(new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 1), .5f);
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

    public IEnumerator Restart()
    {
        transitionImage.gameObject.SetActive(true);
        transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 0);
        transitionImage.DOColor(new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 1), .5f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HotKeys()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeStateMovePlayer();
            movePlayerButton.Select();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            ChangeStatePlaceCubes();
            placeCubesButton.Select();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Restart());
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(Exit());
        }
    }
}
