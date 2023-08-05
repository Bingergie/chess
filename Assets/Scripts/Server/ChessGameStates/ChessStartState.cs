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

        private void SendGameStartMessage() {
            var message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.GameStart);
            message.Add(Game.WhitePlayerId);
            
            NetworkManager.Instance.Server.Send(message, Game.PlayerIds[0]);
            NetworkManager.Instance.Server.Send(message, Game.PlayerIds[1]);
        }
    }
}