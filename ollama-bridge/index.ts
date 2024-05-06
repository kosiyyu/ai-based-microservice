import { WebSocket } from "ws";
import { Ollama } from "ollama-node";
import axios from 'axios';

const modelName = "dolphin-phi";

const ollama = new Ollama();
ollama.setModel(modelName)
  .then(() => {
    console.log(modelName);
  }).catch((err) => {
    console.log(err);
  });

const wsServer = new WebSocket.Server({
  port: 3333,
  verifyClient: async (info, done) => {
    const token = info.req.headers['sec-websocket-protocol'];
    console.log(token);
    if (!token) {
      console.log('No token');
      done(false, 401, 'Unauthorized');
    } else {
      console.log('Token');
      try {
        const response = await axios.get('http://localhost:5003/api/validate', {
          headers: {
            'Authorization': `Bearer ${token}`
          },
          withCredentials: true
        });
        console.log(response);
        if (response.status === 200) {
          console.log(response);
          done(true);
        } else {
          done(false, 401, 'Unauthorized');
        }
      } catch (err) {
        done(false, 401, 'Unauthorized');
      }
    }
  }
}, () => {
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
      console.log(output);
      socket.send(output);
    });
  });
});