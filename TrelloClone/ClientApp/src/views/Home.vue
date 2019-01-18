<template>
  <v-app>
    <landing-header></landing-header>
    <v-content>

      <section class="title-section">
        <v-card flat tile dark class="py-5 px-3" style="background-color:transparent !important">
          <v-container grid-list-sm class="mw-2 mt-5 py-5">
            <v-layout justify-center column>
              <v-flex xs12 text-xs-center>
                <div class="display-2 font-weight-bold lh-1">Trello Clone Project</div>
                <p class="headline font-weight-light lh-1 ls-1">(Asp.net Core + Vue.js)</p>
              </v-flex>
            </v-layout>
            <v-layout row class="mt-5">
              <v-flex xs6>
                <div class="pb-1"> &nbsp;&nbsp;Back-end</div>
                <ul>  
                  <li>ASP.NET Core (메인 프레임워크)</li>
                  <li>SQL Server + EntityFramwork Core (데이터베이스)</li>
                  <li>JWT (사용자 인증 및 세션관리)</li>
                  <li>Swagger (API 문서 자동생성)</li>
                  <li>NLog (로그생성)</li>
                  <li>AutoMapper (객체 자동 매핑)</li>
                </ul>
                <br>

                <div class="pb-1"> &nbsp;&nbsp;Front-end</div>
                <ul>
                  <li>Vue.js (메인 프레임워크)</li>
                    <li>Vuetify (UI 프레임워크)</li>
                    <li>vuex (상태관리)</li>
                    <li>vue-router (클라이언트 라우팅)</li>
                    <li>axios (promise기반 http통신)</li>
                    <li>lodash (자바스크립트 유틸)</li>
                    <li>vee-validate (폼 유효성검사)</li>
                    <li>nprogress (로딩바)</li>
                </ul>
              </v-flex>

              <v-flex xs6>
                <div class="pb-1"> &nbsp;&nbsp;개발 상세내용</div>
                <ul>
                  <li>PC화면 전용. Mobile은 고려되지 않음</li>
                  <li>서버사이드렌더링(SSR) 적용하지 않음 (Asp.net Razor로 적용가능함)</li>
                  <li>패스워드는 hmacsha512로 암호화 (더 안전한 방법필요)</li>
                  <li>JWT를 쿠키에 저장함. (httpOnly, secure)</li>
                  <li>토큰관련 미들웨어 2개 생성
                    <ol>
                      <li>JWTInHeaderMiddleware: 쿠키로 넘어온 토큰을 헤더에 주입</li>
                      <li>JWTRefreshMiddleware: 토큰 만료기간 50%이내에 요청시 토큰 자동 재발급</li>
                    </ol>
                  </li>
                  <li>토큰 유효성검사시 IP도 체크함. 토큰생성 IP와 다르면 권한없음</li>
                  <li>lodash의 debounce로 연속된 키입력에 의한 연속 api호출 방지</li>
                  <li>nprogress를 전역에 등록하여 라우팅, api전송시에 자동으로 로딩바 표시</li>
                </ul>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card>
      </section>

      <section>
        <v-card color="primary" flat tile dark class="pt-5 px-3 pb-4">
          <v-container grid-list-sm class="mw-1 mt-5">
            <v-layout justify-center row>
              <v-flex xs6>
                <div class="display-2 font-weight-bold mb-4 lh-1">Trello lets you work more collaboratively and get more done.</div>
                <p class="headline font-weight-light lh-1 ls-1">Trello’s boards, lists, and cards enable you to organize and prioritize your projects in a fun, flexible and rewarding way.</p>
                <v-btn to="/register" color="success" large>Sing Up - It's Free</v-btn>
              </v-flex>
              <v-flex xs6>
                <v-img :src="require('../images/hero-a.svg')"></v-img>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card>
      </section>

      <section>
        <v-card color="white" flat tile light class="pt-5 px-3">
          <v-container grid-list-sm class="mw-1">
            <v-layout justify-center row align-center>
              <v-flex xs5>
                <div class="display-1 font-weight-medium mb-4 lh-1">Work with any team</div>
                <p class="title font-weight-light lh-1">Whether it’s for work, a side project or even the next family vacation, Trello helps your team stay organized.</p>
                <v-btn to="/register" color="default" large>Start doing →</v-btn>
              </v-flex>
              <v-flex xs7>
                <v-img :src="require('../images/updated-layouts-board.png')"></v-img>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card>
      </section>

      <section>
        <v-card color="white" flat tile light class="pt-5 px-3">
          <v-container grid-list-sm class="mw-1">
            <v-layout justify-center row align-center>
              <v-flex xs6>
                <v-img :src="require('../images/updated-layouts-mobile.png')"></v-img>
              </v-flex>
              <v-flex xs6>
                <div class="display-1 font-weight-medium mb-4 lh-1">Information at a glance</div>
                <p class="title font-weight-light lh-1">Dive into the details by adding comments, attachments, due dates, and more directly to Trello cards. Collaborate on projects from beginning to end.</p>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card>
      </section>

      <section>
        <v-card color="white" flat tile light class="pt-5 px-3">
          <v-container grid-list-sm class="mw-1">
            <v-layout justify-center column align-center>
              <v-flex>
                <div class="display-1 font-weight-medium mb-4 lh-1">See how it works</div>
              </v-flex>
              <v-flex text-xs-center>
                <p class="title font-weight-light lh-1 mw-3">Go from idea to action in seconds with Trello’s intuitively simple boards, lists, and cards.</p>
              </v-flex>
                <v-carousel height="auto" class="mw-2 no-shadow">
                  <v-carousel-item
                    v-for="(item,i) in carouselItems"
                    :key="i"
                  >
                    <v-img :src="item.src" style="width:100%;"></v-img>
                    <div class="carousel title py-4">{{item.caption}}</div>
                  </v-carousel-item>
                </v-carousel>
            </v-layout>
          </v-container>
        </v-card>
      </section>

      <section>
        <v-card color="white" flat tile light class="pt-5 px-3">
          <v-container grid-list-sm class="mw-1">
            <v-layout justify-center column align-center>
              <v-flex>
                <div class="display-1 font-weight-medium mb-4 lh-1">Trello your way</div>
              </v-flex>
              <v-flex text-xs-center>
                <p class="title font-weight-light lh-1 mw-3">Use Trello the way your team works best. We’ve got the flexibility & features to fit any team’s style.</p>
              </v-flex>
              <v-flex>
                <v-container>
                  <v-layout row>
                    <v-flex xs4 text-xs-center class="px-4">
                      <v-img :src="require('../images/updated-layouts-collab.png')"></v-img>
                      <div class="headline font-weight-medium mb-4 lh-1">Trello your way</div>
                      <p class="subheading font-weight-light lh-1 mw-3">Use Trello the way your team works best. We’ve got the flexibility & features to fit any team’s style.</p>
                      <v-btn href="https://trello.com/teams" color="default">Build Your Dream Team</v-btn>
                    </v-flex>
                    <v-flex xs4 text-xs-center class="px-4">
                      <v-img :src="require('../images/updated-layouts-platform.png')"></v-img>
                      <div class="headline font-weight-medium mb-4 lh-1">A Productivity Platform</div>
                      <p class="subheading font-weight-light lh-1 mw-3">Integrate the apps your team already uses directly into your workflow. Power-Ups turn Trello boards into living applications to meet your team's unique business needs.</p>
                      <v-btn href="https://trello.com/power-ups" color="default">Power-Up Your Workflow</v-btn>
                    </v-flex>
                    <v-flex xs4 text-xs-center class="px-4">
                      <v-img :src="require('../images/updated-layouts-sync.png')"></v-img>
                      <div class="headline font-weight-medium mb-4 lh-1">Always In Sync</div>
                      <p class="subheading font-weight-light lh-1 mw-3">No matter where you are, Trello stays in sync across all of your devices. Collaborate with your team anywhere, from sitting on the bus to sitting on the beach.</p>
                      <v-btn href="https://itunes.apple.com/app/trello-organize-anything/id461504587" color="white" class="btn-app1" flat></v-btn>
                      <v-btn href="https://play.google.com/store/apps/details?id=com.trello" color="white" class="btn-app2" flat></v-btn>
                    </v-flex>
                  </v-layout>
                </v-container>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card>
      </section>

      <div id="bottom-gradient" style="background: linear-gradient(to bottom, #fff 0, #f7ecff 46%, #d2ecff 74%, #fff 100%)">
      <section>
        <v-layout justify-center>
          <v-flex class="pt-5 px-3 mw-1">
            <v-card flat tile dark style="background: linear-gradient(180deg, #42548E 0, #6B668C 100%); border-radius: 12px;">
              <v-container grid-list-xl class="mw-1 py-5">
                <v-layout justify-center column align-center class="py-2">
                  <v-flex text-xs-center>
                    <div class="display-1 font-weight-medium mb-4 lh-1">Work smarter with Trello</div>
                    <div class="title font-weight-light lh-1 pt-2 pb-5">Companies of all shapes and sizes use Trello.</div>
                    <img :src="require('../images/logo-adobe.svg')" class="px-2">
                    <img :src="require('../images/logo-kickstarter.svg')" class="px-2">
                    <img :src="require('../images/logo-national-geographic.svg')" class="px-2">
                    <img :src="require('../images/logo-google.svg')" class="px-2">
                    <img :src="require('../images/logo-fender.svg')" class="px-2">
                  </v-flex>
                  <v-flex text-xs-center class="pt-5">
                    <v-btn href="https://trello.com/business-class" color="default" light>Find out how →</v-btn>
                  </v-flex>                    
                </v-layout>
              </v-container>
            </v-card>
          </v-flex>
        </v-layout>
      </section>

      <section>
        <v-container grid-list-sm class="py-5 px-3 my-5 mw-1">
          <v-layout justify-center column align-center>
            <v-flex>
              <div class="display-1 font-weight-medium mb-4 lh-1">Start Planning Today</div>
            </v-flex>
            <v-flex text-xs-center>
              <p class="title font-weight-light lh-1 mw-3">Sign up and become one of the millions of people around the world using Trello to get more done.</p>
            </v-flex>
            <v-flex>
              <v-btn to="/register" color="success" large>Get Started - It's Free</v-btn>
            </v-flex>
          </v-layout>
        </v-container>
      </section>

      <landing-footer></landing-footer>
    </div>

    </v-content>
  </v-app>
</template>

<script>
import UrlConfig from '../api/UrlConfig';
import LandingHeader from "../components/common/LandingHeader";
import LandingFooter from "../components/common/LandingFooter";

export default {
  name: "home",
  components: {
    LandingHeader,
    LandingFooter
  },
  data: function(){
    return {
      carouselItems:[
        {src:require('../images/slide-1.png'), caption:"Create a board for any project, give it a name, and invite your team."},
        {src:require('../images/slide-2.png'), caption:"Add lists to create steps in a workflow that is right for you."},
        {src:require('../images/slide-3.png'), caption:"Create cards for tasks to complete or information you want to organize."},
        {src:require('../images/slide-4.png'), caption:"Click on a card to add details, due dates, checklists, comments, and more."},
        {src:require('../images/slide-5.png'), caption:"Move them across lists to show progress. Go from \"To Do\" to \"Done\" in no time!"},
      ]
    }
  }
};
</script>

<style scoped>
.lh-1{
  line-height: 1.4em !important;
}
.ls-1{
  letter-spacing: 1px !important;
}
.mw-1{
  max-width: 1200px !important;
}
.mw-2{
  max-width: 900px !important;
}
.mw-3{
  max-width: 650px !important;
}
.no-shadow{
  box-shadow: none;
}
.carousel{
  position:absolute; bottom:9%; width:100%; background:rgba(0,0,0,0.5); color:white; text-align:center;
}
.btn-app1{
  background: url("../images/btn-appstore-black.png") no-repeat center center;
  background-size: contain;
  width:7em;
}
.btn-app2{
  background: url("../images/btn-playstore-black.png") no-repeat center center;
  background-size: contain;
  width:7.5em;
}
.title-section{
  background: linear-gradient(
          rgba(0, 0, 0, 0.6), 
          rgba(0, 0, 0, 0.6)
        ),
        url("../images/network-3537401_1280.jpg") center no-repeat;
  background-size: cover;
}
</style>