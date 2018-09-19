using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgePuzzle : MonoBehaviour {
    
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    Direction dir;

    public enum Bridge_Point
    {
        Left,
        Center,
        Right,
        Deactivate
    }
    public Bridge_Point b_point;

    public float speed;
    public Transform point_center;
    public float distance;

    public bool play = false;

	void Start () {
        b_point = Bridge_Point.Deactivate;
        StartCoroutine(move_bridge(Direction.Down));
	}
	
	void Update () {
		
	}

    public void hit_switch(Direction _dir)
    {
        if (!loop)
        {
            if (b_point == Bridge_Point.Deactivate)
            {
                b_point = Bridge_Point.Center;
                dir = Direction.Up;
            }
            else
            {
                dir = _dir;
                switch (dir)
                {
                    case Direction.Left:
                        if (b_point == Bridge_Point.Left)
                        {
                            dir = Direction.Down;
                            b_point = Bridge_Point.Deactivate;
                        }
                        else
                        {
                            b_point--;
                        }
                        break;
                    case Direction.Right:
                        if (b_point == Bridge_Point.Right)
                        {
                            dir = Direction.Down;
                            b_point = Bridge_Point.Deactivate;
                        }
                        else
                        {
                            b_point++;
                        }
                        break;
                    default:
                        break;
                }
            }

            StartCoroutine(move_bridge(dir));
        }
    }

    bool loop;
    IEnumerator move_bridge(Direction _dir)
    {
        loop = true;
        Vector3 move_dir = Vector3.zero;

        switch (_dir)
        {
            case Direction.Left:
                move_dir = Vector3.left;
                break;
            case Direction.Right:
                move_dir = Vector3.right;
                break;
            case Direction.Up:
                move_dir = Vector3.up;
                transform.position = new Vector3(point_center.position.x, -distance, point_center.position.z);
                break;
            case Direction.Down:
                move_dir = Vector3.down;
                break;
            default:
                break;
        }

        while(loop)
        {
            transform.position += move_dir * speed * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);

            switch (_dir)
            {
                case Direction.Left:
                    switch (b_point)
                    {
                        case Bridge_Point.Left:
                            if (transform.position.x < point_center.position.x - distance)
                                loop = false;
                            break;
                        case Bridge_Point.Center:
                            if (transform.position.x < point_center.position.x)
                                loop = false;
                            break;
                        default:
                            break;
                    }
                    break;
                case Direction.Right:
                    switch (b_point)
                    {
                        case Bridge_Point.Right:
                            if (transform.position.x > point_center.position.x + distance)
                                loop = false;
                            break;
                        case Bridge_Point.Center:
                            if (transform.position.x > point_center.position.x)
                                loop = false;
                            break;
                        default:
                            break;
                    }
                    break;
                case Direction.Up:
                    if (transform.position.y > point_center.position.y)
                    {
                        loop = false;
                    }
                    break;
                case Direction.Down:
                    if (transform.position.y < point_center.position.y - distance)
                        loop = false;
                    break;
                default:
                    break;
            }
        }
    }

}
