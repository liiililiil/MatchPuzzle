using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct StagePlacement{
    public Vector2Byte mapSize;
    public Vector2Byte[] blockPos;
    public Vector2Byte[] boxPos;

}

[System.Serializable]
public struct StageQuest{
    public tileType targetTileType;
    public byte targetBlastCount;
    public byte timeLimit;
    public byte movementLimit;

}

public class StageManager : ManagerMonoBehaviour
{
    private const string STAGE_DATA_PATH = "StageData";
    private StageQuest stageQuest;
    private Tile[,] tileMap;

    private void Awake() {
        Initialize(PlayerPrefs.GetInt("StageIndex", -1));
    }

    private void Initialize(int stageIndex){

        try{
            if(stageIndex == -1) throw new Exception("PlayerPrefs has no value.");

            string dirPath = Path.Combine(Application.streamingAssetsPath, STAGE_DATA_PATH);
            
            string filePath = Path.Combine(dirPath, $"{stageIndex}Placement.json");
            string json = File.ReadAllText(filePath);
            InitStagePlacement(JsonUtility.FromJson<StagePlacement>(json));

            filePath = Path.Combine(dirPath, $"{stageIndex}Quest.json");
            json = File.ReadAllText(filePath);
            InitStageQuest(JsonUtility.FromJson<StageQuest>(json));

        } catch(Exception e){
            Debug.LogError($"StageManager: Failed to load stage data from {stageIndex} \n Error : {e.Message}");
        }
    }   

    private void InitStagePlacement(StagePlacement placement){
        tileMap = new Tile[placement.mapSize.x, placement.mapSize.y];
        
        for (int i = 0; i < placement.blockPos.Length; i++){
            // tileMap[placement.blockPos[i].x, placement.blockPos[i].y];
        }
    }

    private void InitStageQuest(StageQuest quest){
        stageQuest = quest;
    }
}


