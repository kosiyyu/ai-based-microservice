<script setup lang="ts">
import { ref } from "vue";

let quetionMsg = ref<string>("");
let askedQuestion = ref<string>("");

type QuestionResponse = {
  question: string;
  response: string | null;
}

// const conversation: QuestionResponse[] = [];

let conversation = ref<QuestionResponse[]>([]);

let ws: WebSocket | undefined = undefined;

function connPromise(): Promise<WebSocket> {
  return new Promise((resolve, reject) => {
      const ws = new WebSocket("ws://localhost:3333");
      ws.onopen = () => {
        resolve(ws);
      }
      ws.onerror = (err) => {
        reject(err);
      }  
  });
}

function isWebSocket(obj: unknown): obj is WebSocket{
  return obj instanceof WebSocket;
}

function connect(): void {
  connPromise()
  .then(server => {
    ws = server;
    console.log("connected");
    ws.addEventListener("message", msg => {
      console.log("Received message:", msg.data);
      conversation.value[conversation.value.length - 1].response += msg.data;
    });
  }).catch(err => {
    console.log("no connection:",err);
  });
}

function send(question: string) {
  if(!isWebSocket(ws)) {
    console.log("WebSocket not initalized");
    return;
  }

  const qr: QuestionResponse = {
    question: question,
    response: null
  }

  conversation.value.push(qr);
  const index = conversation.value.length - 1;
  conversation.value[index].response = "";
  ws.send(question);
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
            <img src="../assets/github-mark.svg"/>
          </a>
        </div>
      </div>
    </div>
  </header>
  <main class="bg-green-950 flex flex-col items-center h-screen">
    <div v-if="conversation.length">
      <div v-for="qr in conversation" :key="qr.question" class="question-item">
        <textarea v-model="qr.question" class="font-bold no-scrollbar rounded-lg bg-special-pink pl-3 py-3 w-5/6 resize-none" readonly></textarea>
        <textarea v-model="qr.response" class="font-bold no-scrollbar rounded-lg bg-special-pink pl-3 py-3 w-5/6 resize-none" readonly></textarea>
      </div>
    </div>
    <div class="flex items-center justify-center w-full">
      <div class="flex items-end w-5/6 m-2">
        <textarea v-model="quetionMsg" class="font-bold no-scrollbar rounded-lg bg-special-pink pl-3 py-3 w-5/6 resize-none"></textarea>
        <button @click ="send(quetionMsg)" class="bg-special-pink hover:bg-special-pink text-black font-bold py-1 px-4 rounded-full ml-2">
          Send
        </button>
      </div>
    </div>
    <button @click="connect" class="bg-special-pink hover:bg-special-pink text-black font-bold py-1 px-4 rounded-full mr-2">
        Connect
    </button>
  </main>
</template>