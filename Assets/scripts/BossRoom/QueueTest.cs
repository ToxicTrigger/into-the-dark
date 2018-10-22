using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueTest : MonoBehaviour {

    [System.Serializable]
    public struct Man
    {
        public string name;
        public float age;
    }

    public string name;
    public float age;

    public Queue<Man> test_queue;
    public List<Man> deque_man;

	void Start () {
        test_queue = new Queue<Man>();
	}
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Man test_man = new Man();
            test_man.name = name;
            test_man.age = age;

            test_queue.Enqueue(test_man);

            Debug.Log(test_queue.Peek().name + test_queue.Peek().age);
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            deque_man.Add((Man)test_queue.Dequeue());
            
            Debug.Log(test_queue.Peek().name + test_queue.Peek().age);
        }
		
	}
}
