using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리 기능을 구현하기 위해 만들었습니다.
// 해당 컴포넌트는 단순히 아이템을 저장하는 것 뿐만 아니라
// 적용된 객체의 Damageable 의 Hp 가 0 이 되었을 때 drop 기능을 구현하고 있습니다.
public class Inventory : MonoBehaviour 
{
	public int MAX_ITEM = 6;
	public List<ItemInfo> Items;
	public Damageable Live;
	public bool Drop;

	private void Awake() 
	{
		Live = GetComponent<Damageable>();	
	}
	
	void Update () 
	{
		if(Drop)
		{
			if(Live.Dead)
			{
				//Drop
			}
		}
	}
}
