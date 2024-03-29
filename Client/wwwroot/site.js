﻿
window.copyTextToClipboard = function (text) {
    const el = document.createElement('textarea');
    el.value = text;
    document.body.appendChild(el);
    el.select();
    document.execCommand('copy');
    document.body.removeChild(el);
};


window.chessFunctions = {
  
    getLegalMoves: function (fen, square) {
        game = new Chess(fen);
        console.log(game.ascii());

        let moves = game.moves({ square: square, verbose: true })
        console.log(moves)
        return moves; 
    },

       getCheckmateStatus: function (fen) {
        let game = new Chess(fen);
           let isCheckmate = game.game_over();
           console.log(isCheckmate);
        return isCheckmate;
    }
}
