using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseSign : MonoBehaviour {
	[Tooltip("시그니처 이미지가 들어가야 합니다")]
	public SpriteRenderer SignatureImage;
	[Tooltip("해당 거점은 몇개가 있는지?")]
	public TextMesh BaseCount;
	public Player player;
	public GameObject totem_img;
	public Base baseInfo;
	public Color panel;
	SpriteRenderer sr;
	// Use this for initialization
	void Start () {
		SignatureImage = transform.GetChild(0).GetComponent<SpriteRenderer>();
		BaseCount = transform.GetChild(1).GetComponent<TextMesh>();
		panel = GetComponent<SpriteRenderer>().color;
		player = FindObjectOfType<Player>();
		baseInfo = transform.parent.GetComponent<Base>();
		sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(baseInfo.TotemIn)
		{
			panel.a = 1;
			SignatureImage.gameObject.SetActive(true);
			BaseCount.gameObject.SetActive(true);
			transform.LookAt(Camera.main.transform);
		}else{
			panel.a = 0;
			SignatureImage.gameObject.SetActive(false);
			BaseCount.gameObject.SetActive(false);
		}
		sr.color = panel;
	}
}
