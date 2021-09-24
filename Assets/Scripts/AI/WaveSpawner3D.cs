using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner3D : MonoBehaviour
{

    // Defines the state of the spawner. "SPAWNING" is when the
    // spawner is currently spawning in enemies. "WAITING" is
    // when the spawner is waiting for all the enemies to be killed
    // and "COUNTING" is when the time between waves is counting down.
    public enum SpawnState { SPAWNING, WAITING, COUNTING };


    // The different attack angles to spawn a wave in.
    // LEFT_RIGHT and RIGHT_LEFT correspond to columns of 
    // enemies, where the enemies are rotated to face the
    // center of the screen
    public enum AttackAngle { DOWN_DIAG, UP_DIAG, GAUNTLET, LEFT_RIGHT, RIGHT_LEFT };

    // Wave Object
    // I recommend a space factor of 4 for columns 
    // and gauntlets, but 3 can work for diagonals.
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public GameObject enemyPrefab;
        public int count;
        public float rate = 5f;
        public AttackAngle attackAngle;
        public float spaceFactor = 4f;
        public Transform spawnPoint;
    }

    // List of waves
    public Wave[] waves;

    // Time in between waves
    public float timeBetweenWaves = 3f;

    // Private variables
    private int nextWave = 0;
    private float waveCountDown;
    private SpawnState state = SpawnState.COUNTING;
    private float searchCountdown = 1f;

    void Start()
    {
        waveCountDown = timeBetweenWaves;

    }

    void Update()
    {

        if (state == SpawnState.WAITING) {
            // Check if enemies are still alive
            if (!EnemyIsAlive()) {
                // Begin a new round
                WaveCompleted();
                return;
            } else {
                // Don't do anything and let player kill enemies
                return;
            }

        }
        if (waveCountDown <= 0) {

            if (state != SpawnState.SPAWNING) {

                // Start spawning wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }

        } else {

            waveCountDown -= Time.deltaTime;
        }
    }

    // Logic for when a wave has been completed
    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");

        // Wave completed, so start counting the time
        // in between waves.
        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1) {
            nextWave = 0;
            // All waves done, destorying Game Object
            Debug.Log("Completed All Waves! Destroying Spawner");
            Destroy(gameObject);
        } else {
            nextWave++;
        }
        

    }

    // Checking whether a wave still has enemies alive
    bool EnemyIsAlive()
    {

        searchCountdown -= Time.deltaTime;

        if (searchCountdown <= 0f) {

            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null) {
                // No more enemies
                return false;
            }
        }
        // All enemies killed
        return true;
    }

    // Logic to spawnn waves
    IEnumerator SpawnWave(Wave _wave) {

        Debug.Log("Spawning Wave: " + _wave.name);

        state = SpawnState.SPAWNING;

        // Offset that will control which emeny to spawn
        // in the wave. 
        int enemyOffset = -(_wave.count / 2);

        // If there are an even number of enemies, 
        // there will be an offset to center enemies
        // around spawn point.
        bool evenBias;
        if (_wave.count % 2 == 0) {
            evenBias = true;
        } else {
            evenBias = false;
        }

        for (int i = 0; i < _wave.count; i++) {
            SpawnEnemy(_wave, enemyOffset, evenBias);
            enemyOffset++;
            // This spawns the enemies according the 'rate' of the wave
            if (_wave.attackAngle != AttackAngle.GAUNTLET) {
                yield return new WaitForSeconds( 1f/_wave.rate );
            }
            
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(Wave _wave, int enemyOffset, bool evenBias) {

        // Spawn Enemy
        Debug.Log("Spawning Enemy: " + _wave.enemyPrefab.name);

        // This box will be used to get the size of the enemy, and will offset
        // the enemies
        BoxCollider enemyBox = _wave.enemyPrefab.GetComponent<BoxCollider>();
        AttackAngle angle = _wave.attackAngle;

        // Offset vector that spaces out one enemy
        Vector3 _offset;
        
        float offset_x = enemyBox.size.x * _wave.spaceFactor;
        float offset_y = 0f;
        float offset_z;
        if (angle != AttackAngle.LEFT_RIGHT && angle != AttackAngle.RIGHT_LEFT) {
            offset_z = enemyBox.size.z * _wave.spaceFactor;
        } else {
            offset_z = enemyBox.size.z / _wave.spaceFactor;
        }
        

        float rotationAngle = 0f;

        // Is LEFT_RIGHT or RIGHT_LEFT
        bool left_right = false;

        if (angle != AttackAngle.LEFT_RIGHT && angle != AttackAngle.RIGHT_LEFT) {

            // If not LEFT_RIGHT or RIGHT_LEFT, then offset vector
            // needs z and x coordinates.
            _offset.x = offset_x;
            _offset.y = offset_y;

            if (angle == AttackAngle.GAUNTLET) { 
                _offset.z = 0f;
            } else if (angle == AttackAngle.DOWN_DIAG) {
                _offset.z = -offset_z;
            } else {
                _offset.z = offset_z;
            }

        } else {

            // If LEFT_RIGHT or RIGHT_LEFT, then offset vector
            // only needs z vector
            left_right = true;

            _offset.x = 0f;
            _offset.y = 0f;
            _offset.z = offset_z;

            // Specifying rotation angle depending on the attack angle
            if (angle == AttackAngle.LEFT_RIGHT) {
                rotationAngle = 90f;
            } else {
                rotationAngle = -90f;
            }
        }

        // The new position of the enemy relative to the spawnpoint and offset.
        Vector3 newPosition = _wave.spawnPoint.position - (_offset * enemyOffset);

        
        if (evenBias && !left_right) {
            // If not LEFT_RIGHT or RIGHT_LEFT, then even bias spacing only affects x coordinate
            newPosition.x -= enemyBox.size.x * (_wave.spaceFactor / 2f);
        } else if (evenBias && left_right) {
            // Else, even bias spacing only affects z coordinates
            newPosition.z -= enemyBox.size.z * (_wave.spaceFactor / 2f);
        }
        GameObject gameObject = Instantiate(_wave.enemyPrefab, newPosition, _wave.spawnPoint.rotation);

        // Rotating object in y coordinate, if applicable
        gameObject.transform.Rotate(0f, rotationAngle, 0f, Space.Self);
    }
}
