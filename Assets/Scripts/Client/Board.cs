using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Client {
    public class Board : MonoBehaviour {
        [SerializeField] private bool whiteIsBottom = true;
        [SerializeField] private float boardDepth = 0;
        [SerializeField] private float pieceDepth = 0.5f;
        [SerializeField] private float boardScale = 1f;
        [SerializeField] private Color whiteColor = Color.white;
        [SerializeField] private Color blackColor = Color.black;
    
        private GameObject[,] _squares;
        private GameObject[,] _pieces;

        private void Start() {
            // subscribe to events
            GameManager.Instance.OnGameStart += GenerateBoard;
            GameManager.Instance.OnBoardUpdated += UpdateBoard;
        
            // initialize arrays
            _squares = new GameObject[8, 8];
            _pieces = new GameObject[8, 8];
            
            // GenerateBoard();
            // UpdateBoard(this, BoardUtil.PiecesFromFen());
        }
    
        private void OnDestroy() {
            // unsubscribe from events
            GameManager.Instance.OnBoardUpdated -= UpdateBoard;
        }

        private void UpdateBoard(object sender, Piece[] pieces) {
            
            /*// todo: delete this section from here...
            Debug.Log("updating board ui");
            string pieceNamestring = "";
            foreach (var piece in pieces) {
                if (EqualityComparer<Piece>.Default.Equals(piece, default)) pieceNamestring += "none ";
                else pieceNamestring += piece.Name + " ";
            }
            Debug.Log(pieceNamestring);
            // todo: ...to here 
            */


            for (int file = 0; file < 8; file++) {
                for (int rank = 0; rank < 8; rank++) {
                    var piece = pieces[BoardUtil.CoordFromFileRank(file, rank)];
                    if (piece != Piece.None) {
                        var pieceAsset = PieceAsset.FindPieceAsset(piece.Name);
                        if (pieceAsset == null) {
                            Debug.LogError($"Couldn't find piece asset for piece {piece.Name}!");
                            continue;
                        }
                        _pieces[file, rank].name = pieceAsset.name;
                        _pieces[file, rank].AddComponent<SpriteRenderer>().sprite = piece.IsWhite ? pieceAsset.whiteSprite : pieceAsset.blackSprite;
                    }
                    else {
                        Destroy(_pieces[file, rank].GetComponent<SpriteRenderer>());
                    }
                }
            }
        }

        private void GenerateBoard(object sender, Piece[] pieces) {
            for (int file = 0; file < 8; file++) {
                for (int rank = 0; rank < 8; rank++) {
                    // square
                    var square = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                    square.localScale = new Vector3(boardScale, 0.1f, boardScale);
                    square.parent = transform;
                    square.position = PositionFromCoord(file, rank, boardDepth);
                    square.gameObject.name = BoardUtil.NameFromCoord(file, rank);
                    square.gameObject.layer = LayerMask.NameToLayer("Board");
                    var squareRenderer = square.GetComponent<Renderer>();
                    squareRenderer.material.color = (file + rank) % 2 == 0 ? whiteColor : blackColor;
                    _squares[file, rank] = square.gameObject;

                    /*// empty renderer for pieces
                    var pieceRenderer = new GameObject ("Piece").AddComponent<SpriteRenderer>();
                    pieceRenderer.transform.parent = square.transform;
                    pieceRenderer.transform.localPosition = new Vector3(0, pieceDepth, 0);
                    pieceRenderer.transform.localScale = Vector2.one * (boardScale * 0.07f);
                    pieceRenderer.transform.rotation = Quaternion.Euler(90, 0, 0);
                    pieces[file, rank] = pieceRenderer;*/
                    var piece = new GameObject("Piece").transform;
                    piece.parent = square;
                    piece.localPosition = new Vector3(0, pieceDepth, 0);
                    piece.localScale = Vector2.one * (boardScale * 0.07f);
                    piece.localRotation = Quaternion.Euler(90, 0, 0);
                    _pieces[file, rank] = piece.gameObject;
                }
            }
            UpdateBoard(sender, pieces);
        }

        /*
        private PieceAsset FindPieceAsset(string pieceName) {
            var pieceAsset = Resources.Load<PieceAsset>("Pieces/" + pieceName);
            if (pieceAsset == null) {
                Debug.LogError("Piece asset not found for " + pieceName);
            }

            return pieceAsset;
        }*/

        private Vector3 PositionFromCoord(int file, int rank, float depth = 0) {
            var row = whiteIsBottom ? rank : 7 - rank;
            var col = whiteIsBottom ? file : 7 - file;
            return new Vector3((float)((col - 3.5) * boardScale), depth, (float)((row - 3.5) * boardScale));
        }

        private Vector3 PositionFromCoord(Coord coord, float depth = 0) {
            return PositionFromCoord(coord.FileIndex, coord.RankIndex, depth);
        }
    }
}