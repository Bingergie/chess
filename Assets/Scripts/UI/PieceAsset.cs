using UnityEngine;

namespace UI {
    [CreateAssetMenu(fileName = "New Piece", menuName = "Piece", order = 0)]
    public class PieceAsset : ScriptableObject {
        public string pieceName;
        public Sprite whiteSprite;
        public Sprite blackSprite;
    }
}