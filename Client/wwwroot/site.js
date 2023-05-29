
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
        
        let moves = game.moves({ square: square, verbose: false })
        console.log(moves)
        return moves; 
    }
}
