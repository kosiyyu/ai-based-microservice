import { WebSocket } from "ws";
import { Ollama } from "ollama-node";

const modelName = "dolphin-phi";

const ollama = new Ollama();
ollama.setModel(modelName)
  .then(() => {
    console.log(modelName);
  }).catch((err) => {
    console.log(err);
  });

const wsServer = new WebSocket.Server({ port: 3333 }, () => {
  console.log(`Server is running on port ${wsServer.options.port}`);
});

wsServer.on("connection", socket => {
  console.log("connected")
  socket.on("message", function (msg) {
    console.log('received: %s', msg);
    if (!msg || `${msg}` === "") {
      return;
    }
    console.log("pass");
    ollama.streamingGenerate(`${msg}`, (output: string) => {
      socket.send(output);
    });
  });
});