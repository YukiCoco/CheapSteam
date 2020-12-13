<template>
  <v-app id="inspire">
    <v-app-bar app class="teal lighten-4">
      <v-toolbar-title>Cheap Steam</v-toolbar-title>
    </v-app-bar>

    <v-main class="grey lighten-5">
      <MainPage />
    </v-main>
    <v-footer >
      <v-col class="text-center" style="padding:3px;" cols="12">
        <p style="padding:0px;margin:0px;">{{ new Date().getFullYear() }} — <strong><a class="" href="https://github.com/YukiCoco/CheapSteam">Cheap Steam</a></strong></p>
        <p style="padding:0px;margin:0px;" class="font-weight-medium">i@kurisu.moe</p>
      </v-col>
    </v-footer>
  </v-app>
</template>

<script>
//import HelloWorld from './components/HelloWorld';
import MainPage from './Pages/Main';

export default {
  name: 'App',

  components: {
    MainPage
  },

  data: () => ({
  }),
};
</script>
