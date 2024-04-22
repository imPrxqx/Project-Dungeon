using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGeneratorScript : MonoBehaviour
{

    //wall vars
    public GameObject wall; 
    public GameObject roofPlane;
    public float WallgenNum = 0.5f;
    public int wallHeight = 8;

    //terrain vars
    public int xSize = 50, zSize = 50;
    public float genNum = 0.12f; 
    public float maxTerrainHeight = 3;
    public float offsetNum = 0;

    //enviroment vars
    public GameObject[] grass; 
    public GameObject[] rocks;
    public GameObject[] trees;
    public int maxParticles = 200;

    //mesh generation
    Mesh terrain;
    Mesh walls;
    Vector3[] Vertices; 
    Vector3[] Vertices2;
    int[] Triangles;
    int[] Triangles2;

    //enemy generation
    public GameObject[] enemys;
    public int enemyCount = 50;

    //level exit
    public GameObject exit;


    //temp
    float newNum;

    // Start is called before the first frame update
    void Start()
    {
        //generating the map
        offsetNum = ((float) Random.Range(1,99)) / 100;
        newNum = offsetNum;

        UpdateTerrain();
        UpdateShape();
        GenerateEnviroment();
        SpawnEnemys();


        //spawns the exit somewhere random on the map
        while (true)
        {
            //choose a random position
            float xPos = Random.Range(0, xSize);
            float zPos = Random.Range(0, zSize);

            RaycastHit hit = new RaycastHit();
            Vector3 origin = new Vector3(xPos, maxTerrainHeight + 3, zPos);
            Vector3 direction = new Vector3(0, -1, 0);
            Physics.Raycast(origin, direction, out hit);

            //if the position is clear spawns the exit, otherwise try again
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                Instantiate(exit, hit.point, enemys[0].transform.rotation);
                break;
            }
        }

    }

    // Update is called once per frame
    void Update()
    { 
        //if the offset number is changed, regenerete the terrain
        if(newNum != offsetNum)
        {
            UpdateTerrain();
            newNum = offsetNum;
        }
    }

    //randomly spawns enemies to the map
    private void SpawnEnemys()
    {
        //for each enemy to spawn
        for (int k = 0; k < enemyCount / 2 - 4; )
        {
            //choose a random position
            float xPos = Random.Range(0, xSize);
            float zPos = Random.Range(0, zSize);

            RaycastHit hit = new RaycastHit();
            Vector3 origin = new Vector3(xPos, maxTerrainHeight + 3, zPos);
            Vector3 direction = new Vector3(0, -1, 0);
            Physics.Raycast(origin, direction, out hit);

            //if the position is clear spawns the enemy, otherwise try again
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                GameObject newEnemy = Instantiate(enemys[0], hit.point, enemys[0].transform.rotation);
                newEnemy.transform.Rotate(new Vector3(0, 1, 0), Random.Range(0, 360), Space.World);
                k++;
            }
        }
    }

    //spawns trees and rocks across the map
    private void GenerateEnviroment()
    {
        //generate the grass
        for (int i = 0; i < xSize/2 - 4; i++)
        {
            for (int k = 0; k < zSize/2 - 4; k++)
            {
                if (Mathf.PerlinNoise(i * genNum * 5 + offsetNum * 1.5f, k * genNum * 5 + offsetNum * 1.5f) > 0.55) {

                    RaycastHit hit = new RaycastHit();
                    Vector3 origin = new Vector3((i + 2) * 2, maxTerrainHeight + 3, (k + 2) * 2);
                    Vector3 direction = new Vector3(0, -1, 0);
                    Physics.Raycast(origin, direction, out hit);

                    if (hit.collider.gameObject.CompareTag("Ground"))
                    {
                        GameObject newGrass = Instantiate(grass[0], hit.point, grass[0].transform.rotation);
                        newGrass.transform.Rotate(new Vector3(0, 1, 0), Random.Range(0, 360), Space.World);
                    }
                }
            }
        }

        //spawn the rocks and trees
        for (int i = 0; i < 50; i++)
        {
            //choose a random position
            float x = Random.Range(20, xSize * 10 - 20) / 10f;
            float z = Random.Range(20, zSize * 10 - 20) / 10f;

            RaycastHit hit = new RaycastHit();
            Vector3 origin = new Vector3(x, maxTerrainHeight + 1, z);
            Vector3 direction = new Vector3(0, -1, 0);
            Physics.Raycast(origin, direction, out hit);

            //if the position is clear spawn either tree or rock, otherwise try again
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                GameObject objToSpawn = null;
                switch (Random.Range(0, 2))
                {
                    case 0:
                        objToSpawn = rocks[Random.Range(0, rocks.Length - 1)];
                        break;
                    case 1:
                        objToSpawn = trees[Random.Range(0, trees.Length - 1)];
                        break;
                }

                //spawn obcejt and randomly rotate it
                GameObject newObj = Instantiate(objToSpawn, hit.point, objToSpawn.transform.rotation);
                newObj.transform.Rotate(new Vector3(0, 1, 0), Random.Range(0, 360), Space.World);
            }
        }
    }

    //generates the ground
    void UpdateTerrain() {
        Vertices = new Vector3[xSize * zSize];
        //float maxH = 0, minH = 0;

        for (int i = 0; i < xSize; i++)
        {
            for (int k = 0; k < zSize; k++)
            {
                float h1 = Mathf.PerlinNoise(i * genNum + offsetNum, k * genNum + offsetNum) * maxTerrainHeight;
                float h2 = Mathf.PerlinNoise(i * genNum * 5 + offsetNum, k * genNum * 5 + offsetNum) * maxTerrainHeight / 4;
                float height = h1 + h2;

                /*if (height < minH)
                    minH = height;

                if (height > maxH)
                    maxH = height;*/

                Vertices[i * xSize + k] = new Vector3(i, height, k);
            }
        }

        /*colors = new Color[Vertices.Length];
        for (int i = 0; i < xSize; i++)
        {
            for (int k = 0; k < zSize; k++)
            {
                float height = Mathf.InverseLerp(minH, maxH, Vertices[i * xSize + k].y);
                colors[i * xSize + k] = terrainColor.Evaluate(height);
            }
        }*/

        Triangles = new int[xSize * zSize * 6];
        for (int i = 0; i < Vertices.Length - xSize - 1; i++)
        {
            Triangles[0 + i * 6] = 0 + i;
            Triangles[1 + i * 6] = 1 + i;
            Triangles[2 + i * 6] = 0 + i + xSize;

            Triangles[3 + i * 6] = 1 + i + xSize;
            Triangles[4 + i * 6] = 0 + i + xSize;
            Triangles[5 + i * 6] = 1 + i;

            if ((i + 2) % xSize == 0)
            {
                i++;
            }
        }

        terrain = new Mesh();
        terrain.Clear();
        terrain.vertices = Vertices;
        terrain.triangles = Triangles;
        //terrain.colors = colors;
        terrain.RecalculateNormals();

        gameObject.GetComponent<MeshFilter>().mesh = terrain;
        gameObject.GetComponent<MeshCollider>().sharedMesh = terrain;
    }

    //generates the walls
    void UpdateShape()
    {
        Vertices2 = new Vector3[(xSize + zSize) * wallHeight];

        for (int i = 0; i < wallHeight; i++)
        {
            for (int k = 0; k < zSize + xSize; k++)
            {
                if (k > xSize + zSize/2)
                {
                    float x = xSize - (k * 2 - (2 * xSize + zSize));
                    float z = Mathf.PerlinNoise(i * WallgenNum + offsetNum, k * WallgenNum + offsetNum) * 5;
                    Vertices2[i * (xSize + zSize) + k] = new Vector3(x, i * 3, z);
                }
                else if (k > xSize/2 + zSize/2)
                {
                    float x = Mathf.PerlinNoise(i * WallgenNum + offsetNum, k * WallgenNum + offsetNum) * 5 + xSize - 4;
                    float z = zSize - (k * 2 - (xSize + zSize));
                    Vertices2[i * (xSize + zSize) + k] = new Vector3(x, i * 3, z);
                }
                else if (k > xSize/2)
                {
                    float x = k * 2 - xSize;
                    float z = Mathf.PerlinNoise(i * WallgenNum + offsetNum, k * WallgenNum + offsetNum) * 5 + zSize - 4;
                    Vertices2[i * (xSize + zSize) + k] = new Vector3(x, i * 3, z);
                }
                else
                {
                    float x = Mathf.PerlinNoise(i * WallgenNum + offsetNum, k * WallgenNum + offsetNum) * 5;
                    float z = k * 2;
                    Vertices2[i * (xSize + zSize) + k] = new Vector3(x, i * 3, z);
                } 
            }
        }

        Triangles2 = new int[(xSize + zSize) * wallHeight * 6];
        for (int i = 0; i < Vertices2.Length - (zSize + xSize) - 1; i++)
        {
            Triangles2[0 + i * 6] = 0 + i;
            Triangles2[1 + i * 6] = 0 + i + (zSize + xSize);
            Triangles2[2 + i * 6] = 1 + i;

            Triangles2[3 + i * 6] = 1 + i + (zSize + xSize);
            Triangles2[4 + i * 6] = 1 + i;
            Triangles2[5 + i * 6] = 0 + i + (zSize + xSize);

            if ((i + 2) % (zSize + xSize) == 0)
            {
                i++;
            }
        }

        roofPlane.transform.position = new Vector3(xSize/2, wallHeight * 2.5f, zSize/2);
        roofPlane.transform.localScale = new Vector3(xSize / 8, 1, zSize / 8);

        walls = new Mesh();
        walls.Clear();
        walls.vertices = Vertices2;
        walls.triangles = Triangles2;
        walls.RecalculateNormals();

        wall.GetComponent<MeshFilter>().mesh = walls;
        wall.GetComponentInChildren<MeshCollider>().sharedMesh = walls;
    }
}
