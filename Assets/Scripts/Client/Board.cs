using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Client {
    public class Board : MonoBehaviour {
        [SerializeField] private bool whiteIsBottom = true;
        [SerializeField] private float boardDepth = 0;
        [SerializeField] private float pieceDepth = 1f;
        [SerializeField] private float pieceDragDepth = 2f;
        [SerializeField] private float boardScale = 1f;
        [SerializeField] private Color whiteColor = Color.white;
        [SerializeField] private Color blackColor = Color.black;
        [SerializeField] private Color highlightColor = Color.yellow;

        private GameObject[] _squares;
        private GameObject[] _pieces;
        private Piece[] _board;
        private Coord _selectedSquare;

        private void Start() {
            // subscribe to events
            GameManager.Instance.OnGameStart += GenerateBoard;
            GameManager.Instance.OnBoardUpdated += MoveMade;
            PlayerManager.OnSquareSelected += ToggleHighlightSquare;
            PlayerManager.OnSquareDeselected += ToggleHighlightSquare;
            PlayerManager.OnPieceDragged += DragPiece;
            /*PlayerManager.Instance.OnSquareSelected += ToggleHighlightSquare;
            PlayerManager.Instance.OnSquareDeselected += ToggleHighlightSquare;
            PlayerManager.Instance.OnPieceDragged += DragPiece;*/

            // initialize arrays
            _squares = new GameObject[64];
            _pieces = new GameObject[64];
            _board = new Piece[64];
            _selectedSquare = Coord.None;

            // GenerateBoard();
            // UpdatePieces(this, BoardUtil.PiecesFromFen());
        }

        private void OnDestroy() {
            // unsubscribe from events
            GameManager.Instance.OnGameStart -= GenerateBoard;
            GameManager.Instance.OnBoardUpdated -= MoveMade;
            PlayerManager.OnSquareSelected -= ToggleHighlightSquare;
            PlayerManager.OnSquareDeselected -= ToggleHighlightSquare;
            PlayerManager.OnPieceDragged -= DragPiece;
        }

        private void UpdatePieces(Piece[] pieces) {
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

            int i = 0;
            foreach (var piece in pieces) {
                if (piece == Piece.None) {
                    _pieces[i].GetComponent<SpriteRenderer>().enabled = false;
                    _pieces[i].name = "None";
                    _board[i] = Piece.None;
                    i++;
                    continue;
                }

                var pieceAsset = PieceAsset.FindPieceAsset(piece.Name);
                _pieces[i].name = pieceAsset.name;
                _pieces[i].GetComponent<SpriteRenderer>().enabled = true;
                _pieces[i].GetComponent<SpriteRenderer>().sprite =
                    piece.IsWhite ? pieceAsset.whiteSprite : pieceAsset.blackSprite;
                _board[i] = piece;
                i++;
            }


            /*for (int file = 0; file < 8; file++) {
                for (int rank = 0; rank < 8; rank++) {
                    var index = BoardUtil.IndexFromCoord(file, rank);
                    var piece = pieces[index];
                    if (piece != Piece.None) {
                        var pieceAsset = PieceAsset.FindPieceAsset(piece.Name);
                        _pieces[file, rank].name = pieceAsset.name;
                        _pieces[file, rank].GetComponent<SpriteRenderer>().enabled = true;
                        _pieces[file, rank].GetComponent<SpriteRenderer>().sprite =
                            piece.IsWhite ? pieceAsset.whiteSprite : pieceAsset.blackSprite;
                        _board[index] = piece;
                    }
                    else {
                        _pieces[file, rank].GetComponent<SpriteRenderer>().enabled = false;
                        _pieces[file, rank].name = "None";
                        _board[index] = Piece.None;
                    }
                }
            }*/
        }

        private void GenerateBoard(object sender, Piece[] pieces) {
            whiteIsBottom = GameManager.IsWhite;
            for (int i = 0; i < 64; i++) {
                var coord = BoardUtil.CoordFromIndex(i);
                var square = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                square.localScale = new Vector3(boardScale, 0.1f, boardScale);
                square.parent = transform;
                square.localPosition = PositionFromCoord(coord, boardDepth);
                square.gameObject.name = BoardUtil.NameFromCoord(coord);
                square.gameObject.layer = LayerMask.NameToLayer("Board");
                var squareRenderer = square.GetComponent<Renderer>();
                squareRenderer.material.color = coord.IsLightSquare() ? whiteColor : blackColor;
                _squares[i] = square.gameObject;

                var pieceTransform = new GameObject("Piece").AddComponent<SpriteRenderer>().transform;
                pieceTransform.parent = square;
                pieceTransform.localPosition = new Vector3(0, pieceDepth, 0);
                pieceTransform.localScale = Vector2.one * (boardScale * 0.07f);
                pieceTransform.localRotation = Quaternion.Euler(90, 0, 0);
                pieceTransform.gameObject.layer = LayerMask.NameToLayer("Piece");
                _pieces[i] = pieceTransform.gameObject;
            }
            /*for (int file = 0; file < 8; file++) {
                for (int rank = 0; rank < 8; rank++) {
                    // square
                    var square = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                    square.localScale = new Vector3(boardScale, 0.1f, boardScale);
                    square.parent = transform;
                    square.localPosition = PositionFromCoord(file, rank, boardDepth);
                    square.gameObject.name = BoardUtil.NameFromCoord(file, rank);
                    square.gameObject.layer = LayerMask.NameToLayer("Board");
                    var squareRenderer = square.GetComponent<Renderer>();
                    squareRenderer.material.color = (file + rank) % 2 == 0 ? whiteColor : blackColor;
                    _squares[file, rank] = square.gameObject;

                    var piece = new GameObject("Piece").AddComponent<SpriteRenderer>().transform;
                    piece.parent = square;
                    piece.localPosition = new Vector3(0, pieceDepth, 0);
                    piece.localScale = Vector2.one * (boardScale * 0.07f);
                    piece.localRotation = Quaternion.Euler(90, 0, 0);
                    piece.gameObject.layer = LayerMask.NameToLayer("Piece");
                    _pieces[file, rank] = piece.gameObject;
                }
            }*/

            UpdatePieces(pieces);
        }

        private void MoveMade(object sender, Move move) {
            var pieces = _board;
            pieces[move.ToIndex] = pieces[move.FromIndex];
            pieces[move.FromIndex] = Piece.None;
            UpdatePieces(pieces);
        }

        private void ToggleHighlightSquare(object sender, Coord coord) {
            var index = BoardUtil.IndexFromCoord(coord);
            var square = _squares[index];
            var squareRenderer = square.GetComponent<Renderer>();
            squareRenderer.material.color = squareRenderer.material.color == highlightColor
                ? coord.IsLightSquare() ? whiteColor : blackColor
                : highlightColor;
        }

        private void DragPiece(object sender, Ray ray) {
            /*var piece = _pieces[_selectedSquare.FileIndex, _selectedSquare.RankIndex];
            piece.transform.position = */
        }

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