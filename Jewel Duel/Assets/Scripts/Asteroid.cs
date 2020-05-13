using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Jewel;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour {
    public static List<Asteroid> All = new List<Asteroid>();
    
    public JewelColour jewelCol;
    public GameObject jewel;

    private Transform _transform;
    private Collider2D _collider;
    private bool _firing = false;

    private Gun _gun;

    private void Awake() {
        _transform = transform;
        _collider = GetComponent<CircleCollider2D>();
    }

    public void CreateAsteroid(Gun gunFired = null) {
        jewel = gameObject.transform.GetChild(0).gameObject;
        if (gunFired == null) ColourCalc();
        else {
            jewelCol = RandomColour();
            _collider.isTrigger = true;
            _gun = gunFired;
        }
        jewel.GetComponent<SpriteRenderer>().sprite = AsteroidField.Instance.jewelSprites[jewelCol.GetHashCode()];
        All.Add(this);
    }
    
    private void ColourCalc() {
        bool colourCheck;
        JewelColour checkColour;
        do {
            checkColour = RandomColour();
            colourCheck = FindLocalAsteroids()
                .Any(localAsteroid => localAsteroid.jewelCol == checkColour && localAsteroid.FindMatchedAsteroids().Count > 0);
        } while (colourCheck);
        
        jewelCol = checkColour;
    }

    private List<Asteroid> FindLocalAsteroids() => All
        .Where(ast => Vector2.Distance(gameObject.transform.position, ast.gameObject.transform.position) < 0.5F)
        .Where(ast => ast != this)
        .ToList();

    private List<Asteroid> FindMatchedAsteroids() => FindLocalAsteroids()
        .Where(ast => ast.jewelCol == jewelCol)
        .ToList();

    private static JewelColour RandomColour() => (JewelColour)Random.Range(0, AsteroidField.Instance.jewelSprites.Count);

    public void Fire() {
        _firing = true;
    }

    private void FixedUpdate() {
        if (_firing) {
            var pos = _transform.position;
            pos += transform.up * (Time.deltaTime * 7);
            _transform.position = pos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (_firing) {
            _firing = false;
            _gun.LoadAsteroid();
            _collider.isTrigger = false;
            _transform.position = NearestEmptySlot();
            if (CheckMatch() != null && CheckMatch().Count > 1) {
                AsteroidField.Instance.ScoreJewels(CheckMatch(), _gun);
            }
        }
    }

    private Vector2 NearestEmptySlot() {
        Vector2 closest = new Vector2();
        foreach (var slot in AsteroidField.Instance.asteroidSlots) {
            if (Vector2.Distance(_transform.position, slot) < Vector2.Distance(_transform.position, closest))
                closest = slot;
        }

        return closest;
    }

    private List<Asteroid> CheckMatch() {
        var asteroidChecks = FindMatchedAsteroids();
        asteroidChecks.Add(this);
        foreach (var localAsteroid in asteroidChecks) {
            if (localAsteroid.FindMatchedAsteroids().Count <= 1) continue;
            var returnList = localAsteroid.FindMatchedAsteroids();
            returnList.Add(localAsteroid);
            return returnList;
        }

        return null;
    }
}
