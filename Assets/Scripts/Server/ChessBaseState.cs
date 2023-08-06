namespace Server {
    public abstract class ChessBaseState {
        protected ChessGame Game;

        protected ChessBaseState(ChessGame game) {
            Game = game;
        }

        public abstract void Enter();
        public abstract void MakeMove(Move move, bool isFromWhite);
    }
}