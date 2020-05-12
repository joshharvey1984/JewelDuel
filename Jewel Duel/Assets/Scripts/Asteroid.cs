using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Jewel;

public class Asteroid {
    public static List<Asteroid> All = new List<Asteroid>();
    
    private readonly GameObject _asteroid;
    public JewelColour JewelCol;
    public GameObject Jewel;

    public Asteroid(GameObject asteroid) {
        _asteroid = asteroid;
        Jewel = _asteroid.transform.GetChild(0).gameObject;
        ColourCalc();
        Jewel.GetComponent<SpriteRenderer>().sprite = AsteroidField.Instance.jewelSprites[JewelCol.GetHashCode()];
        All.Add(this);
    }
    
    private void ColourCalc() {
        int colourCount;
        JewelColour checkColour;
        do {
            colourCount = 0;
            checkColour = (JewelColour) Random.Range(0, 3);
            foreach (var localAsteroid in FindLocalAsteroids()) {
                if (localAsteroid.JewelCol == checkColour && localAsteroid.IsPaired()) colourCount = 2;
            }
        } while (colourCount > 1);

        JewelCol = checkColour;
    }

    private List<Asteroid> FindLocalAsteroids() => All.Where(ast => Vector2.Distance
            (_asteroid.transform.position, ast._asteroid.transform.position) < 0.5F).ToList();

    private bool IsPaired() => FindLocalAsteroids().Any(localAsteroid => localAsteroid.JewelCol == JewelCol);
    
}
