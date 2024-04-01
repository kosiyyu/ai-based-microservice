<script setup lang="ts">
import { ref } from 'vue';


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
    <div class="flex items-center justify-center w-full">
        <div class="flex items-end w-5/6 m-2">
        <textarea class="font-bold no-scrollbar rounded-lg bg-special-pink pl-3 py-3 w-5/6 resize-none"></textarea>
        <button class="bg-special-pink hover:bg-special-pink text-black font-bold py-1 px-4 rounded-full ml-2">
          Send
        </button>
      </div>
    </div>
    <button class="bg-special-pink hover:bg-special-pink text-black font-bold py-1 px-4 rounded-full mr-2">
        Connect
    </button>
  </main>
</template>