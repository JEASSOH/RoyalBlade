using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingScript : MonoBehaviour
{
	public Rigidbody2D rigidBody = null;
	public Rigidbody2D playerBody = null;
	public float tingSpeed = 10.0f;
	public float playerBouncePower = 100;
	public Sprite sprite;


	bool attackChecker = false;





	int currentMaxHeight = 0;
	BoxCollider2D myCollider = null;
	int hp = 0;

	int playerHp = 3;

	bool wazaIng = false;

	class Block
	{
		public GameObject obj;

	};
	Queue<Block> blockQueue;
	Queue<Block> blockPool;

	void CreateBlockPool(int maxBlockCount)
	{
		blockPool = new Queue<Block>();
		for (int i = 0; i < maxBlockCount; ++i)
		{
			Block newBlock = new Block();

			// 오브젝트 셋팅
			newBlock.obj = new GameObject();
			newBlock.obj.AddComponent<SpriteRenderer>().sortingLayerName = "Building";
			newBlock.obj.transform.SetParent(transform);
			newBlock.obj.transform.localPosition = new Vector3(0, 120 * i, 0);


			blockPool.Enqueue(newBlock);
			newBlock.obj.SetActive(false);
		}
	}

	void Start()
	{
		
		hp = Random.Range(1, 3 + 1);
		myCollider = gameObject.GetComponent<BoxCollider2D>();
		CreateBlockPool(100);
		blockQueue = new Queue<Block>();
		Init(5, rigidBody.mass, rigidBody.gravityScale);
	

	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.name == "Floor")
		{
	
			isTuti = true;
		}

	}

	public bool GetTuti() {return isTuti; }
	bool isTuti = false;
	void OnCollisionExit2D(Collision2D coll)
	{
		if (coll.gameObject.name == "Floor")
		{
			isTuti = false;
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.name == "Sword_Defense" && !isTuti)
		{
			rigidBody.velocity = new Vector2(0, tingSpeed);
			playerBody.isKinematic = true;
			playerBody.isKinematic = false;
			playerBody.AddForce(new Vector2(0, -1 * playerBouncePower));

		}
	}

	void Update()
	{
		
	}

	
	void Init(int height, float mass, float gravityScale)
	{
		wazaIng = false;
		currentMaxHeight = height;
		// 블록 생성하는 부분
		for (int i = 0; i < height; ++i)
		{
			Block newBlock = blockPool.Dequeue();
			newBlock.obj.SetActive(true);


			int spriteNum = 0;
			if (i == 0) spriteNum = 4;
			else if (i == height - 1) spriteNum = 0;
			else spriteNum = Random.Range(1, 4);

			// 오브젝트 셋팅
			newBlock.obj.GetComponent<SpriteRenderer>().sprite = sprite;
			newBlock.obj.transform.SetParent(transform);
			newBlock.obj.transform.localPosition = new Vector3(0, 120 * i, 0);

			blockQueue.Enqueue(newBlock);
		}

		// 하늘위로 올려놓는 부분
		gameObject.transform.position = new Vector3(0, 500, 0);
		rigidBody.mass = mass;
		rigidBody.gravityScale = gravityScale;
		// 충돌박스 초기화
		myCollider.size = new Vector2(480, 120 * height);
		myCollider.offset = new Vector2(0, 60 * height);
	}

	uint score = 0;
	void Slashed()
	{
		if (blockQueue.Count <= 0)
			return;

		score += 1;
		

		var slashedBlock = blockQueue.Dequeue();
	

		// 한칸씩 잘리는 경우. 충돌박스를 수정해준다.
		slashedBlock.obj.SetActive(false);
		blockPool.Enqueue(slashedBlock);

		myCollider.size = myCollider.size - new Vector2(0, 120);
		myCollider.offset = myCollider.offset + new Vector2(0, 60);


		if (blockQueue.Count <= 0)
			Init(currentMaxHeight + 1, rigidBody.mass, rigidBody.gravityScale + 0.5f);
	}
	IEnumerator DisableAfterTime(GameObject obj, float time)
	{
		yield return new WaitForSeconds(time);
		obj.SetActive(false);
	}
}