<template>
  <v-footer height="auto" class="py-3" style="background-color: transparent !important;">
    <v-container>
      <v-layout justify-center>
        <v-flex xs2 pb-4>
          <v-select :items="languages" box hide-details label="Language"></v-select>
        </v-flex>
      </v-layout>

      <v-layout justify-center>
        <v-flex xs12 text-xs-center>
          <v-btn v-for="link in links" :key="link.name" flat round :href="link.href">{{ link.name }}</v-btn>
        </v-flex>
      </v-layout>

      <v-layout justify-center>
        <v-flex xs12 text-xs-center>
          <img
            :src="require('../../images/atlassian-logo-gray-small.svg')"
            width="150"
          >
          <span
            class="subheading font-weight-light"
          >&nbsp;&nbsp;&copy; Copyright 2019. All rights reserved.</span>
        </v-flex>
      </v-layout>
    </v-container>
  </v-footer>
</template>

<script>
export default {
  name: "landingfooter",
  data: () => ({
    links: [
      { name: "Tour", href: "https://trello.com/tour" },
      { name: "Pricing", href: "https://trello.com/pricing" },
      { name: "Apps", href: "https://trello.com/platforms" },
      { name: "Jobs", href: "https://trello.com/jobs" },
      { name: "Blog", href: "https://trello.com/blog" },
      { name: "Developers", href: "https://developers.trello.com" },
      { name: "About", href: "https://trello.com/about" },
      { name: "Help", href: "https://trello.com/help" },
      { name: "Legal", href: "https://trello.com/legal" },
      { name: "Privacy", href: "https://trello.com/privacy" }
    ],
    languages: ["한국어", "English"]
  })
};
</script>