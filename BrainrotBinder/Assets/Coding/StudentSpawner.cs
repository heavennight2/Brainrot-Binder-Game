using UnityEngine;

public class StudentSpawner : MonoBehaviour
{
    public GameObject studentPrefab;
    public Transform player;

    public float minimumSpawnTime = 3f;
    public float maximumSpawnTime = 6f;

    public int maximumActiveStudents = 5;

    private float timeUntilSpawn;

    private void Awake()
    {
        SetTimeUntilSpawn();
    }

    private void Update()
    {
        timeUntilSpawn -= Time.deltaTime;

        if (timeUntilSpawn <= 0)
        {
            int activeStudents =
                FindObjectsByType<StudentFollow>(
                    FindObjectsSortMode.None
                ).Length;

            if (activeStudents < maximumActiveStudents)
            {
                SpawnStudent();
            }

            SetTimeUntilSpawn();
        }
    }

    private void SpawnStudent()
    {
        GameObject newStudent = Instantiate(
            studentPrefab,
            transform.position,
            Quaternion.identity
        );

        StudentFollow studentFollow =
            newStudent.GetComponent<StudentFollow>();

        studentFollow.player = player;
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(
            minimumSpawnTime,
            maximumSpawnTime
        );
    }
}