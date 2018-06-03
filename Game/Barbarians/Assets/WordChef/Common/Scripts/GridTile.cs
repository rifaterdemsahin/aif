using UnityEngine;
using UnityEngine.UI;

public class GridTile : MonoBehaviour
{
    public GameObject tile;
    public int numTop, numBottom, numRight, numLeft;
    public Vector3 centerPosition;
    public Transform parentTransform;

    private void Start()
    {
        Sprite sprite = tile.GetComponent<Image>().sprite;
        float tileWidth = sprite.rect.width;
        float tileHeight = sprite.rect.height;

        for (int i = 0; i < numRight; i++)
        {
            for (int j = 0; j < numTop; j++)
            {
                float x = i * tileWidth + tileWidth / 2;
                float y = j * tileHeight + tileHeight / 2;

                GameObject tile1 = (GameObject)Instantiate(tile);
                GameObject tile2 = (GameObject)Instantiate(tile);
                GameObject tile3 = (GameObject)Instantiate(tile);
                GameObject tile4 = (GameObject)Instantiate(tile);

                tile1.transform.SetParent(parentTransform);
                tile2.transform.SetParent(parentTransform);
                tile3.transform.SetParent(parentTransform);
                tile4.transform.SetParent(parentTransform);

                tile1.transform.localScale = Vector3.one;
                tile2.transform.localScale = Vector3.one;
                tile3.transform.localScale = Vector3.one;
                tile4.transform.localScale = Vector3.one;

                tile1.transform.localPosition = new Vector3(x, y, 0);
                tile2.transform.localPosition = new Vector3(-x, y, 0);
                tile3.transform.localPosition = new Vector3(x, -y, 0);
                tile4.transform.localPosition = new Vector3(-x, -y, 0);
            }
        }
    }
}