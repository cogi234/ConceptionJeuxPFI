using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BehaviourTreeColin;

public class Boss1Controller : MonoBehaviour
{
    [Header("Phases")]
    [SerializeField] GameObject[] bodyPrefabs;
    [SerializeField] float phaseTransitionTime;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionForce;
    [SerializeField] float explosionUpModifier;
    [SerializeField] float coreTransitionTime;
    [Header("Cube Spawning")]
    [SerializeField] GameObject cubePrefab;
    [SerializeField] float cubesPerSecond;
    [SerializeField] float cubeTransitionTime;
    [SerializeField] float maxCubeSpawnRadius;
    [Header("Health")]
    [SerializeField] int maxHealth = 100;
    int health;

    Node[] behaviourTrees;
    Dictionary<string, object> data = new Dictionary<string, object>();
    Transform player;
    Transform core;
    Transform coreTarget;
    Transform[] targets;
    List<CubeController> cubes;
    List<CubeController> inactiveCubes;
    float cubeTime;
    float cubeTimer;
    int currentPhase = -1;
    public bool PlayerOnMe
    {
        get
        {
            return (bool)data["playerOnBoss"];
        }
        set
        {
            data["playerOnBoss"] = value;
        }
    }

    private void Awake()
    {
        behaviourTrees = new Node[]
        {
            BehaviourTreeCreator.GetBoss1()
        };

        core = GameObject.Find("Core").transform;
        core.GetComponent<DamageableComponent>().onDamage.AddListener(TakeDamage);
        core.GetComponent<DamageableComponent>().enabled = false;
        core.GetComponent<InteractableComponent>().onInteract.AddListener(StartFight);

        data["playerTransform"] = GameObject.FindWithTag("Player").transform;
        data["bossController"] = this;

        cubes = new List<CubeController>();
        inactiveCubes = new List<CubeController>();
        cubeTime = 1 / cubesPerSecond;

        targets = GameObject.FindGameObjectsWithTag("target").Select((GameObject g, int index) => { return g.transform; }).ToArray();
    }

    private void Update()
    {

        //Cube spawning
        if (cubes.Count < targets.Length)
        {
            cubeTimer += Time.deltaTime;
            while (cubeTimer >= cubeTime)
            {
                cubeTimer -= cubeTime;
                SpawnCube();
            }
        }
        //If we have too many cubes, we deactivate them
        while (cubes.Count > targets.Length)
        {
            DeactivateCube();
        }

        //Evaluating behaviour tree
        if (currentPhase >= 0 && currentPhase < behaviourTrees.Length)
            behaviourTrees[currentPhase].Evaluate(data);
    }

    public void StartFight()
    {
        StartCoroutine(PhaseTransition());
        Destroy(core.GetComponent<InteractableComponent>());
    }

    private IEnumerator PhaseTransition()
    {
        //Deactivate every cube
        while (cubes.Count > 0)
        {
            DeactivateCube();
        }
        if (transform.childCount > 0)
        {
            //Explosion
            Collider[] hits = Physics.OverlapSphere(transform.GetChild(0).position, explosionForce);
            foreach (Collider hit in hits)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.AddExplosionForce(explosionForce, transform.GetChild(0).position, explosionRadius, explosionUpModifier);
            }
            //Destroy the body
            Destroy(transform.GetChild(0).gameObject);
        }
        currentPhase++;
        //Disable damage
        core.GetComponent<DamageableComponent>().enabled = false;

        yield return new WaitForSeconds(phaseTransitionTime);

        if (currentPhase >= bodyPrefabs.Length)
            OnFinalDeath();
        else
        {
            CreateBody();
            //We enable taking damage and heal to max
            core.GetComponent<DamageableComponent>().enabled = true;
            health = maxHealth;
        }
    }

    private void CreateBody()
    {
        GameObject body = Instantiate(bodyPrefabs[currentPhase], transform);
        data["body"] = body;

        //We find every target in the new body
        List<Transform> targetList = new List<Transform>();
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            if (t.gameObject.name.StartsWith("target"))
                targetList.Add(t);
            else if (t.gameObject.name == "Core Target")
                coreTarget = t;
        }
        targets = targetList.ToArray();

        core.GetComponent<CoreMovement>().target = coreTarget;
        core.GetComponent<CoreMovement>().currentAction = CoreMovement.CoreAction.Transition;
        core.GetComponent<CoreMovement>().transitionTime = coreTransitionTime;
    }

    private void SpawnCube()
    {
        if (inactiveCubes.Count > 0)
        {
            CubeController cube = inactiveCubes[inactiveCubes.Count - 1];
            cube.currentAction = CubeController.CubeAction.Transition;
            cube.transitionTime = cubeTransitionTime;
            cube.target = targets[cubes.Count - 1];

            cubes.Add(cube);
        }
        else
        {
            Vector3 cubePos = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(0, -2, Random.Range(0, maxCubeSpawnRadius));
            GameObject cube = Instantiate(cubePrefab, cubePos, Quaternion.identity);
            CubeController controller = cube.GetComponent<CubeController>();
            controller.currentAction = CubeController.CubeAction.Transition;
            controller.transitionTime = cubeTransitionTime;
            controller.target = targets[cubes.Count];

            cubes.Add(controller);
        }
    }
    private void DeactivateCube()
    {
        CubeController cube = cubes[cubes.Count - 1];

        cube.currentAction = CubeController.CubeAction.Nothing;
        cube.target = null;

        cubes.RemoveAt(cubes.Count - 1);
        inactiveCubes.Add(cube);
    }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;

            if (health <= 0)
                OnDeath();
        }
    }

    private void OnDeath()
    {
        StartCoroutine(PhaseTransition());
    }

    private void OnFinalDeath()
    {
        //TODO
    }
}
