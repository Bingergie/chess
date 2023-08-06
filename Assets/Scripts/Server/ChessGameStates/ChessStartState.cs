using Riptide;
using Shared;
using UnityEngine;

namespace Server.ChessGameStates {
    public class ChessStartState : ChessBaseState {
        public ChessStartState(ChessGame game) : base(game) {
        }

        public override void Enter() {
            // set white player
            Game.SetWhitePlayer(Game.PlayerIds[Random.Range(0, 2)]);
            SendGameStartMessage();
            Game.ChangeState(typeof(ChessPlayingState));
        }

        public override void MakeMove(Move move, bool isFromWhite) {
            Debug.Log("can't make move now");
        }

        private void SendGameStartMessage() {
            var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.GameStart);
            message.Add(Game.WhitePlayerId);
            message.Add(BoardUtil.PiecesFromFen());
            
            NetworkManager.Instance.Server.Send(message, Game.PlayerIds[0]);
            NetworkManager.Instance.Server.Send(message, Game.PlayerIds[1]);
        }
    }
}