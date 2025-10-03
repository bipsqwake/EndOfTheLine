using System;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float levelK;

    private Transform[][] levels;
    private float[] levelsPath;
    private float[] levelsLength;

    public void Awake()
    {
        int levelsCount = transform.childCount;
        levels = new Transform[levelsCount][];
        levelsPath = new float[levelsCount];
        levelsLength = new float[levelsCount];
        for (int i = 0; i < levelsCount; i++)
        {
            SpriteRenderer levelSpriteRenderer = transform.GetChild(i).GetComponent<SpriteRenderer>();
            if (levelSpriteRenderer == null)
            {
                throw new ArgumentException("All children of background should have sprite renderer");
            }
            levelsPath[i] = 0.0f;
            levelsLength[i] = levelSpriteRenderer.size.x;
            levels[i] = new Transform[2];
            levels[i][0] = transform.GetChild(i);
            levels[i][1] = Instantiate(transform.GetChild(i),
                                        transform.GetChild(i).position + new Vector3(levelsLength[i], 0, 0),
                                        transform.GetChild(i).rotation,
                                        transform);
            levels[i][1].name = levels[i][0].name;
        }
    }

    public void Update()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            float path = speed * Mathf.Pow(levelK, i) * Time.deltaTime;
            levelsPath[i] += path;
            for (int j = 0; j < levels[i].Length; j++)
            {
                levels[i][j].position += Vector3.left * path;
            }
            if (levelsPath[i] > levelsLength[i])
            {
                Destroy(levels[i][0].gameObject);
                levels[i][0] = levels[i][1];
                levels[i][1] = Instantiate(levels[i][0],
                                        levels[i][0].position + new Vector3(levelsLength[i], 0, 0),
                                        levels[i][0].rotation,
                                        transform);
                levels[i][1].name = levels[i][0].name;
                levelsPath[i] -= levelsLength[i];
            }
        }
    }
}
