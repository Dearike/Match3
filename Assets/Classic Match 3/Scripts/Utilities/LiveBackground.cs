using UnityEngine;
using System.Collections;

public class LiveBackground : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadTiles());
    }

    private IEnumerator LoadTiles()
    {
        GameObject tile = Resources.Load<GameObject>("Prefabs/Background Tile");
        tile.transform.localScale = new Vector3(2, 2, 2);

        float posY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y + tile.transform.localScale.x;
        float screenLeftBound = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenRightBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        while (true)
        {
            Vector3 pos = new Vector3(Random.Range(screenLeftBound, screenRightBound), posY, 0);
            GameObject shape = Instantiate(tile, pos, Quaternion.identity);
            shape.transform.SetParent(transform);
            shape.transform.localPosition = pos;
            shape.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Graphics/Shapes/" + Random.Range(1, 7));
            Destroy(shape, 5);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
