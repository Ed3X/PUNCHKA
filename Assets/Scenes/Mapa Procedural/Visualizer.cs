using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SVS.Simple_Visualizer;

namespace SVS
{
    public class Visualizer : MonoBehaviour
    {
        public LSystem_Generator lsystem;

        public RoadHelper roadHelper;
        public StructureHelper structureHelper;

        public SpawnLocalizer spawnLocalizer;

        public LevelScript levelScript;

        // var random = UnityEngine.Random.Range(0, prefabs.Length);
        public int roadLength = 8;
        private int length = 8;
        private float angle = 90;

        public int Length
        {
            get
            {
                if (length > 0)
                {
                    return length;
                }
                else
                {
                    return 1;
                }
            }
            set => length = value;
        }

        private void Start()
        {
            CreateTown();
        }

        public void CreateTown()
        {
            length = roadLength;
            roadHelper.Reset();
            structureHelper.Reset();
            var sequence = lsystem.GenerateSentence();
            VisualizeSequence(sequence);
        }

        private void VisualizeSequence(string sequence)
        {
            Stack<SavePoint> savePoints = new Stack<SavePoint>();
            var currentPosition = Vector3.zero;

            Vector3 direction = Vector3.forward;
            Vector3 tempPosition = Vector3.zero;

            foreach (var letter in sequence)
            {
                EncodingLetters encoding = (EncodingLetters)letter;
                switch (encoding)
                {
                    case EncodingLetters.save:
                        savePoints.Push(new SavePoint
                        {
                            position = currentPosition,
                            direction = direction,
                            length = Length
                        });
                        break;
                    case EncodingLetters.load:
                        if (savePoints.Count > 0)
                        {
                            var savePoint = savePoints.Pop();
                            currentPosition = savePoint.position;
                            direction = savePoint.direction;
                            length = savePoint.length;
                        }
                        else
                        {
                            throw new System.Exception("Don't have saved point in our stack");
                        }
                        break;
                    case EncodingLetters.draw:
                        tempPosition = currentPosition;
                        currentPosition += direction * length;
                        roadHelper.PlaceStreetPositions(tempPosition, Vector3Int.RoundToInt(direction), length);
                        Length -= 2;
                        break;
                    case EncodingLetters.turnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                        break;
                    case EncodingLetters.turnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                        break;
                    default:
                        break;
                }
            }
            roadHelper.FixRoad();
            structureHelper.PlaceStructuresAroundRoad(roadHelper.GetRoadPositions());
        }
    }
}
