using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGroup : MonoBehaviour
{
    public GameObject blockPref;
    private List<Block> blocks = new List<Block>();
    public int number;
    public Transform targetContainer;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < number; i++)
		{
            Block block = Instantiate(blockPref, transform).GetComponent<Block>();
            block.gameObject.SetActive(true);
            block.transform.localPosition = Vector3.up * i * 0.2f;
            blocks.Add(block);
		}
    }

    public void MoToTarget()
	{
        StartCoroutine(Move());
	}

    IEnumerator Move()
	{
        Vector3 target = targetContainer.position;
        for (int i = 0; i < blocks.Count; i++)
        {
            var block = blocks[number - 1 - i];
            Vector3 dr = block.transform.position - target;
            Vector3 axis = Vector3.Cross(dr, Vector3.up);
            dr.y = 0;
            Vector3 point = block.transform.position + dr / 2f;
            block.transform.DOJump(target, 1, 0, 0.2f);
            block.RotateAround(point, axis, .2f, target);
            target += Vector3.up * 0.2f;
            yield return new WaitForSeconds(.1f);
        }
    }


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
            MoToTarget();
		}
	}
}
