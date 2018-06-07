using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObject : MonoBehaviour {
    //보스의 솟아오르기 공격 이후 오브젝트를 떨굼 (일반 벽, 특수 벽, 몬스터)

    //특수 벽과 몬스터는 랜덤으로 낙하한다.
    public GameObject Interrupt_wall;
    public GameObject enemy;
    //특수 벽은 매번 랜덤한 위치에 깔린다. 아직 특별한 조건이 없으므로 필드 내에만 깔리게 하잡.... or 중앙에서 어느정도의 거리까지....
    [Tooltip("떨어지는 영역의 반지름 _ 맵의 중앙(보스가 떨어지는 곳)을 중심으로 한다.")]
    public float drop_field_radius;


    //일반 벽은 정해진 위치에 낙하한다.
    public GameObject normal_wall;
    public Vector3[] normal_wall_pos;   // 에디터에서 오브젝트를 깔고 그 위치값을 넣어줌 
    //일반 벽이 깔리는 시점은 어떻게 될까? 두번째 세번째 낙하시.... 
    //일단 최초1회만 깔린다. 


	void Start () {
		
	}
	

	void Update () {
		
	}
}
