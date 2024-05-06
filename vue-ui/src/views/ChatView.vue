<script setup lang="ts">
import { ref, onMounted } from "vue";
import { type QuestionResponse, connPromise, isWebSocket, checkConnection } from "../utils/chatUtils";
import { isAuthenticated, getJwt, checkIsAuthenticated } from "@/utils/authUtils";
import NavBar from "@/components/NavBar.vue";
import AutoResizingTextarea from "@/components/AutoResizingTextarea.vue";
import autosize from 'autosize';

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

  const jwt = getJwt();

  connPromise(wsString, jwt)
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
  if (!checkConnection(ws)) {
    isAuthenticated.value = false;
    checkIsAuthenticated();
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
  console.log("Sent message:", question);
}

onMounted(() => {
  autosize(document.querySelectorAll('textarea'));
});
</script>

<template>
  <NavBar :is-authenticated="isAuthenticated" />
  <main class="bg-green-950 flex flex-col items-center h-screen">
    <div class="w-5/6" v-if="conversation.length">
      <div v-for="qr in conversation" :key="qr.question" class="flex flex-col mt-2">
        <textarea v-model="qr.question"
          class="font-bold no-scrollbar rounded-lg bg-special-pink pl-3 py-3 mb-2 w-full resize-vertical h-auto"
          readonly></textarea>
        <div class="relative flex items-center">
          <AutoResizingTextarea
            class="font-bold no-scrollbar rounded-lg bg-special-pink-2 pl-3 py-3 mb-2 w-full resize-vertical h-auto"
            v-model="qr.response" />
          <div class="absolute left-full top-0 ml-2">
            <div v-if="!qr.response || qr.response === ''">
              <span class="flex w-3 h-3 bg-red-500 rounded-full"></span>
            </div>
            <div v-else>
              <span class="flex w-3 h-3 bg-green-500 rounded-full"></span>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="flex items-center justify-center w-full">
      <div class="flex flex-col items-center fixed bottom-0 w-full justify-center pb-4">
        <div class="flex items-center w-5/6">
          <textarea v-model="quetionMsg"
            class="font-bold no-scrollbar rounded-lg bg-special-pink pl-3 py-3 w-full resize-none ml-4"></textarea>
          <button @click="send(quetionMsg)"
            class="bg-special-pink hover:bg-special-pink text-black font-bold py-1 px-4 rounded-full ml-2">
            Send
          </button>
        </div>
        <div class="flex items-center mt-2">
          <button @click="connect"
            class="bg-special-pink hover:bg-special-pink text-black font-bold py-1 px-4 rounded-full mr-2">
            {{ isConnected ? 'Disconnect' : 'Connect' }}
          </button>
          <div v-if="isConnected">
            <span class="flex w-3 h-3 me-3 bg-green-500 rounded-full"></span>
          </div>
          <div v-else>
            <span class="flex w-3 h-3 me-3 bg-red-500 rounded-full"></span>
          </div>
        </div>
      </div>
    </div>
  </main>
</template>