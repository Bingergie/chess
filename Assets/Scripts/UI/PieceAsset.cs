using UnityEngine;

namespace UI {
    [CreateAssetMenu(fileName = "New Piece", menuName = "Piece", order = 0)]
    public class PieceAsset : ScriptableObject {
        public string pieceName;
        public Sprite whiteSprite;
        public Sprite blackSprite;
        
        
        public static PieceAsset FindPieceAsset(string pieceName) {
            var pieceAsset = Resources.Load<PieceAsset>("Pieces/" + pieceName);
            if (pieceAsset == null) {
                Debug.LogError("PieceAsset.FindPieceAsset: PieceAsset not found for piece name " + pieceName);
            }
            return pieceAsset;
        }
    }
}