using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Unit unit;
    public Cube[] cubes;
    private int nextCube = 0;
    
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
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                if(hit.collider.gameObject.tag == "Clickable")
                {
                    switch (this.state)
                    {
                        case State.MovePlayer:
                            this.unit.agent.enabled = true;
                            this.unit.agent.destination = hit.point;
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
                            break;
                    }
                }
            }            
        }
    }

    public void ChangeStateMovePlayer()
    {
        this.state = State.MovePlayer;
    }

    public void ChangeStatePlaceCubes()
    {
        this.state = State.PlaceCubes;
    }
}
