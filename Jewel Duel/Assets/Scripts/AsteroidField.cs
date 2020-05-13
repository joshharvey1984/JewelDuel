using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour {
    public static AsteroidField Instance;
    
    public GameObject asteroid;
    public List<Sprite> jewelSprites = new List<Sprite>();

    public List<Vector2> asteroidSlots;

    private void Awake() {
        Instance = GetComponent<AsteroidField>();
    }

    private void Start() {
        CreateAsteroidSlots();
        CreateAsteroidField();
    }
    
    private static bool IsOdd(int num) => num % 2 != 0;

    private void CreateAsteroidSlots() {
        var rowCount = 1;
        for (var j = -2.8F; j < 2.9F; j += 0.4F) {
            for (var i = -3.6F; i < 3.25F; i += 0.4F) {
                var slotPos = new Vector2(i, j);
                if (IsOdd(rowCount)) slotPos.x += 0.2F;
                asteroidSlots.Add(slotPos);
            }

            rowCount++;
        }
    }

    private void CreateAsteroidField() {
        var rowCount = 1;
        for (var j = -0.4F; j < 0.5F; j += 0.4F) {
            for (var i = -3.6F; i < 3.25F; i += 0.4F) {
                var asteroidPos = new Vector2(i, j);
                if (IsOdd(rowCount)) asteroidPos.x += 0.2F;
                var ast = Instantiate(asteroid, asteroidPos, Quaternion.identity);
                ast.GetComponent<Asteroid>().CreateAsteroid();
            }
            rowCount++;
        }
    }

    public void ScoreJewels(List<Asteroid> asteroids, Gun gun) {
        foreach (var ast in asteroids) {
            Asteroid.All.Remove(ast);
            Destroy(ast.gameObject);
        }
    }
}
