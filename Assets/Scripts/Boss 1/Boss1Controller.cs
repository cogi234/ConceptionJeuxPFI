using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    [Header("Cube Spawning")]
    [SerializeField] GameObject cubePrefab;
    [SerializeField] float cubesPerSecond;
    [SerializeField] float cubeTransitionTime;
    [SerializeField] float maxCubeSpawnRadius;

    Transform[] targets;
    CubeController[] cubes;

    float cubeTime;
    float cubeTimer;

    private void Awake()
    {
        targets = GameObject.FindGameObjectsWithTag("target").Select((GameObject g, int index) => { return g.transform; }).ToArray();
        cubes = new CubeController[targets.Length];
        Debug.Log($"{targets.Length} cubes");

        cubeTime = 1 / cubesPerSecond;
    }

    private void Update()
    {
        //Cube spawning
        if (cubes.Count((CubeController a) => a == null) > 0)
        {
            cubeTimer += Time.deltaTime;
            while (cubeTimer >= cubeTime)
            {
                cubeTimer -= cubeTime;
                SpawnCube();
            }
        }
    }

    private void SpawnCube()
    {
        Vector3 cubePos = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(0, -2, Random.Range(0, maxCubeSpawnRadius));
        GameObject cube = Instantiate(cubePrefab, cubePos, Quaternion.identity);
        CubeController controller = cube.GetComponent<CubeController>();
        controller.currentAction = CubeController.CubeAction.Transition;
        controller.transitionTime = cubeTransitionTime;

        for (int i = 0; i < cubes.Length; i++)
        {
            if (cubes[i] == null)
            {
                cubes[i] = controller;
                controller.target = targets[i];
                break;
            }
        }
    }
}
