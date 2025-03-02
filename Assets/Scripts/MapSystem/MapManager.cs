using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapSystem
{
    public class MapManager : MonoBehaviour
    {
        //Spawning Units.
        [SerializeField] public Tilemap allySpawners;
        [SerializeField] public Tilemap enemySpawners;
        public List<Vector3> allySpawns;
        public List<Vector3> enemySpawns;
        [SerializeField] private Unit unitPrefab;
        [SerializeField] private Unit enemyPrefab;

        private Vector3 center = new Vector3(0.5f , 0.5f , 0);
    
        //Tiles logic system.
    
        [SerializeField] public Tilemap map;
        [SerializeField] public List<TileData> tileDatas;

        public Dictionary<TileBase, TileData> dataFromTiles;

        private void Awake()
        {
            dataFromTiles = new Dictionary<TileBase, TileData>();
            foreach (var tileData in tileDatas)
            {
                foreach (var tile in tileData.tiles)
                {
                    dataFromTiles.Add(tile, tileData);
                }
            }
        }
    
        private void Start()
        {
            allySpawners = GetComponent<Tilemap>();
            enemySpawners = GetComponent<Tilemap>();
            allySpawns = new List<Vector3>();
            enemySpawns = new List<Vector3>();
        }

        public void GetSpawners(Tilemap spawn, List<Vector3> spawners)
        {
            for (int n = spawn.cellBounds.xMin; n < spawn.cellBounds.xMax; n++)
            {
                for (int p = spawn.cellBounds.yMin; p < spawn.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = (new Vector3Int(n, p, (int) spawn.transform.position.y));
                    Vector3 place = spawn.CellToWorld(localPlace);
                    if (spawn.HasTile(localPlace))
                    {
                        //Tile at "place"
                        spawners.Add(place);
                    }
                    else
                    {
                        //No tile at "place"
                    }
                }
            }
        }

        public void SpawnUnit()
        {
            GetSpawners(allySpawners, allySpawns);
            for(var i = 0; i < allySpawns.Count; i++)
            {
                Vector3Int positions = new  Vector3Int((int)allySpawns[i].x ,(int)allySpawns[i].y, 0);
                Instantiate(unitPrefab, allySpawns[i] + center, Quaternion.identity);
                TileBase occupiedTile = allySpawners.GetTile(positions);
                dataFromTiles[occupiedTile].isOccupied = true;
            }
        }

        public void SpawnEnemies()
        {
            GetSpawners(enemySpawners, enemySpawns);
            for(var i = 0; i < enemySpawns.Count; i++)
            {
                Vector3Int positions = new  Vector3Int((int)enemySpawns[i].x ,(int)enemySpawns[i].y, 0);
                Instantiate(enemyPrefab, enemySpawns[i] + center, Quaternion.identity);
                TileBase occupiedTile = enemySpawners.GetTile(positions);
                dataFromTiles[occupiedTile].isOccupied = true;
            }
        }
    }
}
