using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Inventory 컴포넌트와 연계 됩니다.

public class InventoryViewer : MonoBehaviour 
{
	public Inventory Inventory;
	public RectTransform info;
	public Text Name;
	public Text information;
	public Image[] inven;
	
	private void Awake() 
	{
		Name = info.GetChild(0).GetComponent<Text>();
		information = info.GetChild(1).GetComponent<Text>();
	}
	
	void Update () 
	{
		foreach(Image tmp in inven)
		{
			tmp.gameObject.SetActive(false);
		}

		int i = 0;
		IEnumerator iter = Inventory.Items.GetEnumerator();
		while(iter.MoveNext())
		{
			ItemInfo info = iter.Current as ItemInfo;
			inven[i].sprite = info.icon;
			inven[i].name = info.name;
			inven[i].gameObject.SetActive(true);
			i ++;
		}
	}
}
