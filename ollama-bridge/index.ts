import { WebSocket } from 'ws';
import { Ollama } from 'ollama-node';

const ollama = new Ollama();
ollama.setModel('dolphin-phi')
.then(() => {
  console.log("Model loaded");
}).catch((err) => {
  console.log(err);
});

const wss = new WebSocket.Server({ port: 3333 }, () =>{
  console.log(`Server is running on port ${wss.options.port}`);
});

function connectionString(prompt: string) {
  wss.on('connection', function connection(ws) {
    //todo validate connection
    console.log('connected')
    ollama.streamingGenerate(prompt, (word: string) => {
      ws.send(word);
    });
  });
}

wss.on('connection', function connection(ws) {
  //todo validate connection
  console.log('connected')
  ollama.streamingGenerate("why is the sky blue", (word: string) => {
    ws.send(word);
  });
});