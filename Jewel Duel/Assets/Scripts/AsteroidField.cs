using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour {
    public static AsteroidField Instance;
    
    public GameObject asteroid;
    public List<Sprite> jewelSprites = new List<Sprite>();

    private void Start() {
        Instance = GetComponent<AsteroidField>();
        CreateAsteroidField();
    }

    private void CreateAsteroidField() {
        var rowCount = 1;
        for (var j = -0.4F; j < 0.5F; j += 0.4F) {
            for (var i = -3.6F; i < 3.25F; i += 0.4F) {
                var asteroidPos = new Vector2(i, j);
                if (IsOdd(rowCount)) asteroidPos.x += 0.2F;
                var ast = Instantiate(asteroid, asteroidPos, Quaternion.identity);
                var astInst= new Asteroid(ast);
            }
            rowCount++;
        }
    }
    
    private static bool IsOdd(int num) => num % 2 != 0;
}
