<script setup lang="ts">
import { ref } from "vue";
import { type QuestionResponse, connPromise, isWebSocket, checkConnection} from "../utils/chatUtils";

const wsString = "ws://localhost:3333";

let quetionMsg = ref<string>("");
let isConnected = ref<boolean>(false);
let conversation = ref<QuestionResponse[]>([]);
let ws: WebSocket | undefined = undefined;

function connect(): void {
  if (isConnected.value) {
    if (isWebSocket(ws)) {
      ws.close();
    }
    isConnected.value = false;
    console.log("disconnected")
    return;
  }

  connPromise(wsString)
    .then(server => {
      ws = server;
      console.log("connected");
      isConnected.value = true;
      ws.addEventListener("message", msg => {
        console.log("Received message:", msg.data);
        conversation.value[conversation.value.length - 1].response += msg.data;
      });
    }).catch(err => {
      console.log("no connection:", err);
      isConnected.value = false;
    });
}

function send(question: string) {
  if(checkConnection(ws)) {
    return;
  }

  const qr: QuestionResponse = {
    question: question,
    response: null,

  }

  conversation.value.push(qr);
  const index = conversation.value.length - 1;
  conversation.value[index].response = "";
  ws?.send(question);
}
</script>

<template>
  <header class="bg-white py-2 px-1">
    <div class="flex items-center justify-between bg-white rounded-lg py-2 px-2">
      <div class="text-white">Logo</div>
      <div class="flex items-center">
        <button class="bg-white h-8 hover:bg-special text-black font-bold py-1 px-4 rounded-full ml-2">
          Log in
        </button>
        <button class="bg-white h-8 hover:bg-special text-black font-bold py-1 px-4 rounded-full ml-2">
          Register
        </button>
        <div class="ml-2 h-8 w-8">
          <a href="https://github.com/kosiyyu/ai-based-microservice">
            <img src="../assets/github-mark.svg" />
          </a>
        </div>
      </div>
    </div>
  </header>
  <main class="bg-green-950 flex flex-col items-center h-screen">
    <div v-if="conversation.length">
      <div v-for="qr in conversation" :key="qr.question" class="flex flex-col mt-2">
        <textarea v-model="qr.question"
            class="font-bold no-scrollbar rounded-lg bg-special-pink pl-3 py-3 resize-none mb-2" readonly></textarea>
        <div class="flex items-end">
          <textarea v-model="qr.response"
            class="font-bold rounded-lg bg-special-pink pl-3 py-3 w-full resize-vertical h-auto" readonly></textarea>
          <div v-if="!qr.response || qr.response === ''">
            <span class="flex w-3 h-3 me-3 bg-red-500 rounded-full"></span>
          </div>
          <div v-else>
            <span class="flex w-3 h-3 me-3 bg-green-500 rounded-full"></span>
          </div>
        </div>
      </div>
    </div>
    <div class="flex items-center justify-center w-full">
      <div class="flex items-end w-5/6 m-2">
        <textarea v-model="quetionMsg"
          class="font-bold no-scrollbar rounded-lg bg-special-pink pl-3 py-3 w-5/6 resize-none"></textarea>
        <button @click="send(quetionMsg)"
          class="bg-special-pink hover:bg-special-pink text-black font-bold py-1 px-4 rounded-full ml-2">
          Send
        </button>
      </div>
    </div>
    <div class="flex items-end">
      <button @click="connect"
        class="bg-special-pink hover:bg-special-pink text-black font-bold py-1 px-4 rounded-full mr-2">
        {{ isConnected ? 'Disconnect' : 'Connect' }}
      </button>
      <div v-if="isConnected">
        <span class="flex w-3 h-3 me-3 bg-green-500 rounded-full"></span> <!-- Removed self-end and bottom-0 classes -->
      </div>
      <div v-else>
        <span class="flex w-3 h-3 me-3 bg-red-500 rounded-full"></span> <!-- Removed self-end and bottom-0 classes -->
      </div>
    </div>
  </main>
</template>