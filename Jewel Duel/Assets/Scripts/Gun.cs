using UnityEngine;

public class Gun : MonoBehaviour {

    private Transform _transform;

    private const int RotSpeed = 30;
    [SerializeField] private GameObject asteroid;

    private Asteroid _gunAsteroid;

    private void Awake() {
        _transform = transform;
    }

    private void Start() {
        LoadAsteroid();
    }

    public void LoadAsteroid() {
        var ast = Instantiate(asteroid, transform.position, Quaternion.identity, _transform);
        _gunAsteroid = ast.GetComponent<Asteroid>();
        _gunAsteroid.CreateAsteroid(this);
        _gunAsteroid.transform.rotation = transform.rotation;
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) && transform.rotation.z > -0.40F) {
            transform.Rotate(new Vector3 (0, 0, -1) * (RotSpeed * Time.deltaTime));
            
        }
        if (Input.GetKey(KeyCode.LeftArrow) && transform.rotation.z < 0.40F) {
            transform.Rotate(new Vector3 (0, 0, 1) * (RotSpeed * Time.deltaTime));
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && _gunAsteroid != null) {
            _gunAsteroid.Fire();
            _gunAsteroid.transform.parent = null;
            _gunAsteroid = null;
        }
    }
}
