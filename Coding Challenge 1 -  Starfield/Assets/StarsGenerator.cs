using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsGenerator : MonoBehaviour
{
    private const int MIN_STARS_COUNT = 100;
    private const int MAX_STARS_COUNT = 1000;

    private const float MIN_SPEED = 40f;
    private const float MAX_SPEED = 100f;

    private const float MIN_STAR_SIZE = 0.01f;
    private const float MAX_STAR_SIZE = 0.3f;

    private const float MIN_STAR_LENGTH = 1f;
    private const float MAX_STAR_LENGTH = 10f;

    private const float MIN_SPAWN_DISTANCE = 20f;
    private const float MAX_SPAWN_DISTANCE = 40f;

    [Range(MIN_STARS_COUNT, MAX_STARS_COUNT)]
    public int NumberOfStars = 300;

    [Range(MIN_SPEED, MAX_SPEED)]
    public float Speed = 50f;

    [Range(MIN_STAR_SIZE, MAX_STAR_SIZE)]
    public float StarSize = 0.1f;

    [Range(MIN_STAR_LENGTH, MAX_STAR_LENGTH)]
    public float StarLength = 2f;

    public GameObject StarObject;

    private GameObject[] stars = new GameObject[MAX_STARS_COUNT];
    private Rect cameraViewportRect = new Rect(0f, 0f, 1f, 1f);

    private void Start()
    {
        for (int i = 0; i < MAX_STARS_COUNT; i++)
        {
            GameObject star = CreateStar();
            star.SetActive(i < NumberOfStars);
            stars[i] = star;
        }
    }

    private GameObject CreateStar()
    {
        GameObject star = Instantiate(StarObject, transform);
        PlaceStarInRandomPosition(star.transform);
        float startZ = Random.Range(0f, MIN_SPAWN_DISTANCE);
        Vector3 starPosition = star.transform.position;
        star.transform.position = new Vector3(starPosition.x, star.transform.position.y, startZ);
        return star;
    }

    private void Update()
    {
        ActivateNewStars();
        DeactivateUnusedStars();
        UpdateStars();
    }

    private void UpdateStars()
    {
        for (int i = 0; i < NumberOfStars; i++)
        {
            Transform starTransform = stars[i].transform;

            float deltaZ = -Speed * Time.deltaTime;
            starTransform.Translate(0f, 0f, deltaZ, Space.World);

            float speedFactor = Mathf.Clamp01((Speed - MIN_SPEED) / (MAX_SPEED - MIN_SPEED));
            float starDistanceFactor = (starTransform.position.z - Camera.main.transform.position.z) / MAX_SPAWN_DISTANCE;

            float newScaleX = StarLength * speedFactor * starDistanceFactor;
            float newScaleY = StarSize * starDistanceFactor;
            starTransform.localScale = new Vector3(newScaleX, newScaleY, starTransform.localScale.z);

            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(starTransform.position);
            bool isStarOutsideVieport = !cameraViewportRect.Contains(viewportPosition);
            if (isStarOutsideVieport)
            {
                PlaceStarInRandomPosition(starTransform);
            }
        }
    }

    private void ActivateNewStars()
    {
        for (int i = 0; i < NumberOfStars; i++)
        {
            GameObject star = stars[i];
            if (!star.activeSelf)
            {
                PlaceStarInRandomPosition(star.transform);
                star.SetActive(true);
            }
        }
    }

    private static void PlaceStarInRandomPosition(Transform starTransform)
    {
        float randomSpawnDistance = Random.Range(MIN_SPAWN_DISTANCE, MAX_SPAWN_DISTANCE);

        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, randomSpawnDistance));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, randomSpawnDistance));

        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;

        float randomX = Random.Range(-width / 2f, width / 2f);
        float randomY = Random.Range(-height / 2f, height / 2f);
        starTransform.position = new Vector3(randomX, randomY, randomSpawnDistance);
    }

    private void DeactivateUnusedStars()
    {
        for (int i = NumberOfStars; i < MAX_STARS_COUNT; i++)
        {
            GameObject star = stars[i];
            if (star.activeSelf)
            {
                star.SetActive(false);
            }
        }
    }
}
