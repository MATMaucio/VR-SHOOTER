using UnityEngine;

public class ScrollTerrainTexture : MonoBehaviour
{
    public Terrain terrain;
    public int textureIndex = 0; // Índice de la textura en el terreno que quieres animar
    public Vector2 scrollSpeed = new Vector2(0.1f, 0.1f); // Velocidad de desplazamiento en X e Y

    private Vector2 offset = Vector2.zero;
    private TerrainData terrainData;
    private float[,,] originalAlphaMap;

    void Start()
    {
        if (terrain == null)
        {
            terrain = GetComponent<Terrain>();
            if (terrain == null)
            {
                Debug.LogError("No se encontró un componente Terrain adjunto o asignado.");
                return;
            }
        }

        terrainData = terrain.terrainData;
        
        // Guardar el alpha map original para restaurarlo si es necesario
        originalAlphaMap = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
    }

    void Update()
    {
        if (terrainData == null) return;

        // Calcular el nuevo offset
        offset += scrollSpeed * Time.deltaTime;

        // Aplicar el offset a todas las texturas del terreno
        TerrainLayer[] terrainLayers = terrainData.terrainLayers;
        
        if (textureIndex >= 0 && textureIndex < terrainLayers.Length)
        {
            TerrainLayer layer = terrainLayers[textureIndex];
            if (layer != null)
            {
                layer.tileOffset = offset;
            }
        }
        
        // Alternativa: aplicar a todas las texturas
        /*
        foreach (TerrainLayer layer in terrainLayers)
        {
            layer.tileOffset = offset;
        }
        */
    }

    void OnDisable()
    {
        // Opcional: restaurar el alpha map original cuando se desactiva el script
        if (terrainData != null && originalAlphaMap != null)
        {
            terrainData.SetAlphamaps(0, 0, originalAlphaMap);
        }
    }
}