import { WebSocket } from "ws";
import { Ollama } from "ollama-node";

const ollama = new Ollama();
ollama.setModel("dolphin-phi")
  .then(() => {
    console.log("Model loaded");
  }).catch((err) => {
    console.log(err);
  });

const wsServer = new WebSocket.Server({ port: 3333 }, () => {
  console.log(`Server is running on port ${wsServer.options.port}`);
});

wsServer.on("connection", socket => {
  console.log("connected")

  socket.on("message", function(msg) {
    console.log('received: %s', msg);
      ollama.streamingGenerate(`${msg}`, (output: string) => {
        socket.send(output);
      });
  });
});