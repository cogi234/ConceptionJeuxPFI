using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeColin;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss1Controller : MonoBehaviour
{
    [Header("Spawning and death")]
    [SerializeField] GameObject bodyPrefab;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionForce;
    [SerializeField] float explosionUpModifier;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float coreTransitionTime;
    [Header("Cube Spawning")]
    [SerializeField] GameObject cubePrefab;
    [SerializeField] float cubesPerSecond;
    [SerializeField] float cubeTransitionTime;
    [SerializeField] float maxCubeSpawnRadius;
    [Header("Health")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthBar;
    int health;
    [Header("Movement")]
    public float rotationSpeed = 45f;
    public float movementSpeed;
    public float jumpMaxDistance;
    public float jumpMinDistance;

    Node behaviourTree;
    Dictionary<string, object> data = new Dictionary<string, object>();
    Transform player;
    Transform core;
    Transform coreTarget;
    Transform[] targets = new Transform[0];
    List<CubeController> cubes;
    List<CubeController> inactiveCubes;
    float cubeTime;
    float cubeTimer;
    bool processBrain = false;

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
        health = maxHealth;
        healthBar.value = health;

        player = GameObject.FindWithTag("Player").transform;

        core = GameObject.Find("Core").transform;
        core.GetComponent<DamageableComponent>().onDamage.AddListener(TakeDamage);
        core.GetComponent<DamageableComponent>().enabled = false;
        core.GetComponent<InteractableComponent>().onInteract.AddListener(StartFight);

        //Set behaviour tree data
        behaviourTree = BehaviourTreeCreator.GetBoss1();
        data["playerTransform"] = player;
        data["bossController"] = this;
        data["bossTransform"] = transform;
        data["canShoot"] = true;
        data["currentAttack"] = "";
        data["attackCooldown"] = 5f;
        PlayerOnMe = false;

        cubes = new List<CubeController>();
        inactiveCubes = new List<CubeController>();
        cubeTime = 1 / cubesPerSecond;
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
        data["playerDistance"] = Vector3.Distance(transform.position, player.position);
        if (processBrain)
            behaviourTree.Evaluate(data);
    }

    private void LateUpdate()
    {
        //Fix position/rotation fuckery
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public void StartFight()
    {
        StartCoroutine(Introduction());
        core.GetComponent<InteractableComponent>().enabled = false;
        healthBar.gameObject.SetActive(true);
    }

    private IEnumerator Introduction()
    {
        yield return new WaitForSeconds(2);
        CreateBody();
        //We enable taking damage
        core.GetComponent<DamageableComponent>().enabled = true;
        yield return new WaitForSeconds(6);
        processBrain = true;
    }

    private void CreateBody()
    {
        GameObject body = Instantiate(bodyPrefab, transform);
        data["body"] = body.transform;

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

        data["coreTarget"] = coreTarget;

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
            cube.target = targets[cubes.Count];

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
            health = Mathf.Max(health, 0);
            healthBar.value = health;

            if (health <= 0)
                StartCoroutine(OnDeath());
        }
    }

    private IEnumerator OnDeath()
    {
        processBrain = false;
        //Deactivate every cube
        while (cubes.Count > 0)
        {
            DeactivateCube();
        }
        if (transform.childCount > 0)
        {
            //Explosion
            Explosion explosion = Instantiate(explosionPrefab, transform.GetChild(0).position, transform.GetChild(0).rotation).GetComponent<Explosion>();
            explosion.force = explosionForce;
            explosion.radius = explosionRadius;
            explosion.upModifier = explosionUpModifier;
            //Destroy the body
            Destroy(transform.GetChild(0).gameObject);
        }
        //Disable damage
        core.GetComponent<CoreMovement>().target = null;
        core.GetComponent<CoreMovement>().currentAction = CoreMovement.CoreAction.Nothing;
        core.GetComponent<DamageableComponent>().enabled = false;

        //GameObject.Find("FadeToBlack").SetActive(true);

        //Destroy(gameObject);

        yield return new WaitForSeconds(5);

        //Ici, on load la scene du boss 2
        GameObject.Find("SaveManager").GetComponent<Save>().scene1a2();

        SceneManager.LoadScene(3);
    }
}
