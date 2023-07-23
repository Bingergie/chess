using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public class BoardVisual : MonoBehaviour {
        [SerializeField] private bool whiteIsBottom = true;
        [SerializeField] private float boardDepth = 0;
        [SerializeField] private float pieceDepth = 0.5f;
        [SerializeField] private float boardScale = 1f;
        [SerializeField] private Color whiteColor = Color.white;
        [SerializeField] private Color blackColor = Color.black;
    
        private Renderer[,] squareRenderers;
        private SpriteRenderer[,] pieceRenderers;

        private void Awake() {
            // subscribe to events
            GameManager.OnBoardUpdate += UpdateBoard;
        
            GenerateBoard();
            UpdateBoard(this, BoardUtil.PiecesFromFen());
        }
    
        private void OnDestroy() {
            // unsubscribe from events
            GameManager.OnBoardUpdate -= UpdateBoard;
        }

        private void Update() {
            //GenerateBoard();
        }

        private void UpdateBoard(object sender, Piece[] pieces) {
            
            // todo: delete this section from here...
            Debug.Log("updating board ui");
            string pieceNamestring = "";
            foreach (var piece in pieces) {
                if (EqualityComparer<Piece>.Default.Equals(piece, default)) pieceNamestring += "none ";
                else pieceNamestring += piece.name + " ";
            }
            Debug.Log(pieceNamestring);
            // todo: ...to here 


            for (int file = 0; file < GameManager.Width; file++) {
                for (int rank = 0; rank < GameManager.Height; rank++) {
                    var piece = pieces[BoardUtil.CoordFromFileRank(file, rank)];
                    if (EqualityComparer<Piece>.Default.Equals(piece,default)) {
                        pieceRenderers[file, rank].enabled = false;
                    } else {
                        pieceRenderers[file, rank].enabled = true;
                        var pieceAsset = FindPieceAsset(piece.name);
                        pieceRenderers[file, rank].name = pieceAsset.name;
                        pieceRenderers[file, rank].sprite = piece.isWhite ? pieceAsset.whiteSprite : pieceAsset.blackSprite;
                    }
                }
            }
        }

        private void GenerateBoard() {
            squareRenderers = new Renderer[8, 8];
            pieceRenderers = new SpriteRenderer[8, 8];
            Debug.Log("generating board ui");
        
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
                    squareRenderers[file, rank] = squareRenderer;

                    // empty renderer for pieces
                    SpriteRenderer pieceRenderer = new GameObject ("Piece").AddComponent<SpriteRenderer>();
                    pieceRenderer.transform.parent = square.transform;
                    pieceRenderer.transform.localPosition = new Vector3(0, pieceDepth, 0);
                    pieceRenderer.transform.localScale = Vector2.one * (boardScale * 0.07f);
                    pieceRenderer.transform.rotation = Quaternion.Euler(90, 0, 0);
                    pieceRenderers[file, rank] = pieceRenderer;
                }
            }
        }

        private PieceAsset FindPieceAsset(string pieceName) {
            var pieceAsset = Resources.Load<PieceAsset>("Pieces/" + pieceName);
            if (pieceAsset == null) {
                Debug.LogError("Piece asset not found for " + pieceName);
            }

            return pieceAsset;
        }

        private Vector3 PositionFromCoord(int file, int rank, float depth = 0) {
            var row = whiteIsBottom ? rank : 7 - rank;
            var col = whiteIsBottom ? file : 7 - file;
            return new Vector3((float)((col - 3.5) * boardScale), depth, (float)((row - 3.5) * boardScale));
        }

        private Vector3 PositionFromCoord(Coord coord, float depth = 0) {
            return PositionFromCoord(coord.fileIndex, coord.rankIndex, depth);
        }
    }
}