<script setup lang="ts">

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
            <img src="./assets/github-mark.svg"/>
          </a>
        </div>
      </div>
    </div>
  </header>
    <main class="bg-green-950 flex flex-col items-center h-screen">
  <div class="flex flex-grow items-center justify-center flex-col">
    <div class="font-special text-special-pink text-5xl mb-4">
      Application name
    </div>
    <div class="flex items-center justify-center">
      <img src="./assets/pixeltrue-vision-1.svg" class="w-72 h-72"/>
      <div class="bg-black border border-black flex flex-col items-center justify-center p-6 inline-block rounded-tr-lg rounded-br-lg">
        <div class="m-2 flex flex-col">
          <label class="text-special-pink font-bold mb-2">Email</label>
          <input class="bg-special-pink  text-black rounded-lg focus:outline-none focus:ring-2 focus:ring-special-pink"/>
        </div>
        <div class="p-2 flex flex-col">
          <label class="text-special-pink font-bold mb-2">Password</label>
          <input class="bg-special-pink  text-black rounded-lg focus:outline-none focus:ring-2 focus:ring-special-pink"/>
        </div>
        <div class="flex items-center justify-center p-2">
          <button class="bg-special-pink hover:bg-special-pink text-black font-bold py-1 px-4 rounded-full">
            Log in
          </button>
        </div>
      </div>
    </div>
  </div>
</main>
    <footer class="bg-black">
      footer
    </footer>
</template>